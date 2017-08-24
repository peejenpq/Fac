using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIROTLibrary.Business
{
    public class PortStatus
    {
        public string IoTdevicePortID { get; set; }
        public int IoTdevicePort { get; set; }
        public int? SensorType { get; set; }
        public string MeasurementType { get; set; }
        public double? Value { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public DateTime? LastUpdateDateTime { get; set; }
    }
}
