using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
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
            var client = httpClientFactory.CreateClient();
            if (serviceContext.Policy != null && serviceContext.Policy.HttpClientTimeOut != null)
            {
                client.Timeout = TimeSpan.FromMilliseconds(serviceContext.Policy.HttpClientTimeOut.Value);
            }

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
            RequestData requestData = new()
            {
                ServiceName = serviceContext.ServiceName,
                DownstreamPath = serviceContext.DownstreamPath
            };
            if (!serviceContext.IsReadRequestBody && req.Body != null)
            {
                serviceContext.IsReadRequestBody = true;
                using MemoryStream ms = new();
                await req.Body.CopyToAsync(ms);
                requestData.Body = ms.ToArray();
            }

            var queryString = req.QueryString.Value;
            var requestUri = serviceContext.DownstreamPath + queryString;
            if (serviceContext.RequestBefore != null)
            {
                requestData.Headers = new Dictionary<string, IEnumerable<string>>();
                foreach (var header in client.DefaultRequestHeaders)
                {
                    requestData.Headers.Add(header.Key, header.Value);
                }

                if (!string.IsNullOrEmpty(queryString))
                {
                    requestData.QueryParams = QueryHelpers.ParseQuery(queryString);
                }

                requestData = await serviceContext.RequestBefore(requestData);
                foreach (var header in requestData.Headers)
                {
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                requestUri = QueryHelpers.AddQueryString(serviceContext.DownstreamPath, requestData.QueryParams);
            }

            if (requestData.Body != null && requestData.Body.Length > 0)
            {
                content = new ByteArrayContent(requestData.Body);
                content.Headers.Add("Content-Type", req.ContentType ?? "application/json; charset=utf-8");
                serviceContext.RequestValue = Encoding.UTF8.GetString(requestData.Body);
            }

            var request = new HttpRequestMessage(new HttpMethod(method), requestUri)
            {
                Content = content
            };
            var response = await client.SendAsync(request);
            serviceContext.StatusCode = (int)response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Headers != null)
                {
                    var doNotTransformResponseHeaders = App.GetConfig<List<string>>("Gateway:DoNotTransformResponseHeaders");
                    if (doNotTransformResponseHeaders == null)
                    {
                        doNotTransformResponseHeaders = new List<string>();
                    }

                    doNotTransformResponseHeaders.AddRange(Tag.DO_NOT_TRANSFORM_RESPONSE_HEADERS);
                    serviceContext.ResponseHeaders = new Dictionary<string, StringValues>();
                    foreach (var header in response.Headers)
                    {
                        if (doNotTransformResponseHeaders.Any(x => x == header.Key))
                        {
                            continue;
                        }

                        serviceContext.ResponseHeaders.Add(header.Key, new StringValues(header.Value.ToArray()));
                    }
                }

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

            if (serviceContext.ResponseHeaders != null)
            {
                foreach (var header in serviceContext.ResponseHeaders)
                {
                    if (!response.Headers.Any(x => x.Key == header.Key))
                    {
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
                if (serviceContext.ResponseAfter != null)
                {
                    ResponseData responseData = new()
                    {
                        ServiceName = serviceContext.ServiceName,
                        DownstreamPath = serviceContext.DownstreamPath,
                        Body = serviceContext.ResponseValue
                    };

                    foreach (var header in response.Headers)
                    {
                        responseData.Headers.Add(header.Key, header.Value);
                    }

                    responseData = await serviceContext.ResponseAfter(responseData);
                    serviceContext.ResponseValue = responseData.Body;
                    foreach (var header in responseData.Headers)
                    {
                        response.Headers.Clear();
                        response.Headers.Add(header.Key, new StringValues(header.Value.ToArray()));
                    }
                }

                await response.WriteAsync(serviceContext.ResponseValue);
            }
        }
    }
}
