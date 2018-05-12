using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AirConditionerController : Controller
    {

        DevicesContext db;
        public AirConditionerController(DevicesContext context)
        {
            this.db = context;
            if (!db.AirConditioners.Any())
            {
                db.AirConditioners.Add(new AirConditioner
                {
                    Name = "Кондиционер А-505",
                    Company = "Bosh",
                    Power = false,
                    IsConect = false,
                    Mode = 0,
                    Temperature = 0,
                    TemperatureSensorReadings = 0,
                    IsWater = false
                });
                db.AirConditioners.Add(new AirConditioner
                {
                    Name = "Ионизатор воздуха напольный R-351",
                    Company = "Polaris",
                    Power = true,
                    IsConect = false,
                    Mode = 5,
                    Temperature = 20,
                    TemperatureSensorReadings = 15,
                    IsWater = true
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<AirConditioner> Get()
        {
            return db.AirConditioners.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device airConditioner = db.AirConditioners.FirstOrDefault(x => x.Name == name);

            if (airConditioner == null)
                return "device is not found";

            if (airConditioner.IsConect)
                return "device is already connected";
            else
            {
                airConditioner.IsConect = true;
                db.Update(airConditioner);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device airConditioner = db.AirConditioners.FirstOrDefault(x => x.Name == name);

            if (airConditioner == null)
                return "device is not found";

            if (!airConditioner.IsConect)
                return "device is already disconnected";
            else
            {
                airConditioner.IsConect = false;
                db.Update(airConditioner);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]AirConditioner airConditioner)
        {
            if (airConditioner == null)
                return BadRequest();

            if (!db.AirConditioners.Any(x => x.Name == airConditioner.Name))
                return NotFound();

            db.Update(airConditioner);
            db.SaveChanges();
            return Ok(airConditioner);
        }
    }
}
