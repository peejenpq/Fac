using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TIROTAPI.Models;
using TIROTAPI.Services;
using TIROTLibrary.Business;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TIROTAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MasterController : Controller
    {
        private tirotdbContext _context;
        private IThingService _thingService;

        public MasterController(tirotdbContext context, IThingService thingService)
        {
            _context = context;
            _thingService = thingService;
            // GET: api/values
        }

        [HttpGet]
        public IEnumerable<Machine> GetListAllMachine()
        {
            List<Machine> retMachine = new List<Machine>();

            var allMachine = _context.TbMachine.ToList();
            foreach (TbMachine allmc in allMachine)
            {
                Machine newmc = new Machine();
                newmc.MachineId = allmc.MachineId;
                newmc.MachineName = allmc.MachineName;
                newmc.MachineWorkSchduleType = allmc.MachineWorkSchduleType;
                newmc.MachineWorkStatus = allmc.MachineWorkStatus;
                newmc.WorkSchdule = allmc.WorkSchdule;

                retMachine.Add(newmc);
            }
            return retMachine;
        }

        [HttpGet]
        public Machine GetMachineById(string machineId)
        {

            var objMc = _context.TbMachine.FirstOrDefault(p => p.MachineId == machineId);

            Machine newmc = new Machine();
            newmc.MachineId = objMc.MachineId;
            newmc.MachineName = objMc.MachineName;
            newmc.MachineWorkSchduleType = objMc.MachineWorkSchduleType;
            newmc.MachineWorkStatus = objMc.MachineWorkStatus;
            newmc.WorkSchdule = objMc.WorkSchdule;

            return newmc;
        }
    }
}
