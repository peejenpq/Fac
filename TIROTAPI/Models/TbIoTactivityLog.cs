using System;
using System.Collections.Generic;

namespace TIROTAPI.Models
{
    public partial class TbIoTactivityLog
    {
        public string MachineId { get; set; }
        public DateTime ClientDateTime { get; set; }
        public int SensorId { get; set; }
        public double? SensorValue { get; set; }
        public DateTime? ServerDateTime { get; set; }
    }
}
