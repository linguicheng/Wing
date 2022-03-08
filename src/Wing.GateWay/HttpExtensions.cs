using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wing.ServiceProvider;

namespace Wing.GateWay
{
    public static class HttpExtensions
    {
        public static HttpRequestMessage ToHttpRequestMessage(this HttpRequest req, ServiceAddress serviceAddress, string path)
        {
            var reqMsg = new HttpRequestMessage
            {
                Method = new HttpMethod(req.Method),
                RequestUri = new UriBuilder
                {
                    Scheme = serviceAddress.Sheme,
                    Host = serviceAddress.Host,
                    Port = serviceAddress.Port,
                    Path = path,
                    Query = req.QueryString.ToString()
                }.Uri,
                Content = new StreamContent(req.Body)
            };
            if (req.Headers != null && req.Headers.ContainsKey("Content-Type"))
            {
                reqMsg.Content.Headers.Add("Content-Type", req.ContentType);
            }

            return req.Headers.Aggregate(reqMsg, (acc, h) =>
             {
                 acc.Headers.TryAddWithoutValidation(h.Key, h.Value.AsEnumerable());
                 return acc;
             });
        }

        public static async Task FromHttpResponseMessage(this HttpResponse response, HttpResponseMessage reqMsg, Action<int, string> action)
        {
            var statusCode = (int)reqMsg.StatusCode;
            response.StatusCode = statusCode;
            string content = string.Empty;
            if (reqMsg.Content != null)
            {
                if (reqMsg.Content.Headers.Contains("Content-Type"))
                {
                    response.ContentType = reqMsg.Content.Headers.GetValues("Content-Type").Single();
                }

                using var stream = await reqMsg.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);
                content = await reader.ReadToEndAsync();
            }

            action(statusCode, content);
            await response.WriteAsync(content);
        }
    }
}
