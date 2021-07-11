using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Interfaces;
using Model.Model;

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
            var currentObjectState = SensorContextEvaluator.Evaluate(currentContext);

            var currentProblem = PddlProblemParser.Parse(currentObjectState);
            if (!currentProblem.HasInitStates())
            {
                return;
            }

            var plan = _pddlSolver.CreatePlanForProblem(currentProblem);

            var newActuatorState = PddlPlanParser.Parse(plan);
            var latestActuatorState = _contextStore.GetLatestActuatorState();

            // Fill in actuator values that are not included in the plan with the value they currently have 
            newActuatorState.IsVentilationActive ??= latestActuatorState.IsVentilationActive;
            newActuatorState.IsHeaterActive ??= latestActuatorState.IsHeaterActive;
            newActuatorState.IsAirConditionerActive ??= latestActuatorState.IsAirConditionerActive;
            newActuatorState.IsAirPurifierActive ??= latestActuatorState.IsAirPurifierActive;

            var actuators = GetActuatorContexts(newActuatorState, latestActuatorState).ToList();
            if (!actuators.Any())
            {
                return;
            }

            foreach (var actuator in actuators)
            {
                _actuatorContextConsumer.Consume(actuator);
            }

            _contextStore.StoreLatestActuatorState(currentObjectState.ActuatorState);
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