using System.Collections.Generic;
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

            var latestObjectState = _contextStore.GetLatestObjectState();
            if (newActuatorState.Equals(latestObjectState.ActuatorStates))
            {
                return;
            }
            
            var actuators = GetActuatorInfos(newActuatorState, latestObjectState.ActuatorStates);

            foreach (var actuator in actuators)
            {
                _actuatorContextConsumer.Consume(actuator);
            }
            
            _contextStore.StoreLatestObjectState(currentObjectState);
        }

        private List<ActuatorContext> GetActuatorInfos(ActuatorState newActuatorState,
            ActuatorState currentActuatorState)
        {
            var actuatorInfos = new List<ActuatorContext>();
            if (newActuatorState.IsVentilationActive != currentActuatorState.IsVentilationActive)
            {
                actuatorInfos.Add(new ActuatorContext
                {
                    Name = Constants.VentilationName,
                    Type = ActuatorType.Ventilation,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsVentilationActive,
                        TargetValue = 0 // TODO
                    }
                });
            }
            if (newActuatorState.IsHeaterActive != currentActuatorState.IsHeaterActive)
            {
                actuatorInfos.Add(new ActuatorContext
                {
                    Name = Constants.HeaterName,
                    Type = ActuatorType.Heater,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsHeaterActive,
                        TargetValue = 0 // TODO
                    }
                });
            }
            if (newActuatorState.IsAirConditionerActive != currentActuatorState.IsAirConditionerActive)
            {
                actuatorInfos.Add(new ActuatorContext
                {
                    Name = Constants.AirConditionerName,
                    Type = ActuatorType.AirConditioner,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsAirConditionerActive,
                        TargetValue = 0 // TODO
                    }
                });
            }
            if (newActuatorState.IsAirPurifierActive != currentActuatorState.IsAirPurifierActive)
            {
                actuatorInfos.Add(new ActuatorContext
                {
                    Name = Constants.AirPurifierName,
                    Type = ActuatorType.AirPurifier,
                    ActuatorInfo = new ActuatorInfo
                    {
                        Active = newActuatorState.IsAirPurifierActive,
                        TargetValue = 0 // TODO
                    }
                });
            }

            return actuatorInfos;
        }
    }
}