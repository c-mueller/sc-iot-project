using System.Collections.Generic;
using Model.Model;

namespace Core.AiPlanning
{
    public static class PddlPlanParser
    {
        public static ActuatorState Parse(string plan)
        {
            var steps = ParseSteps(plan);

            var actuators = new ActuatorState();
            foreach (var step in steps)
            {
                if (step.StartsWith("deactivate"))
                    actuators = DecideActuatorState(step, actuators, false);
                else
                    actuators = DecideActuatorState(step, actuators, true);
            }

            return actuators;
        }

        private static IEnumerable<string> ParseSteps(string plan)
        {
            var steps = plan.Split("\n");
            for (var i = 0; i < steps.Length; i++)
            {
                steps[i] = CleanUpStep(steps[i]);
            }

            return steps;
        }

        private static string CleanUpStep(string uncleanedStep)
        {
            return uncleanedStep.Split()[0].Trim('(').ToLower();
        }

        private static ActuatorState DecideActuatorState(string step, ActuatorState actuators,
            bool isActive)
        {
            switch (step)
            {
                case "ventilation":
                    actuators.IsVentilationActive = isActive;
                    break;
                case "heater":
                    actuators.IsHeaterActive = isActive;
                    break;
                case "airconditioner":
                    actuators.IsAirConditionerActive = isActive;
                    break;
                case "airpurifier":
                    actuators.IsAirPurifierActive = isActive;
                    break;
            }

            return actuators;
        }
    }
}