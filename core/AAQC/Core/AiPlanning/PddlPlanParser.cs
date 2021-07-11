using System.Collections.Generic;
using System.Linq;
using Model.Model;

namespace Core.AiPlanning
{
    public static class PddlPlanParser
    {
        private const string VentilationPddlName = "ventilation";
        private const string HeaterPddlName = "heater";
        private const string AirConditionerPddlName = "airconditioner";
        private const string AirPurifierPddlName = "airpurifier";

        public static ActuatorState Parse(string plan)
        {
            var steps = SplitPlanIntoSteps(plan).ToList();

            var activateSteps = FindActivateActuatorStep(steps).ToList();
            var deactivateSteps = FindDeactivateActuatorStep(steps, activateSteps).ToList();

            var actuatorState = new ActuatorState();
            foreach (var activateStep in activateSteps)
                actuatorState = DecideActuatorState(activateStep, actuatorState, true);

            foreach (var deactivateStep in deactivateSteps)
                actuatorState = DecideActuatorState(deactivateStep, actuatorState, false);

            return actuatorState;
        }

        private static IEnumerable<string> SplitPlanIntoSteps(string plan)
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

        private static IEnumerable<string> FindActivateActuatorStep(IEnumerable<string> steps)
        {
            var activatedActuators = new List<string>();
            foreach (var step in steps.Where(s => s.StartsWith("activate")))
            {
                switch (step)
                {
                    case VentilationPddlName:
                        activatedActuators.Add(VentilationPddlName);
                        break;
                    case HeaterPddlName:
                        activatedActuators.Add(HeaterPddlName);
                        break;
                    case AirConditionerPddlName:
                        activatedActuators.Add(AirConditionerPddlName);
                        break;
                    case AirPurifierPddlName:
                        activatedActuators.Add(AirPurifierPddlName);
                        break;
                }
            }

            return activatedActuators;
        }

        private static IEnumerable<string> FindDeactivateActuatorStep(IEnumerable<string> steps,
            List<string> activateActuatorSteps)
        {
            var deactivatedActuators = new List<string>();
            foreach (var step in steps.Where(s => s.StartsWith("deactivate")))
            {
                if (activateActuatorSteps.Exists(s => s == step))
                    break;

                switch (step)
                {
                    case VentilationPddlName:
                        deactivatedActuators.Add(VentilationPddlName);
                        break;
                    case HeaterPddlName:
                        deactivatedActuators.Add(HeaterPddlName);
                        break;
                    case AirConditionerPddlName:
                        deactivatedActuators.Add(AirConditionerPddlName);
                        break;
                    case AirPurifierPddlName:
                        deactivatedActuators.Add(AirPurifierPddlName);
                        break;
                }
            }

            return deactivatedActuators;
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