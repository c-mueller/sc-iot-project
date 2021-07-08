namespace Model.Model
{
    public class PddlObjectState
    {
        public PddlObjectSensors SensorStates { get; set; }
        public PddlObjectActuators ActuatorStates { get; set; }

        public bool AreAllInsideSensorsNormal()
        {
            return SensorStates.AreAllInsideSensorsNormal();
        }
    }

    public class PddlObjectSensors
    {
        public PddlSensorState TemperatureIn { set; get; }
        public PddlSensorState TemperatureOut { set; get; }
        public PddlSensorState HumidityOut { set; get; }
        public PddlSensorState AirPurityIn { set; get; }
        public PddlSensorState AirPurityOut { set; get; }
        public PddlSensorState Co2LevelIn { set; get; }

        public bool AreAllInsideSensorsNormal()
        {
            return TemperatureIn == PddlSensorState.Normal && AirPurityIn == PddlSensorState.Normal &&
                   Co2LevelIn == PddlSensorState.Normal;
        }
    }

    public class PddlObjectActuators
    {
        public PddlActuatorState Ventilation { set; get; }
        public PddlActuatorState Heater { set; get; }
        public PddlActuatorState AirConditioner { set; get; }
        public PddlActuatorState AirPurifier { set; get; }
    }

    public enum PddlActuatorState
    {
        On,
        Off,
    }

    public enum PddlSensorState
    {
        BelowThreshold,
        Normal,
        AboveThreshold
    }
}