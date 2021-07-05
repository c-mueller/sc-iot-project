using System;
using System.Collections.Generic;

namespace Model.Model
{
    public class SensorContext
    {
        public List<SensorLocation> Locations { get; set; }
    }

    public class SensorLocation
    {
        public string Location { get; set; }
        public List<SensorState> Measures { get; set; }
    }

    public class SensorState
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