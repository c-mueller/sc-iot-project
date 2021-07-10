using System;
using Model;
using Model.Model;

namespace Core.AiPlanning
{
    public static class SensorContextEvaluator
    {
        public static ObjectState Evaluate(SensorContext context)
        {
            var sensors = new SensorState();
            foreach (var location in context.Locations)
            {
                foreach (var measure in location.Measures)
                {
                    switch (measure.Type)
                    {
                        case SensorType.Humidity:
                            sensors = EvaluateHumidity(measure, sensors);
                            break;
                        case SensorType.Temperature:
                            sensors = EvaluateTemperature(measure, sensors, location.Location);
                            break;
                        case SensorType.CO2:
                            sensors = EvaluateCo2(measure, sensors);
                            break;
                        case SensorType.ParticulateMatter:
                            sensors = EvaluateParticulateMatter(measure, sensors, location.Location);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return new ObjectState
            {
                SensorStates = sensors,
            };
        }

        private static SensorState EvaluateHumidity(SensorMeasure measure, SensorState sensors)
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

            return sensors;
        }

        private static SensorState EvaluateTemperature(SensorMeasure measure, SensorState sensors,
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
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
                }
            }

            return sensors;
        }

        private static SensorState EvaluateCo2(SensorMeasure measure, SensorState sensors)
        {
            if (measure.Value > Constants.Co2EmergencyThreshold)
            {
                sensors.Co2LevelIn = ThresholdRelation.AboveThreshold;
            }
            else
            {
                sensors.Co2LevelIn = ThresholdRelation.BelowThreshold;
            }

            return sensors;
        }

        private static SensorState EvaluateParticulateMatter(SensorMeasure measure,
            SensorState sensors, Location location)
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
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
                }
            }

            return sensors;
        }
    }
}