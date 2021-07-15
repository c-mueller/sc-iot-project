using Model;
using Model.Model;
using Serilog;

namespace Core.AiPlanning
{
    public static class SensorContextEvaluator
    {
        public static ObjectState Evaluate(SensorContext context)
        {
            Log.Information("[AI Planner] Evaluating sensor context");
            var sensors = new SensorState();
            foreach (var location in context.Locations)
            {
                foreach (var measure in location.Measures)
                {
                    switch (measure.Type)
                    {
                        case SensorType.Humidity:
                            sensors.EvaluateHumidity(measure);
                            break;
                        case SensorType.Temperature:
                            sensors.EvaluateTemperature(measure, location.Location);
                            break;
                        case SensorType.CO2:
                            sensors.EvaluateCo2(measure);
                            break;
                        case SensorType.ParticulateMatter:
                            sensors.EvaluateParticulateMatter(measure, location.Location);
                            break;
                        default:
                            Log.Warning("[AI Planning] Measure found with unknown sensor type");
                            break;
                    }
                }
            }

            return new ObjectState
            {
                SensorState = sensors,
                ActuatorState = new ActuatorState(),
            };
        }

        private static void EvaluateHumidity(this SensorState sensors, SensorMeasure measure)
        {
            if (measure.Value > Constants.HumidityOutsideThreshold)
            {
                sensors.HumidityOut = ThresholdRelation.AboveThreshold;
            }
            else if (measure.Value < Constants.HumidityOutsideThreshold)
            {
                sensors.HumidityOut = ThresholdRelation.BelowThreshold;
            }
            else
            {
                sensors.HumidityOut = ThresholdRelation.Normal;
            }
        }

        private static void EvaluateTemperature(this SensorState sensors, SensorMeasure measure,
            Location location)
        {
            switch (location)
            {
                case Location.Outside:
                {
                    if (measure.Value > Constants.TemperatureOutsideUpperThreshold)
                    {
                        sensors.TemperatureOut = ThresholdRelation.AboveThreshold;
                    }
                    else if (measure.Value < Constants.TemperatureOutsideLowerThreshold)
                    {
                        sensors.TemperatureOut = ThresholdRelation.BelowThreshold;
                    }
                    else
                    {
                        sensors.TemperatureOut = ThresholdRelation.Normal;
                    }

                    break;
                }
                case Location.Inside:
                {
                    if (measure.Value > Constants.TemperatureInsideUpperThreshold)
                    {
                        sensors.TemperatureIn = ThresholdRelation.AboveThreshold;
                    }
                    else if (measure.Value < Constants.TemperatureInsideLowerThreshold)
                    {
                        sensors.TemperatureIn = ThresholdRelation.BelowThreshold;
                    }
                    else
                    {
                        sensors.TemperatureIn = ThresholdRelation.Normal;
                    }

                    break;
                }
                default:
                {
                    Log.Warning("[AI Planning] Sensor found with unknown location");
                    break;
                }
            }
        }

        private static void EvaluateCo2(this SensorState sensors, SensorMeasure measure)
        {
            sensors.Co2LevelIn = measure.Value > Constants.Co2EmergencyThreshold
                ? ThresholdRelation.AboveThreshold
                : ThresholdRelation.BelowThreshold;
        }

        private static void EvaluateParticulateMatter(this SensorState sensors, SensorMeasure measure,
            Location location)
        {
            switch (location)
            {
                case Location.Outside:
                {
                    if (measure.Value > Constants.AirPurityOutsideUpperThreshold)
                    {
                        sensors.AirPurityOut = ThresholdRelation.AboveThreshold;
                    }
                    else if (measure.Value < Constants.AirPurityOutsideLowerThreshold)
                    {
                        sensors.AirPurityOut = ThresholdRelation.BelowThreshold;
                    }
                    else
                    {
                        sensors.AirPurityOut = ThresholdRelation.Normal;
                    }

                    break;
                }
                case Location.Inside:
                {
                    if (measure.Value > Constants.AirPurityInsideUpperThreshold)
                    {
                        sensors.AirPurityIn = ThresholdRelation.AboveThreshold;
                    }
                    else if (measure.Value < Constants.AirPurityInsideLowerThreshold)
                    {
                        sensors.AirPurityIn = ThresholdRelation.BelowThreshold;
                    }
                    else
                    {
                        sensors.AirPurityIn = ThresholdRelation.Normal;
                    }

                    break;
                }
                default:
                {
                    Log.Warning("[AI Planning] Sensor found with unknown location");
                    break;
                }
            }
        }
    }
}