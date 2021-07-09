using System.Collections.Generic;
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
            var currentPddlObjectState = SensorContextEvaluator.Evaluate(currentContext);

            var currentProblem = PddlProblemParser.Parse(currentPddlObjectState);
            if (!currentProblem.HasInitStates())
            {
                return;
            }
            
            var plan = _pddlSolver.CreatePlanForProblem(currentProblem);
            var newActuatorState = PddlPlanParser.Parse(plan);

            var latestPddlObjectState = _contextStore.GetLatestObjectState();
            if (newActuatorState.Equals(latestPddlObjectState.ActuatorStates))
            {
                return;
            }
            
            var actuatorInfos = GetActuatorInfos(newActuatorState, latestPddlObjectState.ActuatorStates);
            
            var actuatorContext = new ActuatorContext
            {
                ActuatorStates = actuatorInfos,
            };
            
            _contextStore.StoreLatestObjectState(currentPddlObjectState);
            _actuatorContextConsumer.Consume(actuatorContext);
        }

        private List<ActuatorInfo> GetActuatorInfos(ActuatorState newActuatorState,
            ActuatorState currentActuatorState)
        {
            var actuatorInfos = new List<ActuatorInfo>();
            if (newActuatorState.IsVentilationActive != currentActuatorState.IsVentilationActive)
            {
                actuatorInfos.Add(new ActuatorInfo
                {
                    Name = "ventilation",
                    Type = ActuatorType.Ventilation,
                    Active = newActuatorState.IsVentilationActive,
                    TargetValue = 0 // TODO
                });
            }
            if (newActuatorState.IsHeaterActive != currentActuatorState.IsHeaterActive)
            {
                actuatorInfos.Add(new ActuatorInfo
                {
                    Name = "heater",
                    Type = ActuatorType.Heater,
                    Active = newActuatorState.IsHeaterActive,
                    TargetValue = 0 // TODO
                });
            }
            if (newActuatorState.IsAirConditionerActive != currentActuatorState.IsAirConditionerActive)
            {
                actuatorInfos.Add(new ActuatorInfo
                {
                    Name = "air-conditioner",
                    Type = ActuatorType.AirConditioner,
                    Active = newActuatorState.IsAirConditionerActive,
                    TargetValue = 0 // TODO
                });
            }
            if (newActuatorState.IsAirPurifierActive != currentActuatorState.IsAirPurifierActive)
            {
                actuatorInfos.Add(new ActuatorInfo
                {
                    Name = "air-purifier",
                    Type = ActuatorType.AirPurifier,
                    Active = newActuatorState.IsAirPurifierActive,
                    TargetValue = 0 // TODO
                });
            }

            return actuatorInfos;
        }
    }
}