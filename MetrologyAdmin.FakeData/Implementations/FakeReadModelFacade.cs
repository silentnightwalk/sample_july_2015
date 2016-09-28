using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.FakeData
{
    public class FakeReadModelFacade: IReadModelFacade
    {

        public Organization[] GetOrganizationsTree(int serverId)
        {
            return OrganizationsMock.Instance.GetOrganizationTree(serverId);
        }

        public User[] GetUsersByOrganization(int serverId, int organizationId, bool aggregate)
        {
            if (aggregate)
            {
                var orgTree = OrganizationsMock.Instance.GetOrganizationTree(serverId).ToArray();
                var allOrgs = Organization.AsEnumerable(orgTree).ToArray();

                var root = allOrgs.Where(x => x.Id == organizationId).ToArray();

                return Organization.AsEnumerable(root)
                    .SelectMany(o => UsersMock.Instance
                        .GetAll(serverId)
                        .Where(x => x.OrganizationId == o.Id))
                    .ToArray();
            }
            else
            {
                return UsersMock.Instance
                    .GetAll(serverId)
                    .Where(x => x.OrganizationId == organizationId)
                    .ToArray();
            }
        }

        public Role[] GetAllRoles(int serverId)
        {
            return new Role[] {
                new Role { Id = 1, Name = "sys admin" },
                new Role { Id = 2, Name = "bd admin" },
                new Role { Id = 3, Name = "metrologist" },
                new Role { Id = 4, Name = "good mate" }
            };
        }


        public User GetUserById(int serverId, int userId)
        {
            return UsersMock.Instance
                .GetAllDto(serverId)
                .Cast<User>()
                .First(x => x.Id == userId);
        }


        public User GetUserByLoginDetails(int serverId, string login, string password)
        {
            return UsersMock.Instance
                .GetAllDto(serverId)
                .Where(x => x.AccessCode == password && x.Login == login)
                .Cast<User>()
                .First();
        }
    }
}
