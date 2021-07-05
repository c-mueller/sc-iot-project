using System.Collections.Generic;

namespace Model.Model
{
    public class ActuatorContext
    {
        private List<ActuatorState> ActuatorStates { get; set; }
    }

    public class ActuatorState
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