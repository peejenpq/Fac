using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIROTLibrary.Business
{
    public class Thing
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartWorkTime { get; set; }
        public string EndWorkTime { get; set; }
        public DateTime? LastUpdateDateTime { get; set; }
        // Device Current Status for On/Off/Network issue/Under Maintenance
        public PortStatus ThingWorkStatus { get; set; }
        // Contain All Values of Me
        public List<PortStatus> ThingPortsStatus { get; set; }
        /// <summary>
        /// 0 - Turn off, 1 - Online, 2 - Setup, 3 - Offline, 4 - QA
        /// </summary>
        public string OnlineStatus { get; set; }

    }
}
