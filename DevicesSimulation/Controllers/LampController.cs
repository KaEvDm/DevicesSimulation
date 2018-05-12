using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]/")]
    public class LampController : Controller
    {

        DevicesContext db;
        public LampController(DevicesContext context)
        {
            this.db = context;
            if (!db.Lamps.Any())
            {
                db.Lamps.Add(new Lamp
                {
                    Name = "Lamp 220v",
                    Company = "Light And Magic",
                    Power = false,
                    IsConect = true,
                    Brightness = 0
                });
                db.Lamps.Add(new Lamp
                {
                    Name = "Lamp 220v",
                    Company = "Light And Magic",
                    Power = false,
                    IsConect = false,
                    Brightness = 0
                });
                db.Lamps.Add(new Lamp
                {
                    Name = "Lamp 100w",
                    Company = "Panasonic",
                    Power = true,
                    IsConect = true,
                    Brightness = 255
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Lamp> Get()
        {
            return db.Lamps.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device lamp = db.Lamps.FirstOrDefault(x => x.Name == name);

            if (lamp == null)
                return "device is not found";

            if (lamp.IsConect)
                return "device is already connected";
            else
            {
                lamp.IsConect = true;
                db.Update(lamp);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device lamp = db.Lamps.FirstOrDefault(x => x.Name == name);

            if (lamp == null)
                return "device is not found";

            if (!lamp.IsConect)
                return "device is already disconnected";
            else
            {
                lamp.IsConect = false;
                db.Update(lamp);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]Lamp lamp)
        {
            if(lamp==null)
                return BadRequest();

            if (!db.Lamps.Any(x => x.Name == lamp.Name))
                return NotFound();
 
            db.Update(lamp);
            db.SaveChanges();
            return Ok(lamp);
        }
    }
}
