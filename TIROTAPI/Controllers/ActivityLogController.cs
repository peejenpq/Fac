using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TIROTAPI.Models.ActivityLogViewModels;
using TIROTAPI.Models;
using TIROTLibrary.Business;
using TIROTAPI.Services;
using TIROTLibrary.SysCommon.Utility;
using Newtonsoft.Json;
using TIROTAPI.DataAccess;
using System.Net.Http;
using TIROTAPI.Models.SensorViewModels;
using System.Net.Http.Headers;
using System.Threading;
using TIROTAPI.DataAccess.DataModels;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TIROTAPI.Controllers
{
    [Route("api/[controller]")]
    public class ActivityLogController : Controller
    {
        private tirotdbContext _context;
        private IThingService _thingService;

        public ActivityLogController(tirotdbContext context, IThingService thingService)
        {
            _context = context;
            _thingService = thingService;
        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<TbIoTactivityLog> Get()
        //{
        //    return _context.TbIoTactivityLog;
        //}

        [HttpGet]
        public IEnumerable<Thing> Get()
        {
            return _thingService.GetCurrentActivity();
        }

        // GET: api/values
        //[HttpGet]
        //public IEnumerable<Thing> GetCurrent()
        //{
        //    return _thingService.GetCurrentActivity();
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post()
        {
            byte[] m_Bytes = BinaryData.ReadToEnd(Request.Body);
            var invalue = System.Text.Encoding.UTF8.GetString(m_Bytes);
            Console.WriteLine("Value in : [" + invalue + "]");

            try
            {
                WISE4012ViewModel objWISE = JsonConvert.DeserializeObject<WISE4012ViewModel>(invalue);
                string strMAC12 = StringConverts.MAC17ToMAC12Digit(objWISE.MAC);
                // Get Device/Port/Machine by MAC Address

                var objMN = _context.TbIoTdevice.First(x => x.IoTdeviceMacAddress.Equals(strMAC12));

                var objMachine = _thingService.GetCurrentActivityByMachine(objMN.MachineId);

                if (objMachine == null) return; // Machine Not Found -- not keep log data to system

                var objMongo = new IotLoggerMongo();
                var objMCLog = new MachineRunningStatus();
                var objMCStat = new MachineStatusLog();

                //if (objMachine != null)
                //{
                //    //objMachine.LastUpdateDateTime = DateTime.Now;

                //    objMCStat.MachineID = objMachine.Id;
                //    objMCStat.CDateTime = objWISE.TIM;
                //    objMCStat.SDateTime = DateTime.Now;
                //    objMCStat.MachineRunningStatus = "0"; //Set values to Default 0 Machine Turn off
                //}

                bool changeStatus = false;

                foreach (PortStatus pd in objMachine.ThingPortsStatus)
                {
                    // Check match port and value
                    for (int i = 0; i <= objWISE.Record.GetUpperBound(0); i++)
                    {

                        if (objWISE.Record[i, 0] == 0 && objWISE.Record[i, 1] == pd.IoTdevicePort &&
                            new[] { "1", "2", "3", "7", "10", "12", "19", "20" }.Contains(objWISE.Record[i, 2].ToString()))
                        {

                            var objMGLogger = new MGLogger();
                            objMGLogger.MachineID = objMachine.Id;
                            objMGLogger.CDateTime = objWISE.TIM;
                            objMGLogger.Port = pd.IoTdevicePort.ToString();
                            objMGLogger.SensorValue = objWISE.Record[i, 3].ToString();
                            objMGLogger.MAC = strMAC12;
                            objMGLogger.UID = objWISE.UID;
                            objMGLogger.SDateTime = DateTime.Now; //objMCStat.SDateTime;
                                                                  //var atvlog = new TbIoTactivityLog();
                                                                  //atvlog.MachineId = objMachine.Id;
                                                                  //atvlog.ClientDateTime = objWISE.TIM;
                                                                  //atvlog.SensorId = pd.IoTdevicePort;
                                                                  //atvlog.SensorValue = objWISE.Record[i, 3];
                                                                  //atvlog.ServerDateTime = DateTime.Now;

                            //_context.TbIoTactivityLog.Add(atvlog);
                            //if (objMGLogger.SensorValue == "1")
                            //{
                            // var statusVal = i + 1;
                            // objMCStat.MachineRunningStatus = statusVal.ToString(); // Set Port Number to Machine Status
                            // }

                            pd.LastUpdateDateTime = objWISE.TIM;
            
                            // i < 3 mean check change status only port 0 to 2
                            if (i < 3 && (pd.Value != objWISE.Record[i, 3]))
                            {
                                pd.Value = objWISE.Record[i, 3];

                                changeStatus = true;
                            }

                            objMongo.Create(objMGLogger);



                            // ------ Comment for run production 
                            //if (objMachine != null)
                            //{
                            //    this.UpdateMonitorSensor(objMachine.Id.Trim() + pd.IoTdevicePortID.Trim(), pd.Value.ToString());
                            //}



                            //Thread.Sleep(5000);
                            /*
                            var tmpLog = new DeviceActivityLogModel();
                            tmpLog.MachineID = pd.MachineId;
                            tmpLog.ClientDateTime = objWISE.TIM;
                            tmpLog.DevicePort = pd.IoTdevicePort;
                            tmpLog.SensorValue = objWISE.Record[i, 3];
                            _thingService.SetCurrentActivity(tmpLog);
                            */

                        }
                    }
                }

                if (changeStatus && objMachine != null)
                {

                    if (objMachine.ThingPortsStatus[0].Value == 1)
                    {
                        objMachine.OnlineStatus = "1"; // Green
                    }
                    else
                        if (objMachine.ThingPortsStatus[1].Value == 1)
                    {
                        objMachine.OnlineStatus = "2"; // Red
                    }
                    else
                        if (objMachine.ThingPortsStatus[2].Value == 1)
                    {
                        objMachine.OnlineStatus = "3"; // Yellow
                    }
                    else
                    {
                        objMachine.OnlineStatus = "0"; // Black
                    }
                    objMachine.LastUpdateDateTime = DateTime.Now;

                    objMCStat.MachineID = objMachine.Id;
                    objMCStat.MachineName = objMachine.Name;
                    objMCStat.CDateTime = objWISE.TIM;
                    objMCStat.SDateTime = DateTime.Now;
                    objMCStat.MachineRunningStatus = objMachine.OnlineStatus;

                    objMCLog.Create(objMCStat);

                    this.UpdateMachineChangeStatus(objMachine);

                    //objMachine.OnlineStatus = objMCStat.MachineRunningStatus;
                }

                //Console.WriteLine(strMAC12);
            }
            catch (Exception ex)
            {
                var errdata = new TbUdlog();
                errdata.Indata = "Error Value : " + ex.Message + " : " + ex.StackTrace + " || " + invalue;
                errdata.ServerDateTime = DateTime.Now;
                errdata.Id = Guid.NewGuid().ToString();
                _context.TbUdlog.Add(errdata);
                _context.SaveChanges();
            }

            //Console.WriteLine(" ------------- Application Content-type : [" + Request.Headers["Content-Type"] + "]");
            //_context.SaveChanges();
            //Console.WriteLine(value.MAC + " : " + value.PE);
        }
        /*
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value) //DeviceActivityLogModel value)
        {
            Console.WriteLine(value.ToString());
            if (value != null)
            {
                try
                {
                    var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                    var objDevice = _context.TbIoTdevicePort
                                            .Where(d => d.IoTdeviceMacAddress == remoteIpAddress.ToString() )
                                            .FirstOrDefault();

                    if (objDevice == null)
                    {
                        this.ManageError(new Exception("Machine ID not found"), value);
                        //return;
                    }
                    else
                    {
                        var indata = new TbIoTactivityLog();
                        //indata.ClientDateTime = value.ClientDateTime;
                        indata.ClientDateTime = DateTime.Now;
                        indata.ServerDateTime = DateTime.Now;
                        indata.SensorId = 0;// For Test ------------------------------------------- value.DevicePort;
                        indata.SensorValue = 1; // For Test --------------------------------------- value.SensorValue;
                        indata.MachineId = objDevice.MachineId;

                        // Save New Record to DB
                        _context.TbIoTactivityLog.Add(indata);
                    }
                }
                catch (Exception ex)
                {
                    this.ManageError(ex, value);
                    //var errdata = new TbUdlog();
                    //errdata.Indata = value.ToString() + "<<-- Error : " + ex.Message + " Stack Track : " + ex.StackTrace + "-->>";
                    //errdata.ServerDateTime = DateTime.Now;
                    //_context.TbUdlog.Add(errdata);
                }

            }
            else
            {
                this.ManageError(new Exception("JSON Data In BODY is null."), value);

            }
            //_context.SaveChangesAsync();
            _context.SaveChanges();
        }
        */


        // private async Task<string> UpdateMonitorSensor(string sensorID,string sensorValue)
        private void UpdateMonitorSensor(string sensorID, string sensorValue)
        {
            var objSensor = new SensorViewModel();
            objSensor.SensorID = sensorID;
            objSensor.SensorValue = sensorValue;

            try
            {

                var myContent = JsonConvert.SerializeObject(objSensor);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var client = new HttpClient();
                client.PostAsync("http://localhost:5000/api/MonitorSensor", byteContent);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.Source);
            }
        }

        private void UpdateMachineChangeStatus(Thing objThing)
        {

            //var objSensor = new SensorViewModel();
            //objSensor.SensorID = sensorID;
            //objSensor.SensorValue = sensorValue;

            try
            {

                var myContent = JsonConvert.SerializeObject(objThing);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var client = new HttpClient();
                client.PostAsync("http://localhost:5000/api/MachineSensor", byteContent);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.Source);
            }

            //using (var client = new HttpClient())
            //{
            //client.PostAsync("http://localhost:5000/api/greeting", byteContent);
            //var response = await client.GetStringAsync("http://localhost:5000/api/greeting");
            // The response object is a string that looks like this:
            // "{ message: 'Hello world!' }"
            //return await client.GetStringAsync("http://localhost:5000/api/greeting");

            // }
        }

        private void ManageError(Exception inExc, DeviceActivityLogModel errValue)
        {
            var errdata = new TbUdlog();
            errdata.Indata = errValue.ToString() + "<<-- Error : " + inExc.Message + " Stack Track : " + inExc.StackTrace + "-->>";
            errdata.ServerDateTime = DateTime.Now;
            errdata.Id = Guid.NewGuid().ToString();
            _context.TbUdlog.Add(errdata);
        }

        private void ManageError(Exception inExc, string errValue)
        {
            var errdata = new TbUdlog();
            errdata.Indata = errValue.ToString() + "<<-- Error : " + inExc.Message + " Stack Track : " + inExc.StackTrace + "-->>";
            errdata.ServerDateTime = DateTime.Now;
            errdata.Id = Guid.NewGuid().ToString();
            _context.TbUdlog.Add(errdata);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
