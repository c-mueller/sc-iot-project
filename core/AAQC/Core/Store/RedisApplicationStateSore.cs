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
        private const string LatestPddlObjectState = "latestPddlObjectState";
        private readonly IConnectionMultiplexer _connection;

        public RedisApplicationStateSore(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }
        
        public void StoreLatestObjectState(ObjectState objectState)
        {
            var db = _connection.GetDatabase();
            var payload = JsonConvert.SerializeObject(objectState);
            db.StringSet(LatestPddlObjectState, payload);
        }

        public void StoreLatestSensorContext(SensorContext context)
        {
            var db = _connection.GetDatabase();
            var payload = JsonConvert.SerializeObject(context);
            db.StringSet(LatestContextKey, payload);
        }

        public ObjectState GetLatestObjectState()
        {
            var db = _connection.GetDatabase();
            var objectState = db.StringGet(LatestPddlObjectState);
            if (objectState.HasValue)
            {
                return JsonConvert.DeserializeObject<ObjectState>(objectState.ToString());
            }
            return Constants.InitialObjectState;
        }

        public SensorContext GetLatestSensorContext()
        {
            var db = _connection.GetDatabase();
            var context = db.StringGet(LatestContextKey);
            if (context.HasValue)
            {
                return JsonConvert.DeserializeObject<SensorContext>(context.ToString());
            }
            return new SensorContext();
        }
    }
}