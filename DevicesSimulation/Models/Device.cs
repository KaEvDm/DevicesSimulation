using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public bool Power { get; set; }
        public bool IsConect { get; set; }
    }
}
