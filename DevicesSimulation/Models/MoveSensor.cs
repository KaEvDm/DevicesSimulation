using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.Models
{
    public class MoveSensor : Device
    {
        public bool IsMove { get; set; }
        public bool IsSecureMod { get; set; }
    }
}
