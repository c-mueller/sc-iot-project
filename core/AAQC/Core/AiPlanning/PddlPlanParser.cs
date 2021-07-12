using System.Collections.Generic;
using System.Linq;
using Model.Model;
using Newtonsoft.Json;
using Serilog;

namespace Core.AiPlanning
{
    public static class PddlPlanParser
    {
        private const string VentilationPddlName = "ventilation";
        private const string HeaterPddlName = "heater";
        private const string AirConditionerPddlName = "airconditioner";
        private const string AirPurifierPddlName = "airpurifier";

        public static ActuatorState Parse(IEnumerable<PddlPlanStep> plan)
        {
            var steps = plan.Select(step => step.Name.CleanUpStep()).ToList();
            
            var activateSteps = FindActivateActuatorStep(steps).ToList();
            var deactivateSteps = FindDeactivateActuatorStep(steps, activateSteps).ToList();
            // Log.Information(JsonConvert.SerializeObject(activateSteps));
            // Log.Information(JsonConvert.SerializeObject(deactivateSteps));
            
            var actuatorState = new ActuatorState();
            foreach (var activateStep in activateSteps)
                actuatorState.DecideActuatorState(activateStep, true);

            foreach (var deactivateStep in deactivateSteps)
                actuatorState.DecideActuatorState(deactivateStep, false);
            
            // Log.Information(JsonConvert.SerializeObject(actuatorState));
            
            return actuatorState;
        }
        
        private static string CleanUpStep(this string uncleanedStep)
        {
            return uncleanedStep.Split(" ")[0].Trim('(').ToLower();
        }

        private static IEnumerable<string> FindActivateActuatorStep(IEnumerable<string> steps)
        {
            var activatedActuators = new List<string>();
            foreach (var step in steps.Where(s => s.StartsWith("activate")))
            {
                switch (step)
                {
                    case "activate" + VentilationPddlName:
                        activatedActuators.Add(VentilationPddlName);
                        break;
                    case "activate" + HeaterPddlName:
                        activatedActuators.Add(HeaterPddlName);
                        break;
                    case "activate" + AirConditionerPddlName:
                        activatedActuators.Add(AirConditionerPddlName);
                        break;
                    case "activate" + AirPurifierPddlName:
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
                if (activateActuatorSteps.Exists(s => step.EndsWith(s)))
                    break;

                switch (step)
                {
                    case "deactivate" + VentilationPddlName:
                        deactivatedActuators.Add(VentilationPddlName);
                        break;
                    case "deactivate" + HeaterPddlName:
                        deactivatedActuators.Add(HeaterPddlName);
                        break;
                    case "deactivate" + AirConditionerPddlName:
                        deactivatedActuators.Add(AirConditionerPddlName);
                        break;
                    case "deactivate" + AirPurifierPddlName:
                        deactivatedActuators.Add(AirPurifierPddlName);
                        break;
                }
            }

            return deactivatedActuators;
        }

        private static void DecideActuatorState(this ActuatorState actuatorState, string step,
            bool isActive)
        {
            switch (step)
            {
                case VentilationPddlName:
                    actuatorState.IsVentilationActive = isActive;
                    break;
                case HeaterPddlName:
                    actuatorState.IsHeaterActive = isActive;
                    break;
                case AirConditionerPddlName:
                    actuatorState.IsAirConditionerActive = isActive;
                    break;
                case AirPurifierPddlName:
                    actuatorState.IsAirPurifierActive = isActive;
                    break;
            }
        }
    }
}