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

            var url = "https://test.mosquitto.org/";
            //var username = "user";
            //var psw = "user";
            var port = 1883;            //unauthenticated, unencrypted
            init(url,port);
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


            // Connecting
            Task.Run(() => this.SubscribeAsync(mqttClient,"$share:mygroup:/mytopic")).Wait();
            
            Task.Run(() => this.PublishAsync(mqttClient,"$share:mygroup:/mytopic","hallo")).Wait();
        }

        private void init(string url,int port) {
            var options = new ManagedMqttClientOptionsBuilder()
                            .WithAutoReconnectDelay(TimeSpan.FromSeconds(30))
                            .WithClientOptions(new MqttClientOptionsBuilder()
                                .WithClientId(Guid.NewGuid().ToString())
                                .WithTcpServer(url, port)
                                //.WithCredentials(username, psw)
                                .WithCleanSession()
                                .Build())
                            .Build();
        }

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

   