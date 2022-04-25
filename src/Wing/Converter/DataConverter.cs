using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            using Stream stream = new MemoryStream(bytes);
            return new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
        }

        public static Dictionary<string, string> BytesToDictionary(byte[] bytes)
        {
            return BuildConfig(bytes)
                .AsEnumerable()
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
