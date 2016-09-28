using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetrologyAdmin.Core
{
    public class RoadServerViewModel
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string BdName { get; private set; }

        public RoadServerViewModel(int id, string name, string address, string bdName)
        {
            Id = id;
            Name = name;
            Address = address;
            BdName = bdName;
        }
    }
}
