using System;
using System.Collections.Generic;

namespace TIROTAPI.Models
{
    public partial class TbIoTmodel
    {
        public TbIoTmodel()
        {
            TbIoTdevice = new HashSet<TbIoTdevice>();
        }

        public string IoTmodelId { get; set; }
        public string IoTmodelName { get; set; }
        public int? InputChannel { get; set; }
        public int? OutputChannel { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<TbIoTdevice> TbIoTdevice { get; set; }
    }
}
