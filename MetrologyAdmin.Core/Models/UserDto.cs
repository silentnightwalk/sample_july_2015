using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public class UserDto: User
    {

        public string AccessCode { get; set; }
        public int ServerId { get; set; }

        public UserDto()
        {

        }

        public UserDto(User baseUser, int serverId, string accessCode)
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

            this.ServerId = serverId;
            this.AccessCode = accessCode;
        }

        public void SetId(int newId)
        {
            Id = newId;
        }
    }
}
