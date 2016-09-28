using MetrologyAdmin.Core;
using MetrologyAdmin.Server;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace MetrologyAdmin.ReadModel
{
    public class ReadModelFacade: IReadModelFacade
    {
        private IConnectionFactory _connectionFactory;

        public ReadModelFacade(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Organization[] GetOrganizationsTree(int serverId)
        {
            using (var db = _connectionFactory.Create(serverId))
            {
                var orgService = new OrganizationsReadService(db);
                return orgService.GetOrganizationsTree();
            }
        }

        

        public User[] GetUsersByOrganization(int serverId, int organizationId, bool aggregate)
        {
            using (var db = _connectionFactory.Create(serverId))
            {
                var userService = new UsersReadService(db);
                var orgService = new OrganizationsReadService(db);

                User[] result = aggregate
                    ? userService.GetAggregateUsersByOrganization(organizationId)
                    : userService.GetUsersByOrganization(organizationId);

                //preapre data

                var orgs = orgService.GetOrganizationsTree();
                var flat = Organization.AsEnumerable(orgs).ToArray();

                var mapUser = from x in result
                              let org = flat.FirstOrDefault(o=>o.Id == x.OrganizationId)
                              select new { User = x, Org = org };

                foreach (var item in mapUser)
                {
                    if (item.Org != null)
                    {
                        item.User.FilialName = item.Org.FilialName;
                        item.User.DivisionName = item.Org.DivisionName;
                        item.User.SubDivisionName = item.Org.SubdivisionName;
                    }
                }

                return result;   
            }
        }



        public Role[] GetAllRoles(int serverId)
        {
            using (var db = _connectionFactory.Create(serverId))
            {
                var roleService = new RolesReadService(db);
                return roleService.GetRoles();
            }
        }


        public User GetUserById(int serverId, int userId)
        {
            using (var db = _connectionFactory.Create(serverId))
            {
                var orgService  = new OrganizationsReadService(db);
                var userService = new UsersReadService(db);
                var user = userService.GetUserById(userId);
                //TODO: logic to get single organization
                var org = Organization
                    .AsEnumerable(orgService.GetOrganizationsTree())
                    .FirstOrDefault(
                        x =>  user.OrganizationId.HasValue 
                            ? user.OrganizationId.Value == x.Id
                            : false
                            );
                if (org != null && user != null)
                {
                    user.DivisionName = org.DivisionName;
                    user.FilialName = org.FilialName;
                    user.SubDivisionName = org.SubdivisionName;
                }
                return user;
            }
        }

        public User GetUserByLoginDetails(int serverId, string login, string password)
        {
            using (var db = _connectionFactory.Create(serverId))
            {
                var orgService = new OrganizationsReadService(db);
                var userService = new UsersReadService(db);
                var user = userService.GetUserByLoginDetails(login, password);
                //TODO: logic to get single organization
                var org = Organization
                    .AsEnumerable(orgService.GetOrganizationsTree())
                    .FirstOrDefault(
                        x => user.OrganizationId.HasValue
                            ? user.OrganizationId.Value == x.Id
                            : false
                            );
                if (org != null && user != null)
                {
                    user.DivisionName = org.DivisionName;
                    user.FilialName = org.FilialName;
                    user.SubDivisionName = org.SubdivisionName;
                }
                return user;
            }
        }
    }
}
