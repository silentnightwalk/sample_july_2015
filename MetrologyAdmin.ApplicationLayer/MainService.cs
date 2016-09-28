using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetrologyAdmin;
using MetrologyAdmin.Core;
using MetrologyAdmin.Server;
using MetrologyAdmin.Server.Core;
using Autofac;

namespace MetrologyAdmin.ApplicationLayer
{
    public class MainService
    {

        private IReadModelFacade _ReadModel;
        public IReadModelFacade ReadModel
        {
            get { return _ReadModel; }
            set { _ReadModel = value; }
        }

        private IServersService _serversService;
        private IAuthorizationService _authorizationService;

        public MainService(IReadModelFacade readModelFacade, IServersService serversService, IAuthorizationService authorizationService)
        {

            _ReadModel = readModelFacade;
            _serversService = serversService;
            _authorizationService = authorizationService;
        }

        public RoadServerViewModel[] GetServersList()
        {
            return _serversService.Servers.Select(x => new RoadServerViewModel(x.Id, x.Name,x.NetAddress,x.Catalog)).ToArray();
        }

        public RoadServerViewModel GetServerById(int serverId)
        {
            return _serversService.Servers.Select(x => new RoadServerViewModel(x.Id, x.Name, x.NetAddress, x.Catalog)).First(x => x.Id == serverId);
        }

        public bool Authorize(int serverId, string login, string password)
        {
            _authorizationService.Authorize(serverId, login, password);
            return _authorizationService.IsAuthorized;
        }

    }
}
