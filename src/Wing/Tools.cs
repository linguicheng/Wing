using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Http;

namespace Wing
{
    public class Tools
    {
        public static string LocalIp
        {
            get
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                .Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .Where(p => p.PrefixOrigin == PrefixOrigin.Dhcp || p.PrefixOrigin == PrefixOrigin.Manual)
                .OrderByDescending(p => p.PrefixOrigin)
                .FirstOrDefault(p => p.IsDnsEligible && p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))?.Address.ToString();
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
    }
}
