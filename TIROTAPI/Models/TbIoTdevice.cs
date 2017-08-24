using System;
using System.Collections.Generic;

namespace TIROTAPI.Models
{
    public partial class TbIoTdevice
    {
        public string IoTdeviceId { get; set; }
        public string IoTdeviceDesc { get; set; }
        public string IoTmodelId { get; set; }
        public string MachineId { get; set; }
        public string IoTdeviceMacAddress { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual TbIoTmodel IoTmodel { get; set; }
    }
}
