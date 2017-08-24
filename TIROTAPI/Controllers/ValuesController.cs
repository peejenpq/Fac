using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TIROTAPI.Models;
using TIROTLibrary.SysCommon.Utility;
using Newtonsoft.Json;
using TIROTAPI.Models.ActivityLogViewModels;

namespace TIROTAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {

        private tirotdbContext _context;

        public ValuesController(tirotdbContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        //public void Post([FromBody]string value)
        public void Post()
        {
           
            byte[] m_Bytes = BinaryData.ReadToEnd(Request.Body);
            var invalue = System.Text.Encoding.UTF8.GetString(m_Bytes);
            Console.WriteLine("Value in : ["+ invalue + "]");

            
            
            try
            {
                WISE4012ViewModel objWISE = JsonConvert.DeserializeObject<WISE4012ViewModel>(invalue);
            }
            catch (Exception ex)
            {
                var errdata = new TbUdlog();
                errdata.Indata = "Error Value : " + ex.Message + " : " + ex.StackTrace + " || " + invalue;
                errdata.ServerDateTime = DateTime.Now;
                errdata.Id = Guid.NewGuid().ToString();
                _context.TbUdlog.Add(errdata);
            }
            _context.SaveChanges();
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
