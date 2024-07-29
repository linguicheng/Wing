using System.Collections.Concurrent;
using Wing.APM.Persistence;

namespace Wing.APM.Listeners
{
    public class ListenerTracer
    {
        public static readonly ConcurrentDictionary<string, TracerDto> Data = new ConcurrentDictionary<string, TracerDto>();

        public static void Remove(List<KeyValuePair<string, TracerDto>> trancers)
        {
            foreach (var item in trancers)
            {
                Data.TryRemove(item.Key, out _);
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
