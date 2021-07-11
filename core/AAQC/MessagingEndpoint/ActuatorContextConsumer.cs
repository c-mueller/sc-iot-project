using System.Threading.Tasks;
using Model.Interfaces;
using Model.Model;
using MQTTnet;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace MessagingEndpoint
{
    public class ActuatorContextConsumer : IActuatorContextConsumer      
    {
        private readonly IManagedMqttClient _mqttClient;

        public ActuatorContextConsumer(IManagedMqttClient mqttClient) {
            _mqttClient = mqttClient;
        }
        
        public void Consume(ActuatorContext actuatorContext) {
            var topic = $"room001/output/{actuatorContext.Name}";
            var payload = JsonConvert.SerializeObject(actuatorContext.ActuatorInfo);
            PublishAsync(_mqttClient, topic, payload).Wait();
        }
        
        private static async Task PublishAsync(IManagedMqttClient mqttClient, string topic, string payload,
            bool retainFlag = true, int qos = 1) =>
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel) qos)
                .WithRetainFlag(retainFlag)
                .Build());
    }
}