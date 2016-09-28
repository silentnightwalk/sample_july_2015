using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Server.Core
{
    public class RoadServer
    {
        public Guid? GuidId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string Catalog { get; set; }
        public int? OrgRoot { get; set; }
        public string NetAddress { get; set; }
    }

    public static class RoadServerHelper
    {
        public static string ToConnectionString(this RoadServer server)
        {
            var sb = new SqlConnectionStringBuilder()
            {
                DataSource = server.NetAddress,//+",1433",
                //NetworkLibrary = "DBMSSOCN",
                InitialCatalog = server.Catalog,
                UserID = "metr_admin",
                Password = "cniitei",
                ConnectTimeout = 120
            };

            return sb.ToString();
        }
    }
}
