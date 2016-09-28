using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public class ExcelUser
    {
        public string Id { get; private set; }
        //public int? OrganizationId { get; private set; }
        public string Organization { get; private set; }
        public string Name { get; private set; }
        public string Login { get; private set; }
        public string Telephone { get; private set; }
        public string EMail { get; private set; }
        public string Post { get; private set; }
        //public int? RoleId { get; private set; }
        public string Role { get; private set; }
        public string FilialName { get; set; }
        public string DivisionName { get; set; }
        public string SubDivisionName { get; set; }

        public int Order { get; set; }

        public ExcelUser(User baseUser)
        {
            this.EMail = baseUser.EMail ?? "";
            this.Id = baseUser.Id.ToString();
            this.Login = baseUser.Login ?? "";
            this.Name = baseUser.Name ?? "";
            this.Organization = baseUser.Organization ?? "";
            //this.OrganizationId = baseUser.OrganizationId != null ? baseUser.OrganizationId.Value : "";
            this.Post = baseUser.Post ?? "";
            this.Role = baseUser.Role ?? "";
            //this.RoleId = baseUser.RoleId;
            this.Telephone = baseUser.Telephone ?? "";
            this.FilialName = baseUser.FilialName ?? "";
            this.DivisionName = baseUser.DivisionName ?? "";
            this.SubDivisionName = baseUser.SubDivisionName ?? "";
        }
    }
}
