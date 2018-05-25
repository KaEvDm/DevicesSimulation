using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TemperatureSensorController : Controller
    {

        DevicesContext db;
        public TemperatureSensorController(DevicesContext context)
        {
            this.db = context;
            if (!db.TemperatureSensors.Any())
            {
                db.TemperatureSensors.Add(new TemperatureSensor
                {
                    Name = "SCP-106",
                    Company = "Secure Contain Protect",
                    Power = false,
                    IsConect = false,
                    TemperatureSensorReadings = 0
                });
                db.TemperatureSensors.Add(new TemperatureSensor
                {
                    Name = "Температурный датчик Е241",
                    Company = "Polaris",
                    Power = true,
                    IsConect = false,
                    TemperatureSensorReadings = 15,
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TemperatureSensor> Get()
        {
            return db.TemperatureSensors.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device temperatureSensor = db.TemperatureSensors.FirstOrDefault(x => x.Name == name);

            if (temperatureSensor == null)
                return "device is not found";

            if (temperatureSensor.IsConect)
                return "device is already connected";
            else
            {
                temperatureSensor.IsConect = true;
                db.Update(temperatureSensor);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device temperatureSensor = db.TemperatureSensors.FirstOrDefault(x => x.Name == name);

            if (temperatureSensor == null)
                return "device is not found";

            if (!temperatureSensor.IsConect)
                return "device is already disconnected";
            else
            {
                temperatureSensor.IsConect = false;
                db.Update(temperatureSensor);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]TemperatureSensor temperatureSensor)
        {
            if (temperatureSensor == null)
                return BadRequest();

            if (!db.TemperatureSensors.Any(x => x.Name == temperatureSensor.Name))
                return NotFound();

            var resultTemperatureSensor = db.TemperatureSensors.FirstOrDefault(x => x.Name == temperatureSensor.Name);
            resultTemperatureSensor.Power = temperatureSensor.Power;

            db.Update(resultTemperatureSensor);
            db.SaveChanges();
            return Ok(resultTemperatureSensor);
        }
    }
}
