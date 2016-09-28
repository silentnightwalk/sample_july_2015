using MetrologyAdmin.Core;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.FakeData
{
    public class FakeAuthorizationService: IAuthorizationService
    {
        public bool IsAuthorized { get { return _authorizedUser != null;  } }

        private User _authorizedUser;

        public void Authorize(int serverId, string login, string password)
        {
            var allUsers = UsersMock.Instance.GetAll(serverId);
            var u = allUsers.FirstOrDefault(
                x =>
                    //x.AccessCode == "2" &&
                    x.Login == login
                );

            if (u != null)
            {
                _authorizedUser = u;
            }
            else
            {
                throw new Exception("Wrong password or login");
            }
        }
    }
}
