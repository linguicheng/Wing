using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
    }
}
