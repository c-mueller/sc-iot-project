using Core.AiPlanning;
using Core.AiPlanning.ExternalPddlSolver;
using Core.Store;
using MessagingEndpoint;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Model.Interfaces;
using StackExchange.Redis;

namespace Core
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
            services.AddControllers();

            services.AddSingleton<IApplicationStateStore>(e =>
            {
                var conMux = e.GetService<IConnectionMultiplexer>();
                return new RedisApplicationStateSore(conMux);
            });
            
            services.AddSingleton<IActuatorContextConsumer>(e =>
            {
                return new OutgoingMessages();
            });

            services.AddSingleton<IExternalPddlSolver>(e =>
            {
                return new OnlinePddlSolver();
            });
            
            services.AddSingleton<AiPlanner>(e =>
            {
                var actuatorContextConsumer = e.GetService<IActuatorContextConsumer>();
                var applicationStateStore = e.GetService<IApplicationStateStore>();
                var pddlSolver = e.GetService<IExternalPddlSolver>();
                return new AiPlanner(actuatorContextConsumer, applicationStateStore, pddlSolver);
            });

            services.AddSingleton<ISensorContextConsumer>(e =>
            {
                var aiPlanner = e.GetService<AiPlanner>();
                var applicationStateStore = e.GetService<IApplicationStateStore>();
                return new SensorContextConsumer(aiPlanner, applicationStateStore);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}