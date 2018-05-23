﻿using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DevicesSimulation.TasksRunAsync
{
    public abstract class HostedService : IHostedService
    {
        private readonly CancellationTokenSource _stoppingCts = new CancellationTokenSource();
        private Task _executingTask;

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            // Store the task we're executing
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            // If the task is completed then return it,
            // this will bubble cancellation and failure to the caller
            if (_executingTask.IsCompleted)
                return _executingTask;

            // Otherwise it's running
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
                return;

            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public virtual void Dispose()
        {
            _stoppingCts.Cancel();
        }
    }

}