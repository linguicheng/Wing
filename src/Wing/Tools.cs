using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Wing
{
    public class Tools
    {
        private static readonly object _lock = new();

        private static MD5 _md5 = null;

        public static string LocalIp
        {
            get
            {
                try
                {
                    return NetworkInterface.GetAllNetworkInterfaces()
                    .Select(p => p.GetIPProperties())
                    .SelectMany(p => p.UnicastAddresses)
                    .Where(p => p.PrefixOrigin == PrefixOrigin.Dhcp || p.PrefixOrigin == PrefixOrigin.Manual)
                    .OrderByDescending(p => p.PrefixOrigin)
                    .FirstOrDefault(p => p.IsDnsEligible && p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))?.Address.ToString();
                }
                catch (Exception ex)
                {
                    App.GetRequiredService<ILogger>().LogError(ex, "获取本机IP异常");
                    return string.Empty;
                }
            }
        }

        public static string RemoteIp
        {
            get
            {
                var httpContextAccessor = App.GetService<IHttpContextAccessor>();
                var context = httpContextAccessor?.HttpContext;
                var ip = context?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                return string.IsNullOrWhiteSpace(ip) ? context.Connection.RemoteIpAddress?.MapToIPv4()?.ToString() : ip;
            }
        }

        public static TimeSpan DueTime(string startTime, TimeSpan period)
        {
            var now = TimeSpan.Parse(DateTime.Now.TimeOfDay.ToString(@"hh\:mm"));
            string[] formats = { @"hh\:mm\:ss", @"hh\:mm" };
            if (TimeSpan.TryParseExact(startTime, formats, CultureInfo.InvariantCulture, out TimeSpan executeTime))
            {
                return executeTime >= now ? executeTime - now : period - now + executeTime;
            }

            throw new Exception("时间格式设置错误，支持格式：hh:mm:ss或hh:mm");
        }

        public static long GetHashCode(string key)
        {
            if (_md5 == null)
            {
                lock (_lock)
                {
                    _md5 ??= MD5.Create();
                }
            }

            byte[] results = _md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            long hash = 0;
            for (int i = 0; i < 4; i++)
            {
                hash <<= 8;
                hash |= results[i];
            }

            return hash;
        }
    }
}
