using Core.AiPlanning;
using Core.Model;
using Core.SensorContextStore;
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

            services.AddSingleton<ISensorContextStore>(e =>
            {
                var connection = e.GetService<IConnectionMultiplexer>();
                return new RedisSensorContextSore(connection);
            });

            // TODO Add ActuatorContextConsumer service
            // services.AddSingleton<IActuatorContextConsumer>(e => new ActuatorContextConsumer());

            services.AddSingleton<AiPlanner>(e => new AiPlanner(e.GetService<IActuatorContextConsumer>(),
                e.GetService<ISensorContextStore>()));

            services.AddSingleton<ISensorContextConsumer>(e => new SensorContextConsumer(e.GetService<AiPlanner>()));
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