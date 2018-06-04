using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public class TemperatureOutsideUpdate : ITaskInvoke
    {
        public double PeriodMS => 1000 * 60 * 30; //30 мин

        public Task Invoke()
        {
            WeatherOutside.updateTemperatureOutside();
            return null;
        }
    }
}
