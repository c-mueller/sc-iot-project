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
        public Dictionary<string, string> clients { get; set; }
        private readonly ISensorContextConsumer incomingMessages;
        private readonly IManagedMqttClient _mqttClient;

        public MQTTEndpoint(ISensorContextConsumer incomingMessages, IManagedMqttClient mqttClient)
        {
            //outgoingMessages = new OutgoingMessages(this,mqttClient);
            //incomingMessages = new IncomingMessages(this,mqttClient);
            string clientId = "DON"; //Guid.NewGuid().ToString();
            string mqttURI = "localhost";
            int mqttPort = 1883;
            bool mqttSecure = false;
            clients = new Dictionary<string, string>();
            this.incomingMessages = incomingMessages;
            _mqttClient = mqttClient;

            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                //.WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttURI, mqttPort)
                .WithCleanSession();
            var options = mqttSecure
                ? messageBuilder
                    .WithTls()
                    .Build()
                : messageBuilder
                    .Build();


            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(options)
                .Build();
            setUpConnectionHandlers(_mqttClient);

            _mqttClient.UseApplicationMessageReceivedHandler(e => //handler message received
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;

                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($"Topic: {topic}. Message Received: {payload}"); //pass it to json parser
                        if (!clients.TryGetValue(topic, out string client))
                        {
                            string[] subs = topic.Split('/');
                            clients.Add(subs.Last(), topic);
                            Console.WriteLine("neuer Client " + subs.Last() + " unter topic: " + topic +
                                              " hinzugefügt");
                        }
                        else
                        {
                            Console.WriteLine("topic und client existieren bereits");
                        }

                        var sensorcontext = JsonConvert.DeserializeObject<SensorContext>(payload);
                        incomingMessages.Consume(sensorcontext);
                        //incoming message --> parse json string to Sensorcontext --> then call IncomingMessages
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });


            Task.Run(async () => await _mqttClient.StartAsync(managedOptions)).Wait();

            // Connecting
            Task.Run(() => this.SubscribeAsync(_mqttClient, "#")).Wait();
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


        private void setUpConnectionHandlers(IManagedMqttClient mqttClient)
        {
            mqttClient.UseConnectedHandler(e => { Console.WriteLine("Connected successfully with MQTT Brokers."); });
            mqttClient.UseDisconnectedHandler(e => { Console.WriteLine("Disconnected from MQTT Brokers."); });
        }

        public async Task SubscribeAsync(IManagedMqttClient mqttClient, string topic, int qos = 1) =>
            await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel) qos)
                .Build());

        public async Task PublishAsync(IManagedMqttClient mqttClient, string topic, string payload,
            bool retainFlag = true, int qos = 1) =>
            await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel((MqttQualityOfServiceLevel) qos)
                .WithRetainFlag(retainFlag)
                .Build());
    }
}