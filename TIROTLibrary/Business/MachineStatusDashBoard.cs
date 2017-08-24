using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TIROTLibrary.Business
{
    public class MachineStatusDashBoard
    {
        public int RunningCnt { get; set; } = 0;
        public int StopCnt { get; set; } = 0;
        public int SetupCnt { get; set; } = 0;
        public int QACnt { get; set; } = 0;
        public int OffCnt { get; set; } = 0;
        public float RunningPercent { get; set; } = 0;
        public float StopPercent { get; set; } = 0;
        public float SetupPercent { get; set; } = 0;
        public float QAPercent { get; set; } = 0;
        public float OffPercent { get; set; } = 0;
    }
}
