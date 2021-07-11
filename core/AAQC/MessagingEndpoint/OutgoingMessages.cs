using Microsoft.VisualBasic;
using Model.Interfaces;
using Model.Model;
using MQTTnet.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;
using MQTTnet.Extensions.ManagedClient;
using System.Diagnostics;
using System;
namespace MessagingEndpoint
{
    public class OutgoingMessages : IActuatorContextConsumer      
    {
        MQTTEndpoint mQTTEndpoint;
        IManagedMqttClient mqttClient;

        public OutgoingMessages(MQTTEndpoint mQTTEndpoint, IManagedMqttClient mqttClient) {
            this.mQTTEndpoint = mQTTEndpoint;
            this.mqttClient = mqttClient;
        }
        public void Consume(ActuatorContext actuatorContext) {
            string json = JsonConvert.SerializeObject(actuatorContext, Formatting.Indented);
            string topic;
            if(mQTTEndpoint.clients.ContainsKey(actuatorContext.Name)) {
                Console.WriteLine("topic found");
            }
            topic = mQTTEndpoint.clients[actuatorContext.Name];

            Task.Run(() => mQTTEndpoint.PublishAsync(mqttClient,topic,json)).Wait();
            //Console.WriteLine("Topic gepublisht");
        }
    }
}