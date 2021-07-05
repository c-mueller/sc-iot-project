using System;
using Model.Model;

namespace Core.AiPlanning
{
    public static class SensorContextEvaluator
    {
        public static void Evaluate(SensorContext context)
        {
            foreach (var location in context.Locations)
            {
                foreach (var measure in location.Measures)
                {
                    switch (measure.Type)
                    {
                        case SensorType.Humidity:
                        {
                            EvaluateHumidity(measure);
                            break;
                        }
                        case SensorType.Temperature:
                        {
                            EvaluateTemperature(measure, location.Location);
                            break;
                        }
                        case SensorType.CO2:
                        {
                            EvaluateCo2(measure);
                            break;
                        }
                        case SensorType.ParticulateMatter:
                        {
                            EvaluateParticulateMatter(measure, location.Location);
                            break;
                        }
                        default:
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }

        private static void EvaluateHumidity(SensorState state)
        {
            
        }
        
        private static void EvaluateTemperature(SensorState state, Location location)
        {
            switch (location)
            {
                case Location.Outside:
                {
                    break;
                }
                case Location.Inside:
                {
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
                }
            }
        }
        
        private static void EvaluateCo2(SensorState state)
        {
            
        }
        
        private static void EvaluateParticulateMatter(SensorState state, Location location)
        {
            switch (location)
            {
                case Location.Outside:
                {
                    break;
                }
                case Location.Inside:
                {
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(location), location, null);
                }
            }
        }
    }
}