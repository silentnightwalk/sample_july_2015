using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using MetrologyAdmin.Server.Core;
using System.Data.SqlClient;

namespace MetrologyAdmin.Server
{
    public class ServersService: IServersService
    {
        private RoadServer[] _servers;
        public RoadServer[] Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;
            }
        }

        public ServersService()
        {
            _servers = LoadServers();
        }

        protected virtual IDbConnection CreateConnection()
        {
            var sb = new SqlConnectionStringBuilder()
            {
                DataSource = "***",
                InitialCatalog = "***",
                UserID = "***",
                Password = "***",
                ConnectTimeout = 120
            };
            return new SqlConnection(sb.ToString());
        }

        private RoadServer[] LoadServers()
        {
            string sql = @"  SELECT [Guid] as GuidId, 
                                    Id, 
                                    Name, 
                                    ShortName, 
                                    [Address], 
                                    Base as [Catalog],
                                    StructRoot as OrgRoot,
                                    NetAddress 
                             FROM   dbo.Servers 
                             WHERE  [Disabled] IS NULL   ";


            RoadServer[] result;

            using (IDbConnection connection = CreateConnection())
            {
                result = connection.Query<RoadServer>(sql).ToArray();
            }

            return result;
        }

        
    }
}
