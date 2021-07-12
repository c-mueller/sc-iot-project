using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Model.Interfaces;
using Model.Model;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using Serilog;

namespace MessagingEndpoint
{
    public class MqttEndpoint : IHostedService
    {
        private static readonly string[] TopicList =
        {
            "room001/input/temperature",
            "room001/input/particulate-matter",
            "room001/input/co2",
            "outdoors/temperature",
            "outdoors/particulate-matter",
            "outdoors/humidity"
        };

        private readonly ISensorContextConsumer _incomingMessages;
        private readonly IManagedMqttClient _mqttClient;
        private readonly IApplicationStateStore _stateStore;

        private readonly SensorContext _currentSensorContext = new SensorContext();

        private Timer _plannerUpdateTimer;

        public MqttEndpoint(ISensorContextConsumer incomingMessages, IManagedMqttClient mqttClient,
            IApplicationStateStore stateStore)
        {
            _incomingMessages = incomingMessages;
            _mqttClient = mqttClient;
            _stateStore = stateStore;
        }

        private void Init()
        {
            _mqttClient.UseApplicationMessageReceivedHandler(HandleMqttMessage);

            foreach (var topic in TopicList)
            {
                SubscribeAsync(_mqttClient, topic).Wait();
            }
        }

        private void HandleMqttMessage(MqttApplicationMessageReceivedEventArgs messageEvent)
        {
            var stringPayload = Encoding.UTF8.GetString(messageEvent.ApplicationMessage.Payload);
            try
            {
                var parsedPayload = JsonConvert.DeserializeObject<SensorInput>(stringPayload);

                _currentSensorContext.SubmitMeasurement(parsedPayload);
                _stateStore.StoreLatestSensorContext(_currentSensorContext);
                // _incomingMessages.Consume(_currentSensorContext.DeepCopy());
            }
            catch (Exception e)
            {
                Log.Error("Failed to Process message {Message} from {Topic} because: {ErrorMessage}",
                    stringPayload,
                    messageEvent.ApplicationMessage.Topic, e.Message, e);
                Log.Error(e.StackTrace);
            }
        }

        private void PlannerUpdateHandler(object? e)
        {
            Log.Information("Updating Planner Context...");
            try
            {
                _incomingMessages.Consume(_currentSensorContext.DeepCopy());
            }
            catch (Exception ex)
            {
                Log.Error("Failed to update Ai Planner context. Reason: {ErrorMessage}", ex.Message, ex);
                Log.Error(ex.StackTrace);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information("Initializing MQTT Endpoint");
            Init();

            _plannerUpdateTimer =
                new Timer(PlannerUpdateHandler, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _plannerUpdateTimer.DisposeAsync().AsTask();
        }

        private static async Task SubscribeAsync(IManagedMqttClient mqttClient, string topic, int qos = 1)
        {
            Log.Information("Subscribing to Topic '{Topic}'", topic);

            await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel) qos)
                .Build());
        }
    }
}