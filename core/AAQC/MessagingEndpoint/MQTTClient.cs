using System;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet;
using System.Threading.Tasks;
using System.Text;

namespace MessagingEndpoint
{
    public class MQTTClient
    {
        public MQTTClient() {
            var mqttClient = new MqttFactory().CreateManagedMqttClient();
            string clientId = Guid.NewGuid().ToString();
            string mqttURI = "localhost";
            int mqttPort = 1883; 
            bool mqttSecure = false;

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
            setUpConnectionHandlers(mqttClient);
            
            mqttClient.UseApplicationMessageReceivedHandler(e =>            //handler message received
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;

                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($"Topic: {topic}. Message Received: {payload}");          //pass it to json parser
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });

            
            Task.Run(async () =>await mqttClient.StartAsync(managedOptions)).Wait();
            
            // Connecting
            Task.Run(() => this.SubscribeAsync(mqttClient,"test/")).Wait();
            Console.WriteLine("Topic subscribt");
            Task.Run(() => this.PublishAsync(mqttClient,"test/","hallo12")).Wait();
            Console.WriteLine("Topic gepublisht");
        }

        

        /*static void Main(string[] args)           //für test
        {
            // Display the number of command line arguments.
            Console.WriteLine("Los gehts");
            new MQTTClient();
            while(true) {

            }
        }*/


        private void setUpConnectionHandlers(IManagedMqttClient mqttClient) {
            mqttClient.UseConnectedHandler(e =>
            {
                 Console.WriteLine("Connected successfully with MQTT Brokers.");
            });
            mqttClient.UseDisconnectedHandler(e =>
            {
                 Console.WriteLine("Disconnected from MQTT Brokers.");
            });
        }
        public async Task SubscribeAsync(IManagedMqttClient mqttClient,string topic,int qos = 1) =>
        await mqttClient.SubscribeAsync(new TopicFilterBuilder()
            .WithTopic(topic)
            .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
            .Build());

        public async Task PublishAsync(IManagedMqttClient mqttClient,string topic, string payload, bool retainFlag = true, int qos = 1) => 
        await mqttClient.PublishAsync(new MqttApplicationMessageBuilder()
        .WithTopic(topic)
        .WithPayload(payload)
        .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
        .WithRetainFlag(retainFlag)
        .Build());
    } 
}

   