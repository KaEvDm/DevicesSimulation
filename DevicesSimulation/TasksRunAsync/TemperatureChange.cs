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

        public double PeriodMS => 1000 * 60 * 30; //30 мин

        public double LastTemperatureInside { get; set; }
        public double LastTemperatureOutside { get; set; }

        private const double AbsoluteZeroKelvin = 273.15;

        public TemperatureChange(IServiceProvider provider)
        {
            _provider = provider;

            LastTemperatureOutside = GetTemperatureOutside();
            LastTemperatureInside = 20;
        }

        public double GetTemperatureOutside()
        {
            //Запрос погоды
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "http://api.openweathermap.org/data/2.5/weather?id=1508291&APPID=ce2124ddd35bd7afd4eff844857c517d");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    var weather = Weather.FromJson(reader.ReadToEnd());

                    return weather.Main.Temp - AbsoluteZeroKelvin;
                }
            }
        }

        public Task Run()
        {
            double NowTemperatureInside = 0;
            double NowTemperatureOutside = 0;

            NowTemperatureOutside = GetTemperatureOutside();

            //Данные с устройств
            using (var scope = _provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DevicesContext>();

                var AirConditionersIsOn = context.AirConditioners.Where(z => z.IsConect == true).ToList();
                var HeatersIsOn = context.Heaters.Where(z => z.IsConect == true).ToList();
                var TemperatureSensorsIsOn = context.TemperatureSensors.Where(z => z.IsConect == true).ToList();

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
                double deltaTemperatureInside = Math.Abs(NowTemperatureInside) - Math.Abs(NowTemperatureInside);
                double NewTemperatureInside = LastTemperatureInside + deltaTemperatureOutside / 2 + deltaTemperatureInside;

                //Обновляем последнюю температуру
                LastTemperatureOutside = NowTemperatureOutside;

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

            return null;
        }

        public Task Invoke()
        {
            return Run();
        }
    }
}
