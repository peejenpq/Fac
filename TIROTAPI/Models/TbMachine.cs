using System;
using System.Collections.Generic;

namespace TIROTAPI.Models
{
    public partial class TbMachine
    {
        public TbMachine()
        {
            TbIoTdevicePort = new HashSet<TbIoTdevicePort>();
        }

        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public string MachineWorkStatus { get; set; }
        public string MachineWorkSchduleType { get; set; }
        public string WorkSchdule { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<TbIoTdevicePort> TbIoTdevicePort { get; set; }
    }
}
