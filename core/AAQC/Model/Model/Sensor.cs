using System;

namespace Core.Model
{
    public class Sensor
    {
        public string Name { get; set; }
        public SensorType Type { get; set; }   
        public double Value { get; set; }
        public DateTime MeasuredAt { get; set; }
    }
    
    public enum SensorType
    {
        Temperature,
        Humidity,
        ParticulateMatter,
        CO2,
    }
}