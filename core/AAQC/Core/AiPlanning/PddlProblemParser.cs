using System;
using Core.Model;
using Model;
using Model.Model;

namespace Core.AiPlanning
{
    public static class PddlProblemParser
    {
        public static PddlProblem Parse(PddlObjectState state)
        {
            var problem = new PddlProblem();
            
            problem = ParseHumidityOut(state.SensorStates, problem);

            problem = ParseTemperatureIn(state.SensorStates, problem);
            problem = ParseTemperatureOut(state.SensorStates, problem);

            problem = ParseCo2In(state.SensorStates, problem);

            problem = ParseAirPurityIn(state.SensorStates, problem);
            problem = ParseAirPurityOut(state.SensorStates, problem);
            
            return problem;
        }

        private static PddlProblem ParseHumidityOut(PddlObjectSensors sensors, PddlProblem problem)
        {
            switch (sensors.HumidityOut)
            {
                case PddlSensorState.AboveThreshold:
                    problem.AddInitState("humidity-high", Constants.HumidityOutObjectName);
                    break;
                case PddlSensorState.BelowThreshold:
                    problem.AddInitState("humidity-low", Constants.HumidityOutObjectName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return problem;
        }

        private static PddlProblem ParseTemperatureIn(PddlObjectSensors sensors, PddlProblem problem)
        {
            switch (sensors.TemperatureIn)
            {
                case PddlSensorState.AboveThreshold:
                    problem.AddInitState("temperature-high", Constants.TemperatureOutObjectName);
                    break;
                case PddlSensorState.BelowThreshold:
                    problem.AddInitState("temperature-low", Constants.TemperatureOutObjectName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return problem;
        }

        private static PddlProblem ParseTemperatureOut(PddlObjectSensors sensors, PddlProblem problem)
        {
            switch (sensors.TemperatureOut)
            {
                case PddlSensorState.AboveThreshold:
                    problem.AddInitState("temperature-high", Constants.TemperatureInObjectName);
                    break;
                case PddlSensorState.BelowThreshold:
                    problem.AddInitState("temperature-low", Constants.TemperatureInObjectName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return problem;
        }

        private static PddlProblem ParseCo2In(PddlObjectSensors sensors, PddlProblem problem)
        {
            if (sensors.Co2LevelIn == PddlSensorState.AboveThreshold)
            {
                problem.AddInitState("co2-level-emergency", Constants.Co2LevelInObjectName);
            }

            return problem;
        }

        private static PddlProblem ParseAirPurityIn(PddlObjectSensors sensors, PddlProblem problem)
        {
            if (sensors.AirPurityIn == PddlSensorState.AboveThreshold)
            {
                problem.AddInitState("air-purity-bad", Constants.AirPurityInObjectName);
            }

            return problem;
        }

        private static PddlProblem ParseAirPurityOut(PddlObjectSensors sensors, PddlProblem problem)
        {
            if (sensors.AirPurityOut == PddlSensorState.AboveThreshold)
            {
                problem.AddInitState("air-purity-bad", Constants.AirPurityOutObjectName);
            }

            return problem;
        }
    }
}