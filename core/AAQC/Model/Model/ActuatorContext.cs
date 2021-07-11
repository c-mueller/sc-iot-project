using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model.Model
{
    public class ActuatorContext
    {
        public string Name { get; set; }
        public ActuatorType Type { get; set; }
        public ActuatorInfo ActuatorInfo { get; set; }
    }
    
    [DataContract]
    public class ActuatorInfo
    {
        [DataMember(Name = "active")]
        public bool Active { get; set; }
        
        [DataMember(Name = "targetvalue")] 
        public double TargetValue { get; set; }
    }
    
    public enum ActuatorType
    {
        Ventilation,
        Heater,
        AirConditioner,
        AirPurifier,
    }
}