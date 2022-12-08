using Newtonsoft.Json;
using System.Text;

namespace WebUi.Helpers.Http
{
    public class RequestHelper
    {
        public static StringContent GetStringContentFromObject(object obj)
        {
            var serialized = JsonConvert.SerializeObject(obj);
            var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
            return stringContent;
        }
    }
}
