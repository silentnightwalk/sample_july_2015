using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class UserViewModel: User
    {
        //public string FilialName { get; private set; }
        //public string DivisionName { get; private set; }
        //public string SubDivisionName { get; private set; }

        public UserViewModel(User baseUser) //, string filialName, string divisionName, string subDivisionName)
        {
            this.EMail = baseUser.EMail;
            this.Id = baseUser.Id;
            this.Login = baseUser.Login;
            this.Name = baseUser.Name;
            this.Organization = baseUser.Organization;
            this.OrganizationId = baseUser.OrganizationId;
            this.Post = baseUser.Post;
            this.Role = baseUser.Role;
            this.RoleId = baseUser.RoleId;
            this.Telephone = baseUser.Telephone;

            this.FilialName = baseUser.FilialName;
            this.DivisionName = baseUser.DivisionName;
            this.SubDivisionName = baseUser.SubDivisionName;
            //FilialName = filialName;
            //DivisionName = divisionName;
            //SubdivisionName = subDivisionName;
        }

        
    }
}
