using System;
using Model;
using Model.Model;

namespace Core.AiPlanning
{
    public static class PddlProblemParser
    {
        public static PddlProblem Parse(ObjectState state)
        {
            var problem = new PddlProblem();

            problem = ParseHumidityOut(state.SensorStates, problem);
            problem = ParseTemperatureIn(state.SensorStates, problem);
            problem = ParseTemperatureOut(state.SensorStates, problem);
            problem = ParseCo2In(state.SensorStates, problem);
            problem = ParseAirPurityIn(state.SensorStates, problem);
            problem = ParseAirPurityOut(state.SensorStates, problem);

            problem = ParseVentilationState(state.ActuatorStates, problem);
            problem = ParseHeaterState(state.ActuatorStates, problem);
            problem = ParseAirConditionerState(state.ActuatorStates, problem);
            problem = ParseAirPurifierState(state.ActuatorStates, problem);

            return problem;
        }

        private static PddlProblem ParseHumidityOut(SensorState sensors, PddlProblem problem)
        {
            switch (sensors.HumidityOut)
            {
                case ThresholdRelation.AboveThreshold:
                    problem.AddInitState("humidity-high", Constants.HumidityOutObjectName);
                    break;
                case ThresholdRelation.BelowThreshold:
                    problem.AddInitState("humidity-low", Constants.HumidityOutObjectName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return problem;
        }

        private static PddlProblem ParseTemperatureIn(SensorState sensors, PddlProblem problem)
        {
            switch (sensors.TemperatureIn)
            {
                case ThresholdRelation.AboveThreshold:
                    problem.AddInitState("temperature-high", Constants.TemperatureOutObjectName);
                    break;
                case ThresholdRelation.BelowThreshold:
                    problem.AddInitState("temperature-low", Constants.TemperatureOutObjectName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return problem;
        }

        private static PddlProblem ParseTemperatureOut(SensorState sensors, PddlProblem problem)
        {
            switch (sensors.TemperatureOut)
            {
                case ThresholdRelation.AboveThreshold:
                    problem.AddInitState("temperature-high", Constants.TemperatureInObjectName);
                    break;
                case ThresholdRelation.BelowThreshold:
                    problem.AddInitState("temperature-low", Constants.TemperatureInObjectName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return problem;
        }

        private static PddlProblem ParseCo2In(SensorState sensors, PddlProblem problem)
        {
            if (sensors.Co2LevelIn == ThresholdRelation.AboveThreshold)
                problem.AddInitState("co2-level-emergency", Constants.Co2LevelInObjectName);

            return problem;
        }

        private static PddlProblem ParseAirPurityIn(SensorState sensors, PddlProblem problem)
        {
            if (sensors.AirPurityIn == ThresholdRelation.AboveThreshold)
                problem.AddInitState("air-purity-bad", Constants.AirPurityInObjectName);

            return problem;
        }

        private static PddlProblem ParseAirPurityOut(SensorState sensors, PddlProblem problem)
        {
            if (sensors.AirPurityOut == ThresholdRelation.AboveThreshold)
                problem.AddInitState("air-purity-bad", Constants.AirPurityOutObjectName);

            return problem;
        }

        private static PddlProblem ParseVentilationState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsVentilationActive)
                problem.AddInitState("on", Constants.VentilationObjectName);

            return problem;
        }

        private static PddlProblem ParseHeaterState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsHeaterActive)
                problem.AddInitState("on", Constants.HeaterObjectName);

            return problem;
        }

        private static PddlProblem ParseAirConditionerState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsAirConditionerActive)
                problem.AddInitState("on", Constants.AirConditionerObjectName);

            return problem;
        }

        private static PddlProblem ParseAirPurifierState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsAirPurifierActive)
                problem.AddInitState("on", Constants.AirPurifierObjectName);

            return problem;
        }
    }
}