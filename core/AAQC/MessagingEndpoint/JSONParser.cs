using System;
using Newtonsoft.Json;
using Model.Model;


namespace MessagingEndpoint
{
    public class JSONParser
    {
        /*public static String parseMessageToJSON(ActuatorState actuatorState) {       //send message to actuators --> we need json protocol
           
            return null;
        }*/

        public static SensorContext parseJSONtoModel(string message) {  //we received json message and want SensorContext
            SensorContext sensorContext = new SensorContext();//endpoint has to queue all incoming messages for one sensorcontext message
         
            return sensorContext;
        }
    }

}