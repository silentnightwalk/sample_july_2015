using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public class RoleLocalNames
    {
        public static IDictionary<int, string> Names = new Dictionary<int, string>() 
        { 
            { AdministratorBd, "Bd_admin" }, 
            { AdministratorSys, "Sys_admin" }, 
            { Manager, "Manager" }, 
            { Ctech, "Ctech" } 
        };

        public const int AdministratorBd = 1;
        public const int AdministratorSys = 2;
        public const int Manager = 3;
        public const int Ctech = 4;
    }
}
