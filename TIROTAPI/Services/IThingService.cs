using System.Collections.Generic;
using TIROTLibrary.Business;
using TIROTAPI.Models.ActivityLogViewModels;

namespace TIROTAPI.Services
{
    public interface IThingService
    {
        List<Thing> GetCurrentActivity();
        Thing GetCurrentActivityByMachine(string MachineID);
        void SetCurrentActivity(DeviceActivityLogModel inLog);
    }
}
