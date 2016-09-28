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
    public class DebugServersService: ServersService
    {
        protected override IDbConnection CreateConnection()
        {
            var sb = new SqlConnectionStringBuilder()
            {
                DataSource = @"10.144.10.106\SQLEXPRESS2008",
                InitialCatalog = "MetrologyReplication",
                UserID = "sa",
                Password = "admin",
                ConnectTimeout = 120
            };
            return new SqlConnection(sb.ToString());
        }
        
    }
}
