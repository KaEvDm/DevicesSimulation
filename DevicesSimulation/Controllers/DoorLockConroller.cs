using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]/")]
    public class DoorLockController : Controller
    {

        DevicesContext db;
        public DoorLockController(DevicesContext context)
        {
            this.db = context;
            if (!db.DoorLocks.Any())
            {
                db.DoorLocks.Add(new DoorLock
                {
                    Name = "Замок ESP8266",
                    Company = "OOO Electronic",
                    Power = false,
                    IsConect = false,
                    IsOpen = false
                });
                db.DoorLocks.Add(new DoorLock
                {
                    Name = "Замок Малютка",
                    Company = "www.DoorLocks.com",
                    Power = false,
                    IsConect = false,
                    IsOpen = false
                });
                db.DoorLocks.Add(new DoorLock
                {
                    Name = "Еврозамок",
                    Company = "www.DoorLocks.com",
                    Power = false,
                    IsConect = true,
                    IsOpen = true
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<DoorLock> Get()
        {
            return db.DoorLocks.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device doorLock = db.DoorLocks.FirstOrDefault(x => x.Name == name);

            if (doorLock == null)
                return "device is not found";

            if (doorLock.IsConect)
                return "device is already connected";
            else
            {
                doorLock.IsConect = true;
                db.Update(doorLock);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device doorLock = db.DoorLocks.FirstOrDefault(x => x.Name == name);

            if (doorLock == null)
                return "device is not found";

            if (!doorLock.IsConect)
                return "device is already disconnected";
            else
            {
                doorLock.IsConect = false;
                db.Update(doorLock);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]DoorLock doorLock)
        {
            if (doorLock == null)
                return BadRequest();

            if (!db.DoorLocks.Any(x => x.Name == doorLock.Name))
                return NotFound();

            var resultDoorLock = db.DoorLocks.FirstOrDefault(x => x.Name == doorLock.Name);
            resultDoorLock.IsOpen = doorLock.IsOpen;
            resultDoorLock.Power = doorLock.Power;

            db.Update(resultDoorLock);
            db.SaveChanges();
            return Ok(resultDoorLock);
        }
    }
}
