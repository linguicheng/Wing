using System;
using System.Collections.Generic;

namespace Wing.ServiceProvider
{
    public class ServiceUtils
    {
        public static void GetServiceTagConfig(IEnumerable<string> tags, string startsWith, Action<string> action)
        {
            foreach (var tag in tags)
            {
                if (tag.StartsWith(startsWith))
                {
                    var tagArr = tag.Split(":");
                    if (tagArr.Length == 2)
                    {
                        action(tagArr[1]);
                        break;
                    }
                }
            }
        }

        public static IDiscoveryServiceProvider DiscoveryService { get; set; }
    }
}
