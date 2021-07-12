using Model.Model;

namespace Model
{
    public static class Constants
    {
        // Threshold constants
        public const double TemperatureOutsideUpperThreshold = 25;
        public const double TemperatureOutsideLowerThreshold = 15;
        public const double TemperatureInsideUpperThreshold = 22;
        public const double TemperatureInsideLowerThreshold = 20;
        public const double AirPurityOutsideUpperThreshold = 40;
        public const double AirPurityOutsideLowerThreshold = 0; // TODO remove maybe
        public const double AirPurityInsideUpperThreshold = 50;
        public const double AirPurityInsideLowerThreshold = 30;
        public const double HumidityOutsideThreshold = 60;
        public const double Co2EmergencyThreshold = 1000;
        
        // Actuator Names
        public const string VentilationName = "ventilation";
        public const string HeaterName = "heater";
        public const string AirConditionerName = "air-conditioner";
        public const string AirPurifierName = "air-purifier";

        // PDDL sensor object names
        public const string TemperatureInObjectName = "ti";
        public const string TemperatureOutObjectName = "to";
        public const string HumidityOutObjectName = "ho";
        public const string AirPurityInObjectName = "ai";
        public const string AirPurityOutObjectName = "ao";
        public const string Co2LevelInObjectName = "ci";
        
        //PDDL actuator object names
        public const string VentilationObjectName = "v";
        public const string HeaterObjectName = "h";
        public const string AirConditionerObjectName = "ac";
        public const string AirPurifierObjectName = "ap";
        
        public static readonly ObjectState InitialObjectState = new ObjectState
        {
            SensorState = new SensorState
            {
                TemperatureIn = ThresholdRelation.Normal,
                TemperatureOut = ThresholdRelation.Normal,
                HumidityOut = ThresholdRelation.Normal,
                AirPurityIn = ThresholdRelation.Normal,
                AirPurityOut = ThresholdRelation.Normal,
                Co2LevelIn = ThresholdRelation.Normal,
            },
            ActuatorState = new ActuatorState
            {
                IsVentilationActive = false,
                IsHeaterActive = false,
                IsAirConditionerActive = false,
                IsAirPurifierActive = false,
            },
        };
    }
}