using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevicesSimulation.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MoveSensorController : Controller
    {

        DevicesContext db;
        public MoveSensorController(DevicesContext context)
        {
            this.db = context;
            if (!db.MoveSensors.Any())
            {
                db.MoveSensors.Add(new MoveSensor
                {
                    Name = "SCP-173",
                    Company = "Secure Contain Protect",
                    Power = false,
                    IsConect = false,
                    IsMove = false,
                    IsSecureMod = false
                });
                db.MoveSensors.Add(new MoveSensor
                {
                    Name = "Move-500",
                    Company = "Shield",
                    Power = true,
                    IsConect = false,
                    IsMove = false,
                    IsSecureMod = false
                });
                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<MoveSensor> Get()
        {
            return db.MoveSensors.Where(x => x.IsConect == true).ToList();
        }

        public string ConnectDevice(string name)
        {
            Device moveSensor = db.MoveSensors.FirstOrDefault(x => x.Name == name);

            if (moveSensor == null)
                return "device is not found";

            if (moveSensor.IsConect)
                return "device is already connected";
            else
            {
                moveSensor.IsConect = true;
                db.Update(moveSensor);
                db.SaveChanges();
                return "device is connected";
            }
        }

        public string DisconnectDevice(string name)
        {
            Device moveSensor = db.MoveSensors.FirstOrDefault(x => x.Name == name);

            if (moveSensor == null)
                return "device is not found";

            if (!moveSensor.IsConect)
                return "device is already disconnected";
            else
            {
                moveSensor.IsConect = false;
                db.Update(moveSensor);
                db.SaveChanges();
                return "device is disconnected";
            }
        }

        [HttpPut]
        public IActionResult Command([FromBody]MoveSensor moveSensor)
        {
            if (moveSensor == null)
                return BadRequest();

            if (!db.MoveSensors.Any(x => x.Name == moveSensor.Name))
                return NotFound();

            var resultMoveSensor = db.MoveSensors.FirstOrDefault(x => x.Name == moveSensor.Name);
            resultMoveSensor.Power = moveSensor.Power;
            resultMoveSensor.IsMove = moveSensor.IsMove;
            resultMoveSensor.IsSecureMod = moveSensor.IsSecureMod;

            db.Update(resultMoveSensor);
            db.SaveChanges();
            return Ok(resultMoveSensor);
        }
    }
}
