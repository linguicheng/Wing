using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Wing.Injection;

namespace Wing.Converter
{
    public class JsonConverter : IJson, ISingleton
    {
        public T Deserialize<T>(string value)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return JsonConvert.DeserializeObject<T>(value, DefaultSetting());
        }

        public object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, DefaultSetting());
        }

        public T DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return JsonConvert.DeserializeAnonymousType(value, anonymousTypeObject, DefaultSetting());
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, DefaultSetting());
        }

        public JsonSerializerSettings DefaultSetting()
        {
            var setting = new JsonSerializerSettings
            {
                Formatting = Formatting.None
            };
            setting.Converters.Add(new StringEnumConverter());
            setting.ContractResolver = new LowercaseContractResolver();
            setting.NullValueHandling = NullValueHandling.Ignore;
            setting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
            setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return setting;
        }
    }
}
