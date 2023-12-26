using Microsoft.AspNetCore.Http;
using Wing.ServiceProvider;

namespace Wing.Gateway
{
    public static class HttpExtensions
    {
        public static async Task<HttpRequestMessage> ToHttpRequestMessage(this HttpRequest req, ServiceAddress serviceAddress, string path, string method = "")
        {
            var reqMsg = new HttpRequestMessage
            {
                Method = new HttpMethod(string.IsNullOrWhiteSpace(method) ? req.Method : method),
                RequestUri = new UriBuilder
                {
                    Scheme = serviceAddress.Sheme,
                    Host = serviceAddress.Host,
                    Port = serviceAddress.Port,
                    Path = path,
                    Query = req.QueryString.ToString()
                }.Uri
            };
            if (req.Body != null)
            {
                req.EnableBuffering();
                var ms = new MemoryStream();
                await req.Body.CopyToAsync(ms);
                ms.Position = 0;
                reqMsg.Content = new StreamContent(ms);
                req.Body.Position = 0;
            }

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

                content = await reqMsg.Content.ReadAsStringAsync();
            }

            action(statusCode, content);
            await response.WriteAsync(content);
        }
    }
}
