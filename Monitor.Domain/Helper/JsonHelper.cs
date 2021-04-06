using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Monitor.Domain.Helper
{
    public class JsonHelper
    {
        public static JToken GetJToken(object statistics)
        {
            return JToken.FromObject(statistics, 
                new JsonSerializer() { DateFormatString = "dd/MM/yyyy HH:mm:ss" });
        }
    }
}