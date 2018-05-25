using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class HeaterController : Controller
    {

        DevicesContext db;
        public HeaterController(DevicesContext context)
        {
            this.db = context;
            if (!db.Heaters.Any())
            {
                db.Heaters.Add(new Heater
                {
                    Name = "Обогреватель Ламповый",
                    Company = "PodogrevPlus",
                    Power = false,
                    IsConect = false,
                    Temperature = 0
                });
                db.Heaters.Add(new Heater
                {
                    Name = "Обогреватель 11Mega",
                    Company = "iChill",
                    Power = true,
                    IsConect = false,
                    Temperature = 20
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Heater> Get()
        {
            return db.Heaters.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device heater = db.Heaters.FirstOrDefault(x => x.Name == name);

            if (heater == null)
                return "device is not found";

            if (heater.IsConect)
                return "device is already connected";
            else
            {
                heater.IsConect = true;
                db.Update(heater);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device heater = db.Heaters.FirstOrDefault(x => x.Name == name);

            if (heater == null)
                return "device is not found";

            if (!heater.IsConect)
                return "device is already disconnected";
            else
            {
                heater.IsConect = false;
                db.Update(heater);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]Heater heater)
        {
            if (heater == null)
                return BadRequest();

            if (!db.Heaters.Any(x => x.Name == heater.Name))
                return NotFound();

            var resultHeater = db.Heaters.FirstOrDefault(x => x.Name == heater.Name);
            resultHeater.Power = heater.Power;
            resultHeater.Temperature = heater.Temperature;

            db.Update(resultHeater);
            db.SaveChanges();
            return Ok(resultHeater);
        }
    }
}
