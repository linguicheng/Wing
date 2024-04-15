using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Wing.Gateway.Config;

namespace Wing.Gateway
{
    public static class HttpExtensions
    {
        public static readonly HeadersTransform HEADERS_TRANSFORM = App.GetConfig<HeadersTransform>("Gateway:HeadersTransform");

        public static async Task<ServiceContext> Request(this HttpRequest req, ServiceContext serviceContext)
        {
            string method = serviceContext.Method;
            if (string.IsNullOrWhiteSpace(method))
            {
                method = req.Method;
            }

            method = method.ToLower();

            var httpClientFactory = App.GetRequiredService<IHttpClientFactory>();
            var aa = req.Headers.Accept;
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(serviceContext.ServiceAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("X-Real-IP", Tools.RemoteIp);
            client.DefaultRequestHeaders.Add("X-Forwarded-For", Tools.RemoteIp);
            if (!string.IsNullOrWhiteSpace(req.Headers.Accept))
            {
                foreach (var accepts in req.Headers.Accept)
                {
                    var accptArr = accepts.Split(';', ',', '|');
                    foreach (var accept in accptArr)
                    {
                        if (MediaTypeWithQualityHeaderValue.TryParse(accept, out var parsedValue))
                        {
                            client.DefaultRequestHeaders.Accept.Add(parsedValue);
                        }
                    }
                }
            }

            if (req.Headers != null && req.Headers.Count > 0)
            {
                var donotTransformHeaders = App.GetConfig<List<string>>("Gateway:DoNotTransformHeaders");
                if (donotTransformHeaders == null)
                {
                    donotTransformHeaders = new List<string>();
                }

                donotTransformHeaders.AddRange(Tag.DO_NOT_TRANSFORM_HEADERS);
                foreach (var header in req.Headers)
                {
                    var key = header.Key.ToLower();
                    if (donotTransformHeaders.Any(x => x == key))
                    {
                        continue;
                    }

                    client.DefaultRequestHeaders.Add(header.Key, header.Value.AsEnumerable());
                }
            }

            #region 配置请求头转发到下游服务
            if (HEADERS_TRANSFORM != null && HEADERS_TRANSFORM.Request != null)
            {
                Dictionary<string, string> transformHeaders = new();
                if (HEADERS_TRANSFORM.Request.Services != null)
                {
                    var downstreamService = HEADERS_TRANSFORM.Request.Services.Where(x => x.ServiceName == serviceContext.ServiceName).FirstOrDefault();
                    if (downstreamService != null
                        && downstreamService.Headers != null
                        && downstreamService.Headers.Count > 0)
                    {
                        transformHeaders = downstreamService.Headers;
                    }
                }

                if (HEADERS_TRANSFORM.Request.Global != null && HEADERS_TRANSFORM.Request.Global.Count > 0)
                {
                    foreach (var header in HEADERS_TRANSFORM.Request.Global)
                    {
                        if (!transformHeaders.ContainsKey(header.Key))
                        {
                            transformHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                if (transformHeaders.Count > 0)
                {
                    foreach (var header in transformHeaders)
                    {
                        if (client.DefaultRequestHeaders.Any(x => x.Key == header.Key))
                        {
                            client.DefaultRequestHeaders.Remove(header.Key);
                        }

                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }
            #endregion

            ByteArrayContent content = null;
            if (!serviceContext.IsReadRequestBody && req.Body != null)
            {
                serviceContext.IsReadRequestBody = true;
                using MemoryStream ms = new();
                await req.Body.CopyToAsync(ms);
                byte[] data = ms.ToArray();
                content = new ByteArrayContent(data);
                content.Headers.Add("Content-Type", req.ContentType ?? "application/json; charset=utf-8");
                serviceContext.RequestValue = Encoding.UTF8.GetString(data);
            }

            var requestUri = serviceContext.DownstreamPath + req.QueryString.Value;
            var request = new HttpRequestMessage(new HttpMethod(method), requestUri)
            {
                Content = content
            };
            var response = await client.SendAsync(request);
            serviceContext.StatusCode = (int)response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                serviceContext.ContentType = response.Content.Headers.ContentType?.ToString();
                if (serviceContext.ContentType.Contains("application/json") || serviceContext.ContentType.Contains("text/plain"))
                {
                    serviceContext.ResponseValue = await response.Content.ReadAsStringAsync();
                    serviceContext.IsFile = false;
                    return serviceContext;
                }

                serviceContext.ResponseStream = await response.Content.ReadAsStreamAsync();
                serviceContext.IsFile = true;
            }

            return serviceContext;
        }

        public static async Task Response(this HttpResponse response, ServiceContext serviceContext)
        {
            response.StatusCode = serviceContext.StatusCode;
            #region 配置响应头返回客户端
            if (HEADERS_TRANSFORM != null && HEADERS_TRANSFORM.Response != null)
            {
                Dictionary<string, string> transformHeaders = new();
                if (HEADERS_TRANSFORM.Response.Services != null)
                {
                    var downstreamService = HEADERS_TRANSFORM.Response.Services.Where(x => x.ServiceName == serviceContext.ServiceName).FirstOrDefault();
                    if (downstreamService != null
                        && downstreamService.Headers != null
                        && downstreamService.Headers.Count > 0)
                    {
                        transformHeaders = downstreamService.Headers;
                    }
                }

                if (HEADERS_TRANSFORM.Response.Global != null && HEADERS_TRANSFORM.Response.Global.Count > 0)
                {
                    foreach (var header in HEADERS_TRANSFORM.Response.Global)
                    {
                        if (!transformHeaders.ContainsKey(header.Key))
                        {
                            transformHeaders.Add(header.Key, header.Value);
                        }
                    }
                }

                if (transformHeaders.Count > 0)
                {
                    foreach (var header in transformHeaders)
                    {
                        if (response.Headers.Any(x => x.Key == header.Key))
                        {
                            response.Headers.Remove(header.Key);
                        }

                        response.Headers.Add(header.Key, header.Value);
                    }
                }
            }
            #endregion
            if (serviceContext.IsFile)
            {
                response.ContentType = serviceContext.ContentType;
                var bytes = new byte[4096];
                int len = 0;
                while ((len = serviceContext.ResponseStream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    await response.Body.WriteAsync(bytes, 0, len);
                }

                serviceContext.ResponseStream?.Dispose();
                return;
            }

            if (!string.IsNullOrWhiteSpace(serviceContext.ResponseValue))
            {
                response.ContentType = serviceContext.ContentType ?? "text/plain; charset=utf-8";
                await response.WriteAsync(serviceContext.ResponseValue);
            }
        }
    }
}
