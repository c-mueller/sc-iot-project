using Newtonsoft.Json;

namespace MessagingEndpoint
{
    public static class Util
    {
        public static T DeepCopy<T>(this T element)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(element));
        }
    }
}