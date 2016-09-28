using MetrologyAdmin.Server.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.FakeData
{
    public class FakeServersService: IServersService
    {
        public RoadServer[] Servers
        {
            get { return ServersMock.Instance.GetAll().ToArray(); }
        }
    }
}
