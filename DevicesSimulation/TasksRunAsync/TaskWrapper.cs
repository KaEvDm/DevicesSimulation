using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public class TaskWrapper
    {
        public DateTime LastRunTime { get; set; }
        public DateTime NextRunTime { get; set; }
        public ITaskInvoke Task { get; set; }
        public double PeriodMS { get; set; }

        public void Increment()
        {
            LastRunTime = NextRunTime;
            NextRunTime = LastRunTime.AddMilliseconds(PeriodMS);
        }

        public bool IsRun(DateTime currentTime)
        {
            return NextRunTime <= currentTime;
        }
    }
}
