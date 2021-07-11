using System;
using System.Threading.Tasks;
using Model.Interfaces;
using Model.Model;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;

namespace MessagingEndpoint
{
    public class ActuatorContextConsumer : IActuatorContextConsumer      
    {
        private readonly MQTTEndpoint _mqttEndpoint;
        private readonly IManagedMqttClient _mqttClient;

        public ActuatorContextConsumer(MQTTEndpoint mqttEndpoint, IManagedMqttClient mqttClient) {
            _mqttEndpoint = mqttEndpoint;
            _mqttClient = mqttClient;
        }
        public void Consume(ActuatorContext actuatorContext) {
            if(_mqttEndpoint.Clients.ContainsKey(actuatorContext.Name))
                Console.WriteLine("topic found");
            
            var serializedActuatorInfo = JsonConvert.SerializeObject(actuatorContext.ActuatorInfo);
            var topic = _mqttEndpoint.Clients[actuatorContext.Name];
            
            MQTTEndpoint.PublishAsync(_mqttClient, topic, serializedActuatorInfo).Wait();
            //Console.WriteLine("Topic gepublisht");
        }
    }
}