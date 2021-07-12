namespace Model.Model
{
    public class ObjectState
    {
        public SensorState SensorState { get; set; }
        public ActuatorState ActuatorState { get; set; }
    }

    public class SensorState
    {
        public SensorState()
        {
            TemperatureIn = ThresholdRelation.Normal;
            TemperatureOut = ThresholdRelation.Normal;
            HumidityOut = ThresholdRelation.Normal;
            AirPurityIn = ThresholdRelation.Normal;
            AirPurityOut = ThresholdRelation.Normal;
            Co2LevelIn = ThresholdRelation.Normal;
        }

        public ThresholdRelation TemperatureIn { set; get; }
        public ThresholdRelation TemperatureOut { set; get; }
        public ThresholdRelation HumidityOut { set; get; }
        public ThresholdRelation AirPurityIn { set; get; }
        public ThresholdRelation AirPurityOut { set; get; }
        public ThresholdRelation Co2LevelIn { set; get; }
    }

    public class ActuatorState
    {
        public bool? IsVentilationActive { set; get; }
        public bool? IsHeaterActive { set; get; }
        public bool? IsAirConditionerActive { set; get; }
        public bool? IsAirPurifierActive { set; get; }

        public bool Equals(ActuatorState other)
        {
            return IsVentilationActive == other.IsVentilationActive &&
                   IsHeaterActive == other.IsHeaterActive &&
                   IsAirConditionerActive == other.IsAirConditionerActive &&
                   IsAirPurifierActive == other.IsAirPurifierActive;
        }
    }

    public enum ThresholdRelation
    {
        BelowThreshold,
        Normal,
        AboveThreshold
    }
}