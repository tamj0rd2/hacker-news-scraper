using Newtonsoft.Json;

namespace HackerNewsScraper.Wrappers
{
    public class JsonWrapper
    {
        public virtual string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }
    }
}
