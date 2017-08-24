using System;
using System.Collections.Generic;

namespace TIROTAPI.Models
{
    public partial class TbUdlog
    {
        public string Id { get; set; }
        public DateTime? ServerDateTime { get; set; }
        public string Indata { get; set; }
    }
}
