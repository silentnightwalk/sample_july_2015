using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public class Organization
    {
        //private Organization BaseOrganization { get; set; }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public Guid RowGuid { get; set; }
        public Guid? NodGuid { get; set; }


        public Organization Parent { get; set; }
        public Organization[] Children { get; set; }

        public string FilialName { get; set; }
        public string DivisionName { get; set; }
        public string SubdivisionName { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public void SetNamesInfo(OrganizationType orgType, string filialName, string divisionName, string subdivisionName)
        {
            OrganizationType = orgType;
            FilialName = filialName;
            DivisionName = divisionName;
            SubdivisionName = subdivisionName;
        }

        public static IEnumerable<Organization> AsEnumerable(IEnumerable<Organization> roots)
        {
            if (roots == null) yield break;

            foreach (var item in roots)
            {
                yield return item;
                foreach (var child in AsEnumerable(item.Children))
                {
                    yield return child;
                }
            }
        }

        public static IEnumerable<Organization> Ancestors(Organization item)
        {
            var parent = item.Parent;
            while (parent != null)
            {
                yield return parent;
                parent = parent.Parent;
            }
        }

    }

    public enum OrganizationType
    {
        None,
        OaoRzd,
        ServComp_Directions_Railwais,
        Filial,
        Division,
        UnitOrSubdivision
    }

}
