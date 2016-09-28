using MetrologyAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class ReportViewModel
    {
        public string ReportName { get; private set; }
        public string ServerName { get; private set; }
        public string OrganizationName { get; private set; }
        public string FilterInfo { get; set; }
        public ExcelUser[] Users { get; set; }

        public ReportViewModel(string reportName, string serverName, string organizationName, ExcelUser[] users, string filterInfo = "")
        {
            ReportName = reportName ?? "";
            ServerName = serverName ?? "";
            FilterInfo = filterInfo ?? "";
            OrganizationName = organizationName ?? "";
            Users = users;
        }
    }
}
