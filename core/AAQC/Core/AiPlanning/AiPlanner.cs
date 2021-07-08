using System.Collections.Generic;
using Core.Model;
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

            var latestPddlObjectState = _contextStore.GetLastPddlObjectState();
            if (newActuatorState.Equals(latestPddlObjectState.ActuatorStates))
            {
                return;
            }
            
            _contextStore.StorePddlObjectState(currentPddlObjectState);
            // TODO create Actuator Context and send
            // TODO decide if we want to send all actuator info or only changed ones
            var actuatorInfos = new List<ActuatorInfo>();
            
            var actuatorContext = new ActuatorContext
            {
                ActuatorStates = actuatorInfos,
            };
            
            _actuatorContextConsumer.Consume(actuatorContext);
        }
    }
}