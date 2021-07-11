using System;
using Core;
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
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Serilog;
using StackExchange.Redis;

namespace API
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

            services.AddSingleton<IConnectionMultiplexer>(e => ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = {Configuration["RedisEndpoint"]}
            }));

            services.AddSingleton<IApplicationStateStore>(e =>
            {
                var conMux = e.GetService<IConnectionMultiplexer>();
                return new RedisApplicationStateSore(conMux);
            });

            services.AddSingleton<IManagedMqttClient>(e =>
            {
                Log.Information("Initializing MQTT");
                var client = new MqttFactory().CreateManagedMqttClient();

                var options = new MqttClientOptionsBuilder()
                    .WithClientId(Guid.NewGuid().ToString())
                    .WithTcpServer(Configuration["MqttHost"], int.Parse(Configuration["MqttPort"]))
                    .WithCleanSession()
                    .Build();

                var managedOptions = new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(options)
                    .Build();

                client.UseConnectedHandler(_ => { Log.Information("Successfully connected with MQTT Broker."); });
                client.UseDisconnectedHandler(_ => { Log.Information("Disconnected from MQTT Broker."); });

                client.StartAsync(managedOptions).Wait();

                return client;
            });

            services.AddHostedService<MqttEndpoint>();

            services.AddSingleton<IActuatorContextConsumer>(e =>
            {
                var mqttClient = e.GetService<IManagedMqttClient>();
                return new ActuatorContextConsumer(mqttClient);
            });

            services.AddSingleton<IExternalPddlSolver>(e => new OnlinePddlSolver());

            services.AddSingleton(e =>
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