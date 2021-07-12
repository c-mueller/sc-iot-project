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

            problem = ParseHumidityOut(state.SensorState, problem);
            problem = ParseTemperatureIn(state.SensorState, problem);
            problem = ParseTemperatureOut(state.SensorState, problem);
            problem = ParseCo2In(state.SensorState, problem);
            problem = ParseAirPurityIn(state.SensorState, problem);
            problem = ParseAirPurityOut(state.SensorState, problem);

            problem = ParseVentilationState(state.ActuatorState, problem);
            problem = ParseHeaterState(state.ActuatorState, problem);
            problem = ParseAirConditionerState(state.ActuatorState, problem);
            problem = ParseAirPurifierState(state.ActuatorState, problem);

            return problem;
        }

        private static PddlProblem ParseHumidityOut(SensorState sensors, PddlProblem problem)
        {
            if (sensors.HumidityOut == ThresholdRelation.AboveThreshold)
                problem.AddInitState("humidity-high", Constants.HumidityOutObjectName);
            
            return problem;
        }

        private static PddlProblem ParseTemperatureIn(SensorState sensors, PddlProblem problem)
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

            return problem;
        }

        private static PddlProblem ParseTemperatureOut(SensorState sensors, PddlProblem problem)
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
            if (actuators.IsVentilationActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.VentilationObjectName);

            return problem;
        }

        private static PddlProblem ParseHeaterState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsHeaterActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.HeaterObjectName);

            return problem;
        }

        private static PddlProblem ParseAirConditionerState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsAirConditionerActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.AirConditionerObjectName);

            return problem;
        }

        private static PddlProblem ParseAirPurifierState(ActuatorState actuators, PddlProblem problem)
        {
            if (actuators.IsAirPurifierActive.GetValueOrDefault())
                problem.AddInitState("on", Constants.AirPurifierObjectName);

            return problem;
        }
    }
}