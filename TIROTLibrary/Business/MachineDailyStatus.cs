using System;

namespace TIROTLibrary.Business
{
    public class MachineDailyStatus
    {
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public TimeSpan RunTime { get; set; }
        public int RunCnt { get; set; } = 0;
        public float RunPercent { get; set; }
        public TimeSpan StopTime { get; set; }
        public int StopCnt { get; set; } = 0;
        public float StopPercent { get; set; }
        public TimeSpan SetupTime { get; set; }
        public int SetupCnt { get; set; } = 0;
        public float SetupPercent { get; } 
        public TimeSpan OffTime { get; set; }
        public int OffCnt { get; set; } = 0;
        public float OffPercent { get; set; }
        public TimeSpan QATime { get; set; }
        public int QACnt { get; set; } = 0;
        public float QAPercent { get; set; } 
        public TimeSpan TotalTime { get { return RunTime + StopTime + SetupTime + OffTime + QATime; } }
        public int TotalCnt { get { return RunCnt + StopCnt + SetupCnt + OffCnt + QACnt;} }
    }
}
