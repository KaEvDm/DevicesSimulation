using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public class TaskRunner : HostedService
    {
        IEnumerable<ITaskInvoke> _tasks;
        List<TaskWrapper> _listTasks = new List<TaskWrapper>();

        public TaskRunner(IEnumerable<ITaskInvoke> tasks)
        {
            _tasks = tasks;
            foreach (var item in _tasks)
            {
                _listTasks.Add(new TaskWrapper
                {
                    Task = item,
                    PeriodMS = item.PeriodMS,
                    NextRunTime = DateTime.Now
                });
            }
        }

        public async Task AllTaskRun()
        {
            var currentTime = DateTime.Now;
            var taskFactory = new TaskFactory();
            var taskShouldRun = _listTasks.Where(z => z.IsRun(currentTime)).ToList();

            foreach (var item in taskShouldRun)
            {
                item.Increment();
                await taskFactory.StartNew(
                        async () =>
                        {
                            await item.Task.Invoke();
                        });
            }
        }

        protected override async Task ExecuteAsync(CancellationToken ctoken)
        {
            while (!ctoken.IsCancellationRequested)
            {
                await AllTaskRun();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}
