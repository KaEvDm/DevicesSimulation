using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.Models
{
    public class AirConditioner : Device
    {
        public int Mode { get; set; }
        public int Temperature { get; set; }
        public int TemperatureSensorReadings { get; set; }
        public bool IsWater { get; set; }
    }
}
