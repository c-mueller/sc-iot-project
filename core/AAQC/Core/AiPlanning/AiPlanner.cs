using Core.Model;
using Model.Interfaces;
using Model.Model;

namespace Core.AiPlanning
{
    public class AiPlanner
    {
        private IActuatorContextConsumer _actuatorContextConsumer;
        private PddlEvaluator _evaluator;
        private readonly ISensorContextStore _contextStore;

        public AiPlanner(IActuatorContextConsumer actuatorContextConsumer, PddlEvaluator evaluator, ISensorContextStore contextStore)
        {
            _actuatorContextConsumer = actuatorContextConsumer;
            _evaluator = evaluator;
            _contextStore = contextStore;
        }

        public void Initiate(SensorContext sensorContext)
        {
            // Evaluate if there are any changes
            var latestContext = _contextStore.GetLastContext();
            if (latestContext.Equals(new SensorContext()))
            {
                // TODO set latestContext to init context
            }
            
            
        }
    }
}