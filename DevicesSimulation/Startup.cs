using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevicesSimulation.Models;
using DevicesSimulation.TasksRunAsync;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevicesSimulation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            string connectionString = "Host=localhost;Port=5432;Database=DevSim;Username=postgres;Password=123";
            services.AddDbContext<DevicesContext>(options => options.UseNpgsql(connectionString));
            services.AddSingleton<ITaskInvoke, TemperatureChange>();
            services.AddSingleton<ITaskInvoke, TemperatureOutsideUpdate>();
            services.AddSingleton<ITaskInvoke, MoveChange>();
            services.AddSingleton<IHostedService, TaskRunner>();
            

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
