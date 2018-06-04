using DevicesSimulation.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public class TemperatureChange : ITaskInvoke
    {
        public IServiceProvider _provider;

        public double PeriodMS => 1000 * 10; //10 сек

        public double LastTemperatureInside { get; set; }
        public double LastTemperatureOutside { get; set; }

        public TemperatureChange(IServiceProvider provider)
        {
            _provider = provider;

            LastTemperatureOutside = WeatherOutside.TemperatureOutside;
            LastTemperatureInside = 20;
        }

        public Task Run()
        {
            double NowTemperatureOutside = WeatherOutside.TemperatureOutside;

            double NowTemperatureInside = 0;
            //Данные с устройств
            using (var scope = _provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DevicesContext>();

                var AirConditionersIsOn = context.AirConditioners.Where(z => z.IsConect == true).ToList();
                var HeatersIsOn = context.Heaters.Where(z => z.IsConect == true).ToList();
                var TemperatureSensorsIsOn = context.TemperatureSensors.Where(z => z.IsConect == true).ToList();
                
                //если есть устройства которые отображают температуру
                if(AirConditionersIsOn != null || TemperatureSensorsIsOn != null)
                {
                    //если есть устройства которые меняют температуру
                    if (AirConditionersIsOn != null || HeatersIsOn != null)
                    {
                        int count = 0;
                        foreach (var item in AirConditionersIsOn)
                        {
                            NowTemperatureInside += item.Temperature;
                            LastTemperatureInside = item.TemperatureSensorReadings;
                            count++;
                        }
                        foreach (var item in HeatersIsOn)
                        {
                            NowTemperatureInside += item.Temperature;
                            count++;
                        }
                        foreach (var item in TemperatureSensorsIsOn)
                        {
                            LastTemperatureInside = item.TemperatureSensorReadings;
                        }
                        NowTemperatureInside /= count;

                        //ФОРМУЛА
                        double deltaTemperatureOutside = Math.Abs(LastTemperatureOutside) - Math.Abs(NowTemperatureOutside);
                        double deltaTemperatureInside = Math.Abs(NowTemperatureInside) - Math.Abs(LastTemperatureInside);
                        var rnd = new Random();
                        double NewTemperatureInside = Math.Round(
                            LastTemperatureInside + 
                            (deltaTemperatureOutside + deltaTemperatureInside + rnd.NextDouble() * rnd.Next(-1,1) * 2)/5, 
                            2, MidpointRounding.AwayFromZero);

                        if (NewTemperatureInside > 100 || NewTemperatureInside < -100)
                            NewTemperatureInside = NowTemperatureInside;

                        //Меняем покаания датчиков
                        foreach (var item in AirConditionersIsOn)
                        {
                            item.TemperatureSensorReadings = NewTemperatureInside;
                            context.Update(item);
                        }
                        foreach (var item in TemperatureSensorsIsOn)
                        {
                            item.TemperatureSensorReadings = NewTemperatureInside;
                            context.Update(item);
                        }
                        context.SaveChanges();
                    }
                }
            }

            //Обновляем последнюю температуру
            LastTemperatureOutside = NowTemperatureOutside;

            return null;
        }

        public Task Invoke()
        {
            return Run();
        }
    }
}
