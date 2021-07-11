using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Model.Model
{
    public class SensorContext
    {
        public List<SensorLocation> Locations { get; set; }

        public void SubmitMeasurement(SensorInput sensorInput)
        {
            if (Locations == null)
            {
                Locations = new List<SensorLocation>();
            }

            var location = sensorInput.Location == "outdoors" ? Location.Outside : Location.Inside;

            foreach (var sensorLocation in Locations)
            {
                if (sensorLocation.LocationId == sensorInput.Location)
                {
                    foreach (var measurement in sensorLocation.Measures)
                    {
                        if (measurement.Type == sensorInput.SensorType)
                        {
                            measurement.Value = sensorInput.Value;
                            measurement.MeasuredAt = sensorInput.Timestamp;
                            return;
                        }
                    }

                    sensorLocation.Measures.Add(sensorInput.ToSensorMeasure());
                    return;
                }
            }

            Locations.Add(new SensorLocation
            {
                Location = location,
                LocationId = sensorInput.Location,
                Measures = new List<SensorMeasure> {sensorInput.ToSensorMeasure()}
            });
        }
        
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
        [DataMember(Name = "location")] public string Location { get; set; }

        [DataMember(Name = "sensortype")] public SensorType SensorType { get; set; }

        [DataMember(Name = "value")] public double Value { get; set; }

        [DataMember(Name = "timestamp")] public DateTime Timestamp { get; set; }

        public SensorMeasure ToSensorMeasure()
        {
            return new SensorMeasure
            {
                Name = $"{SensorType.ToString()}-{Location}",
                Type = SensorType,
                Value = Value,
                MeasuredAt = Timestamp
            };
        }
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