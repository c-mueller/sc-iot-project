using Model;
using Model.Interfaces;
using Model.Model;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.Store
{
    public class RedisApplicationStateSore: IApplicationStateStore
    {
        private const string LatestContextKey = "latestContext";
        private const string LatestActuatorState = "latestActuatorState";
        private readonly IConnectionMultiplexer _connection;

        public RedisApplicationStateSore(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }
        
        public void StoreLatestActuatorState(ActuatorState actuatorState)
        {
            var db = _connection.GetDatabase();
            var payload = JsonConvert.SerializeObject(actuatorState);
            db.StringSet(LatestActuatorState, payload);
        }

        public void StoreLatestSensorContext(SensorContext context)
        {
            var db = _connection.GetDatabase();
            var payload = JsonConvert.SerializeObject(context);
            db.StringSet(LatestContextKey, payload);
        }

        public ActuatorState GetLatestActuatorState()
        {
            var db = _connection.GetDatabase();
            var objectState = db.StringGet(LatestActuatorState);
            
            return objectState.HasValue ? JsonConvert.DeserializeObject<ActuatorState>(objectState.ToString()) : Constants.InitialObjectState.ActuatorState;
        }

        public SensorContext GetLatestSensorContext()
        {
            var db = _connection.GetDatabase();
            var context = db.StringGet(LatestContextKey);
            
            return context.HasValue ? JsonConvert.DeserializeObject<SensorContext>(context.ToString()) : new SensorContext();
        }
    }
}