using System;
using Model;
using Model.Model;

namespace Core.AiPlanning
{
    public static class SensorContextEvaluator
    {
        public static PddlObjectState Evaluate(SensorContext context)
        {
            var sensors = new PddlObjectSensors();
            foreach (var location in context.Locations)
            {
                foreach (var measure in location.Measures)
                {
                    switch (measure.Type)
                    {
                        case SensorType.Humidity:
                        {
                            sensors = EvaluateHumidity(measure, sensors);
                            break;
                        }
                        case SensorType.Temperature:
                        {
                            sensors = EvaluateTemperature(measure, sensors, location.Location);
                            break;
                        }
                        case SensorType.CO2:
                        {
                            sensors = EvaluateCo2(measure, sensors);
                            break;
                        }
                        case SensorType.ParticulateMatter:
                        {
                            sensors = EvaluateParticulateMatter(measure, sensors, location.Location);
                            break;
                        }
                        default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }

            return new PddlObjectState
            {
                SensorStates = sensors,
            };
        }

        private static PddlObjectSensors EvaluateHumidity(SensorMeasure sensorMeasure, PddlObjectSensors sensors)
        {
            if (sensorMeasure.Value > Constants.HumidityOutsideThreshold)
            {
                sensors.HumidityOut = PddlSensorState.AboveThreshold;
            }
            else if (sensorMeasure.Value < Constants.HumidityOutsideThreshold)
            {
                sensors.HumidityOut = PddlSensorState.BelowThreshold;
            }
            else
            {
                sensors.HumidityOut = PddlSensorState.Normal;
            }

            return sensors;
        }

        private static PddlObjectSensors EvaluateTemperature(SensorMeasure sensorMeasure, PddlObjectSensors sensors,
            Location location)
        {
            switch (location)
            {
                case Location.Outside:
                {
                    if (sensorMeasure.Value > Constants.TemperatureOutsideUpperThreshold)
                    {
                        sensors.TemperatureOut = PddlSensorState.AboveThreshold;
                    }
                    else if (sensorMeasure.Value < Constants.TemperatureOutsideLowerThreshold)
                    {
                        sensors.TemperatureOut = PddlSensorState.BelowThreshold;
                    }
                    else
                    {
                        sensors.TemperatureOut = PddlSensorState.Normal;
                    }

                    break;
                }
                case Location.Inside:
                {
                    if (sensorMeasure.Value > Constants.TemperatureInsideUpperThreshold)
                    {
                        sensors.TemperatureIn = PddlSensorState.AboveThreshold;
                    }
                    else if (sensorMeasure.Value < Constants.TemperatureInsideLowerThreshold)
                    {
                        sensors.TemperatureIn = PddlSensorState.BelowThreshold;
                    }
                    else
                    {
                        sensors.TemperatureIn = PddlSensorState.Normal;
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

        private static PddlObjectSensors EvaluateCo2(SensorMeasure sensorMeasure, PddlObjectSensors sensors)
        {
            if (sensorMeasure.Value > Constants.Co2EmergencyThreshold)
            {
                sensors.Co2LevelIn = PddlSensorState.AboveThreshold;
            }
            else
            {
                sensors.Co2LevelIn = PddlSensorState.BelowThreshold;
            }

            return sensors;
        }

        private static PddlObjectSensors EvaluateParticulateMatter(SensorMeasure sensorMeasure,
            PddlObjectSensors sensors, Location location)
        {
            switch (location)
            {
                case Location.Outside:
                {
                    if (sensorMeasure.Value > Constants.AirPurityOutsideUpperThreshold)
                    {
                        sensors.AirPurityOut = PddlSensorState.AboveThreshold;
                    }
                    else if (sensorMeasure.Value < Constants.AirPurityOutsideLowerThreshold)
                    {
                        sensors.AirPurityOut = PddlSensorState.BelowThreshold;
                    }
                    else
                    {
                        sensors.AirPurityOut = PddlSensorState.Normal;
                    }

                    break;
                }
                case Location.Inside:
                {
                    if (sensorMeasure.Value > Constants.AirPurityInsideUpperThreshold)
                    {
                        sensors.AirPurityIn = PddlSensorState.AboveThreshold;
                    }
                    else if (sensorMeasure.Value < Constants.AirPurityInsideLowerThreshold)
                    {
                        sensors.AirPurityIn = PddlSensorState.BelowThreshold;
                    }
                    else
                    {
                        sensors.AirPurityIn = PddlSensorState.Normal;
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