using Model;
using Model.Model;
using Serilog;

namespace Core.AiPlanning
{
    public static class PddlProblemParser
    {
        public static PddlProblem Parse(ObjectState state)
        {
            Log.Information("[AI Planner] Parsing PDDL problem");
            var problem = new PddlProblem();

            problem.ParseHumidityOut(state.SensorState);
            problem.ParseTemperatureIn(state.SensorState);
            problem.ParseTemperatureOut(state.SensorState);
            problem.ParseCo2In(state.SensorState);
            problem.ParseAirPurityIn(state.SensorState);
            problem.ParseAirPurityOut(state.SensorState);

            problem.ParseVentilationState(state.ActuatorState);
            problem.ParseHeaterState(state.ActuatorState);
            problem.ParseAirConditionerState(state.ActuatorState);
            problem.ParseAirPurifierState(state.ActuatorState);

            return problem;
        }

        private static void ParseHumidityOut(this PddlProblem problem, SensorState sensors)
        {
            if (sensors.HumidityOut == ThresholdRelation.AboveThreshold)
                problem.AddInitState("humidity-high", Constants.HumidityOutObjectName);
        }

        private static void ParseTemperatureIn(this PddlProblem problem, SensorState sensors)
        {
            switch (sensors.TemperatureIn)
            {
                case ThresholdRelation.AboveThreshold:
                    problem.AddInitState("temperature-high", Constants.TemperatureInObjectName);
                    break;
                case ThresholdRelation.BelowThreshold:
                    problem.AddInitState("temperature-low", Constants.TemperatureInObjectName);
                    break;
            }
        }

        private static void ParseTemperatureOut(this PddlProblem problem, SensorState sensors)
        {
            switch (sensors.TemperatureOut)
            {
                case ThresholdRelation.AboveThreshold:
                    problem.AddInitState("temperature-high", Constants.TemperatureOutObjectName);
                    break;
                case ThresholdRelation.BelowThreshold:
                    problem.AddInitState("temperature-low", Constants.TemperatureOutObjectName);
                    break;
            }
        }

        private static void ParseCo2In(this PddlProblem problem, SensorState sensors)
        {
            if (sensors.Co2LevelIn == ThresholdRelation.AboveThreshold)
                problem.AddInitState("co2-level-emergency", Constants.Co2LevelInObjectName);
        }

        private static void ParseAirPurityIn(this PddlProblem problem, SensorState sensors)
        {
            if (sensors.AirPurityIn == ThresholdRelation.AboveThreshold)
                problem.AddInitState("air-purity-bad", Constants.AirPurityInObjectName);
        }

        private static void ParseAirPurityOut(this PddlProblem problem, SensorState sensors)
        {
            if (sensors.AirPurityOut == ThresholdRelation.AboveThreshold)
                problem.AddInitState("air-purity-bad", Constants.AirPurityOutObjectName);
        }

        private static void ParseVentilationState(this PddlProblem problem, ActuatorState actuators)
        {
            if (actuators.IsVentilationActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.VentilationObjectName);
        }

        private static void ParseHeaterState(this PddlProblem problem, ActuatorState actuators)
        {
            if (actuators.IsHeaterActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.HeaterObjectName);
        }

        private static void ParseAirConditionerState(this PddlProblem problem, ActuatorState actuators)
        {
            if (actuators.IsAirConditionerActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.AirConditionerObjectName);
        }

        private static void ParseAirPurifierState(this PddlProblem problem, ActuatorState actuators)
        {
            if (actuators.IsAirPurifierActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.AirPurifierObjectName);
        }
    }
}