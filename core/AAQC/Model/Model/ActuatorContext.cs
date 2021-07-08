using System.Collections.Generic;

namespace Model.Model
{
    public class ActuatorContext
    {
        public List<ActuatorInfo> ActuatorStates { get; set; }
    }

    public class ActuatorInfo
    {
        public string Name { get; set; }
        public ActuatorType Type { get; set; }
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