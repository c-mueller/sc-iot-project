using System.Collections.Generic;

namespace Model.Model
{
    public class ActuatorContext
    {
        public string Name { get; set; }
        public ActuatorType Type { get; set; }
        public ActuatorInfo ActuatorInfo { get; set; }
    }

    public class ActuatorInfo
    {
        public bool Active { get; set; }
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