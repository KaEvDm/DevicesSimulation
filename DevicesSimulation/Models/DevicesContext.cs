using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.Models
{
    public class DevicesContext : DbContext
    {
        public DbSet<Lamp> Lamps { get; set; }
        public DbSet<AirConditioner> AirConditioners { get; set; }
        public DbSet<DoorLock> DoorLocks { get; set; }
        public DbSet<Heater> Heaters { get; set; }


        public DevicesContext(DbContextOptions<DevicesContext> options)
            : base(options)
        { }
    }
}
