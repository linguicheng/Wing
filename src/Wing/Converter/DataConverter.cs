using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Wing.Converter
{
    public class DataConverter
    {
        public static string BytesToString(byte[] value)
        {
            return value != null && value.Length > 0 ? Encoding.UTF8.GetString(value) : string.Empty;
        }

        public static byte[] StringToBytes(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : Encoding.UTF8.GetBytes(value);
        }

        public static IConfigurationRoot BuildConfig(byte[] bytes)
        {
            using (Stream stream = new MemoryStream(bytes))
            {
                return new ConfigurationBuilder()
               .AddJsonStream(stream)
               .Build();
            }
        }

        public static Dictionary<string, string> BytesToDictionary(byte[] bytes)
        {
            return BuildConfig(bytes)
                .AsEnumerable()
                .ToDictionary(p => p.Key, p => p.Value);
        }

        public static byte[] ObjectToBytes(object obj)
        {
           return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public static object BytesToObject(byte[] bytes, Type type)
        {
            using (MemoryStream ms = new (bytes))
            {
                return JsonSerializer.Deserialize(ms, type);
            }
        }
    }
}
