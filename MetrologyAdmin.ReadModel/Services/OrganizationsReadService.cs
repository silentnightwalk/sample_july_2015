using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using MetrologyAdmin.Server.Core;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Diagnostics;

namespace MetrologyAdmin.ReadModel
{
    public class OrganizationsReadService
    {
        private static readonly MemoryCache _Cache = new MemoryCache("OrganizationsReadServiceCache");
        private static readonly object CacheLock = new object();

        private readonly string _CacheKey;

        private IDbConnection _connection;

        private readonly Guid RzdRoot = new Guid("A1BE19EC-E360-491D-81BF-8F3C418DBEF9");
        private readonly Guid ServiceCompaniesRoot = new Guid("BB78805C-9C46-4A70-B8EC-B93E629FF515");
        private readonly Guid DirectionsRoot = new Guid("89BECC1F-1763-4FE8-853D-7B7E2119C44E");
        private readonly Guid RailwaysRoot = new Guid("F4A9666B-73F9-4337-AA4A-34EF6D274BE7");

        public OrganizationsReadService(IDbConnection connection)
        {
            _connection = connection;
            _CacheKey = _connection.ConnectionString;
        }

        public Organization[] GetOrganizationsTree()
        {
            lock (CacheLock)
            {
                var cachedData = _Cache[_CacheKey] as Organization[];
                if (cachedData == null)
                {
                    cachedData = GetOrganizationsTreeFromDB();
                    _Cache[_CacheKey] = cachedData;
                }
                //else
                //{
                //    Debug.WriteLine("FROM CACHE", "OrganizationsReadService");
                //}
                return cachedData;
            }
        }

        private Organization[] GetOrganizationsTreeFromDB()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(assembly.GetManifestResourceStream("MetrologyAdmin.ReadModel.Sql.TreeQuery.sql"));

            var sql = reader.ReadToEnd();

            var organizations = _connection.Query<Organization>(sql).ToArray();

            var result = organizations
                .Where(o => o.RowGuid == RzdRoot || o.RowGuid == ServiceCompaniesRoot)
                .OrderBy(o => o.RowGuid == RzdRoot ? 1 : 2)
                .ToArray();

            //расстановка детей и родителей
            var grouped = organizations.GroupBy(o => o.ParentId);
            foreach (var group in grouped)
            {
                var groupHead = organizations.FirstOrDefault(x => x.Id == group.Key);

                if (groupHead != null)
                    groupHead.Children = group.ToArray();

                foreach (var member in group)
                {
                    member.Parent = groupHead;
                }
            }

            //расстановка названий 3 полей (филиалов и тд) 
            SetNames(result);

            return result;
        }


        private void SetNames(IEnumerable<Organization> levelOrgs)
        {
            foreach (var org in levelOrgs)
            {
                var parent = org.Parent;

                if (parent == null)
                {
                    if (org.RowGuid == RzdRoot)
                        org.SetNamesInfo(OrganizationType.OaoRzd, "-", "-", "-");

                    if (org.RowGuid == ServiceCompaniesRoot)
                        org.SetNamesInfo(OrganizationType.ServComp_Directions_Railwais, "-", "-", "-");
                }
                else
                {
                    if (org.Parent.OrganizationType == OrganizationType.OaoRzd)
                        org.SetNamesInfo(OrganizationType.ServComp_Directions_Railwais, "-", "-", "-");

                    if (org.Parent.OrganizationType == OrganizationType.ServComp_Directions_Railwais)
                        org.SetNamesInfo(OrganizationType.Filial, org.Name, "-", "-");

                    if (org.Parent.OrganizationType == OrganizationType.Filial)
                        org.SetNamesInfo(OrganizationType.Division, parent.FilialName, org.Name, "-");

                    if (org.Parent.OrganizationType == OrganizationType.Division)
                        org.SetNamesInfo(OrganizationType.UnitOrSubdivision, parent.FilialName, parent.DivisionName, org.Name);

                    if (org.Parent.OrganizationType == OrganizationType.UnitOrSubdivision)
                        org.SetNamesInfo(OrganizationType.UnitOrSubdivision, parent.FilialName, parent.DivisionName, org.Name);
                }
            }

            var downLevel = levelOrgs.SelectMany(o => o.Children != null ? o.Children : Enumerable.Empty<Organization>());

            if (downLevel != null && downLevel.Count() > 0)
                SetNames(downLevel);
        }

    }
}
