using Core.Model;
using Model;
using Model.Model;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Core.SensorContextStore
{
    public class RedisSensorContextSore: ISensorContextStore
    {
        private const string LatestContextKey = "latestContext";
        private readonly IConnectionMultiplexer _connection;

        public RedisSensorContextSore(IConnectionMultiplexer connection)
        {
            _connection = connection;
        }
        
        public void StorePddlObjectState(PddlObjectState objectState)
        {
            var payload = JsonConvert.SerializeObject(objectState);
            var db = _connection.GetDatabase();
            db.StringSet(LatestContextKey, payload);
        }

        public PddlObjectState GetLastPddlObjectState()
        {
            var db = _connection.GetDatabase();
            var context = db.StringGet(LatestContextKey);
            if (context.HasValue)
            {
                return JsonConvert.DeserializeObject<PddlObjectState>(context.ToString());
            }
            return Constants.InitialPddlObjectState;
        }
    }
}