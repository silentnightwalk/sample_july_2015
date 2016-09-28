using MetrologyAdmin.Core;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.FakeData
{
    public class ServersMock : SingletonBase<ServersMock>
    {
        private ServersMock()
        {
            LoadServers();
        }

        private RoadServer[] LoadServers()
        {
            var servers = new List<RoadServer>();
            servers.Add(new RoadServer() { Id = 1, Name = "Кронштадская дорога" });
            servers.Add(new RoadServer() { Id = 2, Name = "Питерская дорога" });
            return servers.ToArray();
        }

        public RoadServer[] GetAll()
        {
            return LoadServers();
        }
    }
}
