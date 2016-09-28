using MetrologyAdmin.Core;
using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Server
{
    public class ConnectionFactory: IConnectionFactory
    {
        private IServersService _serversService;

        public ConnectionFactory(IServersService serversService)
        {
            _serversService = serversService;
        }

        public IDbConnection Create(int serverId)
        {
            string connectionString =
                _serversService
                    .Servers.First(x => x.Id == serverId)
                    .ToConnectionString();

            return new SqlConnection(connectionString);
        }
    }
}
