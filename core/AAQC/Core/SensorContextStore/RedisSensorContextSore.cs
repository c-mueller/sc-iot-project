using Core.Model;
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
        
        public void StoreContext(SensorContext sensorContext)
        {
            var payload = JsonConvert.SerializeObject(sensorContext);
            var db = _connection.GetDatabase();
            db.StringSet(LatestContextKey, payload);
        }

        public SensorContext GetLastContext()
        {
            var db = _connection.GetDatabase();
            var context = db.StringGet(LatestContextKey);
            return context.HasValue ? JsonConvert.DeserializeObject<SensorContext>(context.ToString()): new SensorContext();
        }
    }
}