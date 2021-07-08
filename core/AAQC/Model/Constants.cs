using Model.Model;

namespace Model
{
    public static class Constants
    {
        // Threshold constants TODO fix values
        public const double TemperatureOutsideUpperThreshold = 20;
        public const double TemperatureOutsideLowerThreshold = 20;
        public const double TemperatureInsideUpperThreshold = 20;
        public const double TemperatureInsideLowerThreshold = 20;
        public const double AirPurityOutsideUpperThreshold = 20;
        public const double AirPurityOutsideLowerThreshold = 20;
        public const double AirPurityInsideUpperThreshold = 20;
        public const double AirPurityInsideLowerThreshold = 20;
        public const double HumidityOutsideThreshold = 20;
        public const double Co2EmergencyThreshold = 20;
            
        // PDDL object names
        public const string TemperatureInObjectName = "ti";
        public const string TemperatureOutObjectName = "to";
        public const string HumidityOutObjectName = "ho";
        public const string AirPurityInObjectName = "ai";
        public const string AirPurityOutObjectName = "ao";
        public const string Co2LevelInObjectName = "ci";
        
        public static readonly PddlObjectState InitialPddlObjectState = new PddlObjectState
        {
            SensorStates = new PddlObjectSensors
            {
                TemperatureIn = PddlSensorState.Normal,
                TemperatureOut = PddlSensorState.Normal,
                HumidityOut = PddlSensorState.Normal,
                AirPurityIn = PddlSensorState.Normal,
                AirPurityOut = PddlSensorState.Normal,
                Co2LevelIn = PddlSensorState.Normal,
            },
            ActuatorStates = new PddlObjectActuators
            {
                Ventilation = PddlActuatorState.Off,
                Heater = PddlActuatorState.Off,
                AirConditioner = PddlActuatorState.Off,
                AirPurifier = PddlActuatorState.Off,
            },
        };
    }
}