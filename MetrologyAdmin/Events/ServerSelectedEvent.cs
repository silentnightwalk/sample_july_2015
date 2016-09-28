using MetrologyAdmin.Core;
using MetrologyAdmin.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin
{
    public class ServerSelectedEvent: IEvent 
    {
        //public int SelectedServerId { get; private set; }
        public RoadServerViewModel SelectedServer { get; private set; }

        public ServerSelectedEvent(RoadServerViewModel selectedServer)
        {
            //SelectedServerId = selectedServerId;
            SelectedServer = selectedServer;
        }
    }
}
