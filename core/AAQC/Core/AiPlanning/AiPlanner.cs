using Core.Model;
using Model.Interfaces;
using Model.Model;

namespace Core.AiPlanning
{
    public class AiPlanner
    {
        private IActuatorContextConsumer _actuatorContextConsumer;
        private readonly ISensorContextStore _contextStore;

        public AiPlanner(IActuatorContextConsumer actuatorContextConsumer, ISensorContextStore contextStore)
        {
            _actuatorContextConsumer = actuatorContextConsumer;
            _contextStore = contextStore;
        }

        public void Initiate(SensorContext currentContext)
        {
            // Evaluate if there are any changes
            var latestPddlObjectState = _contextStore.GetLastPddlObjectState();
            var currentPddlObjectState = SensorContextEvaluator.Evaluate(currentContext);
            
        }
    }
}