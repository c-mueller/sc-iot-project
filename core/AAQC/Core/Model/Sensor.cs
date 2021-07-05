namespace Core.Model
{
    public class Sensor
    {
        public SensorType Type { get; set; }   
        public string Value { get; set; }
        public string Timestamp { get; set; }
    }
    
    public enum SensorType
    {
        Temperature,
        Humidity,
        ParticulateMatter,
        Co2,
    }
}