using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PowerSocketController : Controller
    {

        DevicesContext db;
        public PowerSocketController(DevicesContext context)
        {
            this.db = context;
            if (!db.PowerSockets.Any())
            {
                db.PowerSockets.Add(new PowerSocket
                {
                    Name = "Розетка 1",
                    Company = "Electronic",
                    Power = false,
                    IsConect = false,
                    IsOn = true
                });
                db.PowerSockets.Add(new PowerSocket
                {
                    Name = "Розетка 2",
                    Company = "Thunderstorm",
                    Power = true,
                    IsConect = false,
                    IsOn = false
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<PowerSocket> Get()
        {
            return db.PowerSockets.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device powerSocket = db.PowerSockets.FirstOrDefault(x => x.Name == name);

            if (powerSocket == null)
                return "device is not found";

            if (powerSocket.IsConect)
                return "device is already connected";
            else
            {
                powerSocket.IsConect = true;
                db.Update(powerSocket);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device powerSocket = db.PowerSockets.FirstOrDefault(x => x.Name == name);

            if (powerSocket == null)
                return "device is not found";

            if (!powerSocket.IsConect)
                return "device is already disconnected";
            else
            {
                powerSocket.IsConect = false;
                db.Update(powerSocket);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]PowerSocket powerSocket)
        {
            if (powerSocket == null)
                return BadRequest();

            if (!db.PowerSockets.Any(x => x.Name == powerSocket.Name))
                return NotFound();

            var resultPowerSocket = db.PowerSockets.FirstOrDefault(x => x.Name == powerSocket.Name);
            resultPowerSocket.Power = powerSocket.Power;
            resultPowerSocket.IsOn = powerSocket.IsOn;

            db.Update(resultPowerSocket);
            db.SaveChanges();
            return Ok(resultPowerSocket);
        }
    }
}
