using System;

namespace TIROTLibrary.Business
{
    public class MachineStatusLog
    {       
            public string Id { get; set; }
            public string MachineID { get; set; }
            public string MachineName { get; set; }
            public DateTime CDateTime { get; set; }
            public DateTime SDateTime { get; set; }
            public string MachineRunningStatus { get; set; }
    }
}

