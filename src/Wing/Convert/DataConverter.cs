using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Wing.Convert
{
    public class DataConverter
    {
        public static string BytesToString(byte[] bytes)
        {
            return bytes != null && bytes.Length > 0 ? Encoding.UTF8.GetString(bytes) : string.Empty;
        }

        public static Dictionary<string, string> BytesToDictionary(byte[] bytes)
        {
            using Stream stream = new MemoryStream(bytes);
            return new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build()
                .AsEnumerable()
                .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
