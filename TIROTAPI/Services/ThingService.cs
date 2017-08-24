using System;
using System.Collections.Generic;
using System.Linq;
using TIROTLibrary.Business;
using TIROTAPI.Models;
using TIROTAPI.Models.ActivityLogViewModels;
using TIROTAPI.DataAccess;
namespace TIROTAPI.Services
{
    public class ThingService : IThingService
    {
        private List<Thing> _thing;

        public ThingService(tirotdbContext context)
        {
            _thing = new List<Thing> { };
            
            // Get Machine
            var objMachine = from x in context.TbMachine orderby x.MachineId select x;

            foreach (TbMachine mch in objMachine)
            {
                var objThing = new Thing();
                objThing.Id = mch.MachineId;
                objThing.Name = mch.MachineName;
                objThing.StartWorkTime = "00:00";
                objThing.EndWorkTime = "00:00";

                // Get and Update Last Status from MongoDB
                var objMongo = new MachineRunningStatus();
                var objTimeLine = objMongo.GetLastMachineStatus(objThing.Id);

                if (objTimeLine != null && objTimeLine.Count() > 0)
                {
                    objThing.OnlineStatus = objTimeLine.ElementAt(0).MachineRunningStatus;
                    objThing.LastUpdateDateTime = objTimeLine.ElementAt(0).CDateTime.ToLocalTime();
                } else
                {
                    objThing.OnlineStatus = "0";
                    objThing.LastUpdateDateTime = DateTime.Now;
                }
                

                //objThing.ThingWorkStatus = new PortStatus();
                objThing.ThingPortsStatus = new List<PortStatus> {};

                //var objPort = from y in context.TbIoTdevicePort select y;
                _thing.Add(objThing);
            }
            // Get Port
            var objPort = from x in context.TbIoTdevicePort select x ;
            // Add Port
            foreach (TbIoTdevicePort dp in objPort)
            {
                var objPS = new PortStatus();
                objPS.IoTdevicePortID = dp.IoTdevicePort.ToString();
                objPS.IoTdevicePort = dp.IoTdevicePort;
                objPS.SensorType = dp.SensorType;
                objPS.MeasurementType = dp.MeasurementId;

                var tmpThing = _thing.First(x => x.Id.Contains(dp.MachineId));
                if (tmpThing != null)
                {
                    tmpThing.ThingPortsStatus.Add(objPS);
                }
            }
        }

        public void SetCurrentActivity(DeviceActivityLogModel inLog)
        {
            var tmpMn = _thing.First(x => x.Id.Contains(inLog.MachineID));
            var tmpPt = tmpMn.ThingPortsStatus.First(x => x.IoTdevicePortID.Contains(inLog.DevicePort.ToString()));
            if (tmpPt != null)
            {
                tmpPt.Value = inLog.SensorValue;
                tmpPt.LastUpdateDateTime = inLog.ClientDateTime;
            }
            //_thing.Add(inLog);
        }
        /// <summary>
        /// Return all Machine activity
        /// </summary>
        /// <returns></returns>
        public List<Thing> GetCurrentActivity()
        {
            return _thing.ToList();
        }
        /// <summary>
        /// Return Current Activiry by Machine 
        /// </summary>
        /// <param name="MachineID"></param>
        /// <returns></returns>
        public Thing GetCurrentActivityByMachine(string MachineID)
        {
            return _thing.FirstOrDefault(x => x.Id.Contains(MachineID));
        }
    }
}
