using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;
using Model.Model;
using Newtonsoft.Json;
using Serilog;

namespace Core.AiPlanning
{
    public class AiPlanner
    {
        private readonly IActuatorContextConsumer _actuatorContextConsumer;
        private readonly IApplicationStateStore _contextStore;
        private readonly IExternalPddlSolver _pddlSolver;

        public AiPlanner(IActuatorContextConsumer actuatorContextConsumer, IApplicationStateStore contextStore,
            IExternalPddlSolver pddlSolver)
        {
            _actuatorContextConsumer = actuatorContextConsumer;
            _contextStore = contextStore;
            _pddlSolver = pddlSolver;
        }

        public void Initiate(SensorContext currentContext)
        {
            // Log.Information(JsonConvert.SerializeObject(currentContext));
            var currentObjectState = SensorContextEvaluator.Evaluate(currentContext);
            // Log.Information(JsonConvert.SerializeObject(currentObjectState));

            var latestActuatorState = _contextStore.GetLatestActuatorState();
            currentObjectState.ActuatorState = latestActuatorState;

            var currentProblem = PddlProblemParser.Parse(currentObjectState);
            if (!currentProblem.HasInitStates())
            {
                Log.Information("[AI Planner] Planning finished: No Init states for pddl problem found");
                return;
            }
            // Log.Information(currentProblem.BuildProblemFile());

            var plan = _pddlSolver.CreatePlanForProblem(currentProblem);
            if (plan == null)
            {
                Log.Error(
                    "[AI Planner] Planning finished: Error creating a plan for the given problem init states \n'{InitStates}'",
                    currentProblem.GetInitStatesAsString());
                return;
            }
            if (!plan.Any())
            {
                Log.Information("[AI Planner] Planning finished: Plan is empty and no changes are required");
                return;
            }
            // Log.Information(JsonConvert.SerializeObject(plan));

            Log.Information("[AI Planner] Finding changes in actuator state");
            var newActuatorState = PddlPlanParser.Parse(plan);

            // Fill in actuator values that are not included in the plan with the value they currently have 
            /*
            newActuatorState.IsVentilationActive ??= latestActuatorState.IsVentilationActive;
            newActuatorState.IsHeaterActive ??= latestActuatorState.IsHeaterActive;
            newActuatorState.IsAirConditionerActive ??= latestActuatorState.IsAirConditionerActive;
            newActuatorState.IsAirPurifierActive ??= latestActuatorState.IsAirPurifierActive;
            */

            var actuators = GetActuatorContexts(newActuatorState, latestActuatorState).ToList();
            if (!actuators.Any())
            {
                Log.Information("[AI Planner] Planning finished: No changes in actuator state found");
                return;
            }

            foreach (var actuator in actuators)
            {
                _actuatorContextConsumer.Consume(actuator);
            }

            _contextStore.StoreLatestActuatorState(newActuatorState);
            Log.Information("[AI Planner] Planning finished: Changes in actuator state found");
        }

        private static IEnumerable<ActuatorContext> GetActuatorContexts(ActuatorState newActuatorState,
            ActuatorState latestActuatorState)
        {
            if (newActuatorState.Equals(latestActuatorState))
            {
                return new List<ActuatorContext>();
            }

            var actuators = new List<ActuatorContext>();
            if (newActuatorState.IsVentilationActive != latestActuatorState.IsVentilationActive)
            {
                actuators.Add(new ActuatorContext
                {
                    Name = Constants.VentilationName,
                    Type = ActuatorType.Ventilation,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsVentilationActive.GetValueOrDefault(),
                        TargetValue = 0 // TODO
                    }
                });
            }

            if (newActuatorState.IsHeaterActive != latestActuatorState.IsHeaterActive)
            {
                actuators.Add(new ActuatorContext
                {
                    Name = Constants.HeaterName,
                    Type = ActuatorType.Heater,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsHeaterActive.GetValueOrDefault(),
                        TargetValue = 0 // TODO
                    }
                });
            }

            if (newActuatorState.IsAirConditionerActive != latestActuatorState.IsAirConditionerActive)
            {
                actuators.Add(new ActuatorContext
                {
                    Name = Constants.AirConditionerName,
                    Type = ActuatorType.AirConditioner,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsAirConditionerActive.GetValueOrDefault(),
                        TargetValue = 0 // TODO
                    }
                });
            }

            if (newActuatorState.IsAirPurifierActive != latestActuatorState.IsAirPurifierActive)
            {
                actuators.Add(new ActuatorContext
                {
                    Name = Constants.AirPurifierName,
                    Type = ActuatorType.AirPurifier,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsAirPurifierActive.GetValueOrDefault(),
                        TargetValue = 0 // TODO
                    }
                });
            }

            return actuators;
        }
    }
}