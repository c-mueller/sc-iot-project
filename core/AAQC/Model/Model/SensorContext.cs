using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model.Model
{
    public class SensorContext
    {
        public List<SensorLocation> Locations { get; set; }
    }

    public class SensorLocation
    {
        public string LocationId { get; set; }
        public Location Location { get; set; }
        public List<SensorMeasure> Measures { get; set; }
    }

    public class SensorMeasure
    {
        public string Name { get; set; }
        public SensorType Type { get; set; }
        public double Value { get; set; }
        public DateTime MeasuredAt { get; set; }
    }

    [DataContract]
    public class SensorInput
    {
        [DataMember(Name = "location")]
        public string Location { get; set; }
        
        [DataMember(Name = "sensortype")]
        public string SensorType { get; set; }
        
        [DataMember(Name = "value")]
        public double Value { get; set; }
        
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; set; }
    }

    public enum SensorType
    {
        Temperature,
        Humidity,
        ParticulateMatter,
        CO2,
    }

    public enum Location
    {
        Inside,
        Outside,
    }
}