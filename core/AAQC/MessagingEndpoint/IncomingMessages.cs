using Model.Interfaces;
using Model.Model;
using MQTTnet.Extensions.ManagedClient;

namespace MessagingEndpoint
{
    public class IncomingMessages : ISensorContextConsumer 
    {
        MQTTEndpoint mQTTEndpoint;
         public IncomingMessages(MQTTEndpoint mQTTEndpoint, IManagedMqttClient mqttClient) {
            this.mQTTEndpoint = mQTTEndpoint;
        }
        public void Consume(SensorContext sensorContext) {
            
        }
    }

}