using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TIROTAPI.Models;
using TIROTLibrary.Business;
using TIROTLibrary.SysCommon.Utility;
using Newtonsoft.Json;
using TIROTAPI.Services;
using TIROTAPI.Models.ActivityLogViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TIROTAPI.Controllers
{
    [Route("api/[controller]")]
    public class WISE_SeriesController : Controller
    {
        private tirotdbContext _context;
        private IThingService _thingService;

        public WISE_SeriesController(tirotdbContext context, IThingService thingService)
        {
            _context = context;
            _thingService = thingService;
        }
        
        // GET api/values
        [HttpGet]
        public IEnumerable<Thing> Get()
        {
            /*
             Thing wmx = new Thing();
             wmx.Id = Guid.NewGuid().ToString();
             wmx.LastUpdateDateTime = DateTime.Now;

            try
            {
                _thingService.SetCurrentActivity(wmx);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            */

            return _thingService.GetCurrentActivity();
        }

        // GET api/values
        //[HttpGet]
        //public Thing Get(string MachineID)
        //{
        //    return _thingService.GetCurrentActivityByMachine(MachineID);
        //}

        // POST api/values
        [HttpPost]
        //public void Post([FromBody]WISE4012ViewModel value)
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

                //var objxx = objMachine.ThingPortsStatus
                //var objDPs = _context.TbIoTdevicePort.Where(x => x.IoTdeviceMacAddress.Equals(strMAC12));
                //Random tmprnd = new Random();
                //foreach (TbIoTdevicePort pd in objDPs)
                foreach (PortStatus pd in objMachine.ThingPortsStatus)
                {
                    // Check match port and value
                    for (int i = 0; i <= objWISE.Record.GetUpperBound(0);i++)
                    {
                        
                        if (objWISE.Record[i,0] == 0 && objWISE.Record[i,1] == pd.IoTdevicePort && 
                            new[] { "1", "2", "3", "7", "10", "12", "19", "20" }.Contains(objWISE.Record[i,2].ToString()))
                        {
                            var atvlog = new TbIoTactivityLog();
                            atvlog.MachineId = objMachine.Id;
                            atvlog.ClientDateTime = objWISE.TIM;
                            atvlog.SensorId = pd.IoTdevicePort;
                            atvlog.SensorValue = objWISE.Record[i, 3];
                            atvlog.ServerDateTime = DateTime.Now;

                            _context.TbIoTactivityLog.Add(atvlog);

                            pd.LastUpdateDateTime = objWISE.TIM;
                            pd.Value = objWISE.Record[i, 3];


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
                
                //Console.WriteLine(strMAC12);
            }
            catch (Exception ex)
            {
                var errdata = new TbUdlog();
                errdata.Indata = "Error Value : " + ex.Message + " : " + ex.StackTrace + " || " + invalue;
                errdata.ServerDateTime = DateTime.Now;
                errdata.Id = Guid.NewGuid().ToString();
                _context.TbUdlog.Add(errdata);
                //_context.SaveChanges();
            }

            Console.WriteLine(" ------------- Application Content-type : [" + Request.Headers["Content-Type"] + "]");
            _context.SaveChanges();
            //Console.WriteLine(value.MAC + " : " + value.PE);
        }
    }
}
