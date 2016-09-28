using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Server.Core
{
    public interface IServersService
    {
        RoadServer[] Servers { get; }  
    }
}
