using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public static class WeatherOutside
    {
        public static double TemperatureOutside { get; set; }
        private const double AbsoluteZeroKelvin = 273.15;

        public static void updateTemperatureOutside()
        {
            //Запрос погоды
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                "http://api.openweathermap.org/data/2.5/weather?id=1508291&APPID=ce2124ddd35bd7afd4eff844857c517d");
            request.Method = "GET";
            request.ContentType = "application/json";

            WebResponse response = request.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string json = streamReader.ReadToEnd();
                var weather = Weather.FromJson(json);

                TemperatureOutside = weather.Main.Temp - AbsoluteZeroKelvin;
            }
        }
    }
}
