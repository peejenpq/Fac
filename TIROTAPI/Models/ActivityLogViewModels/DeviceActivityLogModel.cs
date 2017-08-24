using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIROTAPI.Models.ActivityLogViewModels
{
    public class DeviceActivityLogModel
    {
        public DateTime ClientDateTime { get; set; }
        public string MachineID { get; set; }
        public int DevicePort { get; set; }
        public double SensorValue { get; set; }
    }
}
