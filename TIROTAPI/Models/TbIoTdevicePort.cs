using System;
using System.Collections.Generic;

namespace TIROTAPI.Models
{
    public partial class TbIoTdevicePort
    {
        public string IoTdeviceMacAddress { get; set; }
        public int IoTdevicePort { get; set; }
        public int? SensorType { get; set; }
        public string MeasurementId { get; set; }
        public string MachineId { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual TbMachine Machine { get; set; }
    }
}
