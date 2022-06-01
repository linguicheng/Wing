using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Wing.Persistence.APM;

namespace Wing.APM.Listeners
{
    public class ListenerTracer
    {
        public static readonly List<TracerDto> Data = new List<TracerDto>();

        public TracerDto this[string traceId] => Data.Single(x => x.Tracer.Id == traceId);

        public static void Remove(List<TracerDto> tracers)
        {
            foreach (var tracer in tracers)
            {
                Data.Remove(tracer);
            }
        }

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
                content = await reqMsg.Content.ReadAsStringAsync();
            }

            return content;
        }

        public static async Task<string> GetResponseValue(HttpResponseMessage resMsg)
        {
            string content = string.Empty;
            if (resMsg.Content != null)
            {
                content = await resMsg.Content.ReadAsStringAsync();
            }

            return content;
        }
    }
}
