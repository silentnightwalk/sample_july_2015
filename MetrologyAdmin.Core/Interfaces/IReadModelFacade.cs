using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public interface IReadModelFacade
    {
        Organization[] GetOrganizationsTree(int serverId);

        User[] GetUsersByOrganization(int serverId, int organizationId, bool aggregate);

        Role[] GetAllRoles(int serverId);

        User GetUserById(int serverId, int userId);

        User GetUserByLoginDetails(int serverId, string login, string password);

    }
}
