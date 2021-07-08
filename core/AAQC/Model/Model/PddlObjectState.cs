using System;

namespace Model.Model
{
    public class PddlObjectState
    {
        public PddlObjectSensors SensorStates { get; set; }
        public PddlObjectActuators ActuatorStates { get; set; }
    }

    public class PddlObjectSensors
    {
        public PddlSensorState TemperatureIn { set; get; }
        public PddlSensorState TemperatureOut { set; get; }
        public PddlSensorState HumidityOut { set; get; }
        public PddlSensorState AirPurityIn { set; get; }
        public PddlSensorState AirPurityOut { set; get; }
        public PddlSensorState Co2LevelIn { set; get; }
    }

    public class PddlObjectActuators
    {
        public bool IsVentilationActive { set; get; }
        public bool IsHeaterActive { set; get; }
        public bool IsAirConditionerActive { set; get; }
        public bool IsAirPurifierActive { set; get; }

        public bool Equals(PddlObjectActuators other)
        {
            return IsVentilationActive == other.IsVentilationActive && 
                   IsHeaterActive == other.IsHeaterActive &&
                   IsAirConditionerActive == other.IsAirConditionerActive &&
                   IsAirPurifierActive == other.IsAirPurifierActive;
        }
    }

    public enum PddlSensorState
    {
        BelowThreshold,
        Normal,
        AboveThreshold
    }
}