using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public class User
    {
        public int Id { get; set; }
        public int? OrganizationId { get; set; }
        public string Organization { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        //public string AccessCode { get; set; }
        public string Telephone { get; set; }
        public string EMail { get; set; }
        public string Post { get; set; }

        public string FilialName { get; set; }
        public string DivisionName { get; set; }
        public string SubDivisionName { get; set; }

        public int? RoleId { get; set; }
        public string Role { get; set; }
    }
}
