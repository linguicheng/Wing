using System.Collections.Concurrent;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Wing.Persistence.APM;

namespace Wing.APM.Listeners
{
    public class ListenerHelper
    {
        public static readonly ConcurrentDictionary<string, TracerDto> TracerData = new ConcurrentDictionary<string, TracerDto>();

        public static T GetProperty<T>(object value, string name)
        {
            return (T)value.GetType()
                        .GetProperty(name)
                        .GetValue(value);
        }

        public static async Task<string> GetRequestValue(HttpRequestMessage reqMsg)
        {
            string content = string.Empty;
            if (reqMsg.Content != null)
            {
                using var stream = await reqMsg.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public static async Task<string> GetResponseValue(HttpResponseMessage resMsg)
        {
            string content = string.Empty;
            if (resMsg.Content != null)
            {
                using var stream = await resMsg.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }

            return content;
        }
    }
}
