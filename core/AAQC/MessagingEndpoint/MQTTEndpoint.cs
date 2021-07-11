using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Interfaces;
using Model.Model;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace MessagingEndpoint
{
    public class MQTTEndpoint
    {
        public Dictionary<string, string> Clients { get; set; }
        private readonly ISensorContextConsumer _incomingMessages;
        private readonly IManagedMqttClient _mqttClient;

        private const string ClientId = "DON";
        private const string MqttUri = "localhost";
        private const int MqttPort = 1883;

        public MQTTEndpoint(ISensorContextConsumer incomingMessages, IManagedMqttClient mqttClient)
        {
            _incomingMessages = incomingMessages;
            _mqttClient = mqttClient;

            Clients = new Dictionary<string, string>();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(ClientId)
                .WithTcpServer(MqttUri, MqttPort)
                .WithCleanSession()
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(options)
                .Build();
            SetUpConnectionHandlers(_mqttClient);

            _mqttClient.UseApplicationMessageReceivedHandler(e => //handler message received
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;

                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($"Topic: {topic}. Message Received: {payload}"); //pass it to json parser
                        if (!Clients.TryGetValue(topic, out string client))
                        {
                            string[] subs = topic.Split('/');
                            Clients.Add(subs.Last(), topic);
                            Console.WriteLine("neuer Client " + subs.Last() + " unter topic: " + topic +
                                              " hinzugefügt");
                        }
                        else
                        {
                            Console.WriteLine("topic und client existieren bereits");
                        }

                        var sensorcontext = JsonConvert.DeserializeObject<SensorContext>(payload);
                        _incomingMessages.Consume(sensorcontext);
                        //incoming message --> parse json string to Sensorcontext --> then call IncomingMessages
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });


            _mqttClient.StartAsync(managedOptions).Wait();

            // Connecting
            SubscribeAsync(_mqttClient, "#").Wait();
            Console.WriteLine("Topic subscribt");
            //Task.Run(() => this.PublishAsync(mqttClient,"room001/output/heater","hallo12")).Wait();
            //Console.WriteLine("Topic gepublisht");
        }


        /*static void Main(string[] args)
        {
            // Display the number of command line arguments.
            Console.WriteLine("Los gehts");
            new MQTTClient();
            while(true) {

            }
        }*/


        private static void SetUpConnectionHandlers(IManagedMqttClient mqttClient)
        {
            mqttClient.UseConnectedHandler(e => { Console.WriteLine("Connected successfully with MQTT Brokers."); });
            mqttClient.UseDisconnectedHandler(e => { Console.WriteLine("Disconnected from MQTT Brokers."); });
        }

        private static async Task SubscribeAsync(IManagedMqttClient mqttClient, string topic, int qos = 1) =>
            await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel) qos)
                .Build());

        public static async Task PublishAsync(IManagedMqttClient mqttClient, string topic, string payload,
            bool retainFlag = true, int qos = 1) =>
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel) qos)
                .WithRetainFlag(retainFlag)
                .Build());
    }
}