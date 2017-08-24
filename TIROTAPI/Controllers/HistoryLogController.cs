using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TIROTAPI.Models;
using TIROTLibrary.Business;
using TIROTAPI.Services;
using TIROTAPI.DataAccess;
using TIROTAPI.DataAccess.DataModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TIROTAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HistoryLogController : Controller
    {

        private tirotdbContext _context;
        private IThingService _thingService;


        public HistoryLogController(tirotdbContext context, IThingService thingService)
        {
            _context = context;
            _thingService = thingService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Thing> GetCurrent()
        {
            return _thingService.GetCurrentActivity();
        }

        [HttpGet]
        public IEnumerable<MGLogger> GetHistoryByNow()
        {
            var objMongo = new IotLoggerMongo();

            var getDate = DateTime.Now.AddDays(-1);
            //var res = Query<MGLogger>.GTE(p => p.SDateTime, DateTime.SpecifyKind(getDate, DateTimeKind.Local));
            return objMongo.GetLogByDate(getDate);
            //return _db.GetCollection<MGLogger>(_mgCollName).Find(res);
            //var objMongo = new IotLoggerMongo();
            //return objMongo.GetLogByDate(DateTime.Now);


            //ObjectId("58940ffb470b810967334141")
            //var xid = MongoDB.Bson.ObjectId.Parse("58940ffb470b810967334141");
            //return objMongo.GetProduct(xid);

            //return _thingService.GetCurrentActivity();
        }

        [HttpGet]
        public IEnumerable<MGLogger> GetYesterdayLog()
        {
            var objMongo = new IotLoggerMongo();
            //var listsdata = objMongo.GetYesterdayLog();
            //List<MGLogger> retdata = new List<MGLogger>();
            //foreach (var listdata in listsdata)
            //{
            //    retdata.Add(listdata);

            //    Console.WriteLine(listdata.Id);
            //}
            //return retdata;
            return objMongo.GetYesterdayLog();
        }

        [HttpGet]
        public MGLogger GetLogTest()
        {
            var objMongo = new IotLoggerMongo();
            //return objMongo.GetLogByDate(DateTime.Now);


            //ObjectId("58940ffb470b810967334141")
            //var xid = MongoDB.Bson.ObjectId.Parse("58940ffb470b810967334141");
            return objMongo.GetProduct("0f20bfb1-187c-4305-b4db-1a20c5c937d2");

            //return _thingService.GetCurrentActivity();
        }

        [HttpGet]
        public IEnumerable<MGLogger> GetLogByStartDate(DateTime startDate)
        {
            var objMongo = new IotLoggerMongo();
            return objMongo.GetLogByDate(startDate);
        }

        [HttpGet]
        public IEnumerable<MGLogger> GetLogByFromToDate(DateTime startDate,DateTime endDate)
        {
            var objMongo = new IotLoggerMongo();
            return objMongo.GetLogByDate(startDate, endDate);
        }

        [HttpGet]
        public IEnumerable<MGLogger> GetLogByMachineAndDate(string MachineID, DateTime LogDate)
        {
            var objMongo = new IotLoggerMongo();
            return objMongo.GetLogByMachineAndDate(MachineID, LogDate);
        }

        [HttpGet]
        public IEnumerable<MGLogger> GetLogByMachinePortAndDate(string MachineID,string portNo, DateTime LogDate)
        {
            var objMongo = new IotLoggerMongo();
            return objMongo.GetLogByMachinePortAndDate(MachineID,portNo, LogDate);
        }


        [HttpGet]
        public IEnumerable<MachineStatusLog> GetMachineStatusByDate(DateTime LogDate)
        {
            var objMongo = new MachineRunningStatus();
            return objMongo.GetMachineStatusByDate(LogDate);
        }

        [HttpGet]
        public IEnumerable<MachineStatusLog> GetMachineStatusByIdAndDate(string MachineID, DateTime LogDate)
        {
            var objMongo = new MachineRunningStatus();
            return objMongo.GetMachineStatusByIdAndDate(MachineID,LogDate);
        }

        //[HttpGet]
        //public IEnumerable<MachineTimelineReport> GetJSONMachineTimelineByDate(DateTime LogDate)
        //{

        //    List<MachineTimelineReport> retTimeline = new List<MachineTimelineReport>();

        //    foreach (var objThing in _thingService.GetCurrentActivity())
        //    {
        //        var objMc = new MachineTimelineReport();
        //        objMc.MachineName = objThing.Name;
        //        objMc.Data = new List<MachineTimelineReportData>();
        //        var objMongo = new MachineRunningStatus();
        //        var objTimeLine = objMongo.GetMachineStatusByIdAndDate(objThing.Id, LogDate);

        //        for (int i = 0; i < objTimeLine.Count(); i++)
        //        {
        //            var objData = new MachineTimelineReportData();

        //            objData.StartDateTime = objTimeLine.ElementAt(i).CDateTime.ToLocalTime();
        //            objData.Status = objTimeLine.ElementAt(i).MachineRunningStatus;

        //            if (i >= objTimeLine.Count() - 1)
        //            {
        //                objData.EndDateTime = objTimeLine.ElementAt(i).CDateTime.ToLocalTime();
        //            }
        //            else
        //            {
        //                objData.EndDateTime = objTimeLine.ElementAt(i + 1).CDateTime.ToLocalTime();
        //            }

        //            objMc.Data.Add(objData);

        //        }
        //        retTimeline.Add(objMc);
        //    }
        //    return retTimeline;
        //}


        [HttpGet]
        public IEnumerable<MachineTimelineReport> GetMachineTimelineByDate(DateTime LogDate)
        {
            // ------------------- Set Start Time to 08:00:00 AM ------------------------- 
            LogDate = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, 8, 0, 0, DateTimeKind.Local);
            // ---------------------------------------------------------------------------

            List<MachineTimelineReport> retTimeline = new List<MachineTimelineReport>();

            foreach (var objThing in _thingService.GetCurrentActivity())
            {
                var objMc = new MachineTimelineReport();
                objMc.measure = objThing.Name;
                objMc.data = new List<string>();
                var objMongo = new MachineRunningStatus();
                var objTimeLine = objMongo.GetMachineStatusByIdAndDate(objThing.Id, LogDate);

                if (objTimeLine.Count() > 0)
                {
                    for (int i = 0; i < objTimeLine.Count(); i++)
                    {
                        string objData = "";

                        //objData.StartDateTime = objTimeLine.ElementAt(i).CDateTime.ToLocalTime();
                        //objData.Status = objTimeLine.ElementAt(i).MachineRunningStatus;

                        if (i >= objTimeLine.Count() - 1)
                        {
                            DateTime endday;

                            if (objTimeLine.ElementAt(i).CDateTime.ToLocalTime().ToString("yyyyMMdd") != DateTime.Now.ToString("yyyMMdd"))
                            {
                                //endday = new DateTime(objTimeLine.ElementAt(i).CDateTime.Year, objTimeLine.ElementAt(i).CDateTime.Month, objTimeLine.ElementAt(i).CDateTime.Day, 23, 59, 59);
                                endday = LogDate.AddDays(1);
                            }
                            else
                            {
                                endday = DateTime.Now;
                            }



                            objData = "[\"" + objTimeLine.ElementAt(i).CDateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") +
                                        @"""," + objTimeLine.ElementAt(i).MachineRunningStatus + @",""" +
                                        endday.ToString("yyyy-MM-dd HH:mm:ss") + "\"]";
                            //objData.EndDateTime = objTimeLine.ElementAt(i).CDateTime.ToLocalTime();
                        }
                        else
                        {
                            objData = "[\"" + objTimeLine.ElementAt(i).CDateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") +
                                          @"""," + objTimeLine.ElementAt(i).MachineRunningStatus + @",""" +
                                        objTimeLine.ElementAt(i + 1).CDateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") + "\"]";
                            //objData.EndDateTime = objTimeLine.ElementAt(i + 1).CDateTime.ToLocalTime();
                        }

                        objMc.data.Add(objData);

                    }
                } else
                {
                    DateTime dayenddt = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, 12, 0, 0, DateTimeKind.Local);
                    string objData = "[\"" + dayenddt.ToString("yyyy-MM-dd HH:mm:ss") +
                                        @""",0,""" + dayenddt.ToString("yyyy-MM-dd HH:mm:ss") + "\"]";
                    objMc.data.Add(objData);
                }
                retTimeline.Add(objMc);
            }
            return retTimeline;
        }

        [HttpGet]
        public IEnumerable<MachineDailyStatus> GetMachineDailySummaryByDate(DateTime LogDate)
        {

            // ------------------- Set Start Time to 08:00:00 AM ------------------------- 
            LogDate = new DateTime(LogDate.Year, LogDate.Month, LogDate.Day, 8, 0, 0, DateTimeKind.Local);
            // ---------------------------------------------------------------------------

            List<MachineDailyStatus> retReport = new List<MachineDailyStatus>();

            foreach (var objThing in _thingService.GetCurrentActivity())
            {
                var objMc = new MachineDailyStatus();
                objMc.MachineID = objThing.Id;
                objMc.MachineName = objThing.Name;
                
                var objMongo = new MachineRunningStatus();
                var objTimeLine = objMongo.GetMachineStatusByIdAndDate(objThing.Id, LogDate);

                string lastStatus = "";

                for (int i = 0; i < objTimeLine.Count(); i++)
                {

                    if (i < objTimeLine.Count() - 1)
                    {

                        if (lastStatus != objTimeLine.ElementAt(i).MachineRunningStatus)
                        {
                            switch (objTimeLine.ElementAt(i).MachineRunningStatus)
                            {
                                case "0":
                                    objMc.OffCnt++;
                                    break;
                                case "1":
                                    objMc.RunCnt++;
                                    break;
                                case "2":
                                    objMc.SetupCnt++;
                                    break;
                                case "3":
                                    objMc.StopCnt++;
                                    break;
                                case "4":
                                    objMc.QACnt++;
                                    break;
                            }
                        }

                        switch (objTimeLine.ElementAt(i).MachineRunningStatus)
                        {
                            case "0":
                                objMc.OffTime += (objTimeLine.ElementAt(i + 1).CDateTime - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "1":
                                objMc.RunTime += (objTimeLine.ElementAt(i + 1).CDateTime - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "2":
                                objMc.SetupTime += (objTimeLine.ElementAt(i + 1).CDateTime - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "3":
                                objMc.StopTime += (objTimeLine.ElementAt(i + 1).CDateTime - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "4":
                                objMc.QATime += (objTimeLine.ElementAt(i + 1).CDateTime - objTimeLine.ElementAt(i).CDateTime);
                                break;
                        }
                    }
                    else
                    {
                        // Case Last Record 
                        if (lastStatus != objTimeLine.ElementAt(i).MachineRunningStatus)
                        {
                            switch (objTimeLine.ElementAt(i).MachineRunningStatus)
                            {
                                case "0":
                                    objMc.OffCnt++;
                                    break;
                                case "1":
                                    objMc.RunCnt++;
                                    break;
                                case "2":
                                    objMc.SetupCnt++;
                                    break;
                                case "3":
                                    objMc.StopCnt++;
                                    break;
                                case "4":
                                    objMc.QACnt++;
                                    break;
                            }
                        }

                        DateTime endday;

                        if (objTimeLine.ElementAt(i).CDateTime.ToLocalTime().ToString("yyyyMMdd") != DateTime.Now.ToString("yyyMMdd"))
                        { 
                            //endday = new DateTime(objTimeLine.ElementAt(i).CDateTime.Year, objTimeLine.ElementAt(i).CDateTime.Month, objTimeLine.ElementAt(i).CDateTime.Day,23, 59, 59);
                            endday = LogDate.AddDays(1);
                        } else
                        {
                            endday = DateTime.Now.ToUniversalTime();
                        }

                        switch (objTimeLine.ElementAt(i).MachineRunningStatus)
                        {
                            case "0":
                                objMc.OffTime += (endday - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "1":
                                objMc.RunTime += (endday - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "2":
                                objMc.SetupTime += (endday - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "3":
                                objMc.StopTime += (endday - objTimeLine.ElementAt(i).CDateTime);
                                break;
                            case "4":
                                objMc.QATime += (endday - objTimeLine.ElementAt(i).CDateTime);
                                break;
                        }
                    }

                    lastStatus = objTimeLine.ElementAt(i).MachineRunningStatus;
                }
                retReport.Add(objMc);
            }
            return retReport;
        }

        [HttpGet]
        public IEnumerable<MachineStatusLog> GetMachineDailyLogByDate(DateTime LogDate)
        {
            var objMongo = new MachineRunningStatus();
            return objMongo.GetMachineStatusLogByDate(LogDate);
        }

        [HttpGet]
        public MachineStatusDashBoard GetCurrentStatusDashboard()
        {
            var objRe = new MachineStatusDashBoard();
            foreach (var objthing in _thingService.GetCurrentActivity())
            {
                switch (objthing.OnlineStatus)
                {
                    case "0":
                        objRe.OffCnt++;
                        break;
                    case "1":
                        objRe.RunningCnt++;
                        break;
                    case "2":
                        objRe.SetupCnt++;
                        break;
                    case "3":
                        objRe.StopCnt++;
                        break;
                    case "4":
                        objRe.QACnt++;
                        break;
                }
            }
            return objRe;
        }
    }
}
