using DevicesSimulation.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public class MoveChange : ITaskInvoke
    {
        public IServiceProvider _provider;

        public double PeriodMS => 1000 * 30; //30 сек;

        public MoveChange(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task Run()
        {
            //Данные с устройств
            using (var scope = _provider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DevicesContext>();

                var MoveSensorsIsOn = context.MoveSensors.Where(z => z.IsConect == true).ToList();

                //если есть подключённые датчики
                if(MoveSensorsIsOn != null)
                {
                    foreach (var item in MoveSensorsIsOn)
                    {
                        Random rand = new Random();
                        int randNumber = rand.Next(100);
                        if (randNumber <= 50)
                        {
                            if (item.IsMove) item.IsMove = false;
                            else item.IsMove = true;
                            context.Update(item);
                        }
                    }
                    context.SaveChanges();
                }
            }
            return null;
        }

        public Task Invoke()
        {
            return Run();
        }
    }
}
