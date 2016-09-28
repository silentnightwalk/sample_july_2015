using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Server.Core
{
    public interface IAuthorizationService
    {
        bool IsAuthorized { get; }
        void Authorize(int serverId, string login, string password);
    }
}
