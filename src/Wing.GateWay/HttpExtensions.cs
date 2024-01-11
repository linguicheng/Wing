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
            var client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(serviceContext.ServiceAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            if (req.Headers != null && req.Headers.Count > 0)
            {
                foreach (var header in req.Headers)
                {
                    var key = header.Key.ToLower();
                    if (Tag.DO_NOT_TRANSFORM_HEADERS.Any(x => x == key))
                    {
                        continue;
                    }

                    client.DefaultRequestHeaders.Add(header.Key, header.Value.AsEnumerable());
                }
            }

            List<Header> transformHeaders = [];
            if (HEADERS_TRANSFORM != null && HEADERS_TRANSFORM.Request != null)
            {
                if (HEADERS_TRANSFORM.Request.Services != null)
                {
                    var downstreamService = HEADERS_TRANSFORM.Request.Services.Where(x => x.ServiceName == serviceContext.ServiceName).FirstOrDefault();
                    if (downstreamService != null
                        && downstreamService.Headers != null
                        && downstreamService.Headers.Count > 0)
                    {
                        transformHeaders.AddRange(downstreamService.Headers);
                    }
                }

                if (HEADERS_TRANSFORM.Request.Global != null && HEADERS_TRANSFORM.Request.Global.Count > 0)
                {
                    foreach (var header in HEADERS_TRANSFORM.Request.Global)
                    {
                        if (!transformHeaders.Any(x => x.Name == header.Name))
                        {
                            transformHeaders.Add(header);
                        }
                    }
                }

                if (transformHeaders.Count > 0)
                {
                    foreach (var header in transformHeaders)
                    {
                        client.DefaultRequestHeaders.Where(x=>x.Key)
                    }
                }
            }

            if (!serviceContext.IsReadRequestBody && req.Body != null)
            {
                serviceContext.IsReadRequestBody = true;
                using (var reader = new StreamReader(req.Body))
                {
                    serviceContext.RequestValue = await reader.ReadToEndAsync();
                }
            }

            var content = new StringContent(serviceContext.RequestValue, Encoding.UTF8, "application/json");
            HttpResponseMessage response = method switch
            {
                "get" => await client.GetAsync(serviceContext.DownstreamPath),
                "post" => await client.PostAsync(serviceContext.DownstreamPath, content),
                "put" => await client.PutAsync(serviceContext.DownstreamPath, content),
                "delete" => await client.DeleteAsync(serviceContext.DownstreamPath),
                _ => throw new Exception($"网关不支持该请求方式：{method} 的转发！"),
            };
            serviceContext.StatusCode = (int)response.StatusCode;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                serviceContext.ResponseValue = await response.Content.ReadAsStringAsync();
            }

            return serviceContext;
        }

        public static async Task Response(this HttpResponse response, ServiceContext serviceContext)
        {
            response.StatusCode = serviceContext.StatusCode;
            if (!string.IsNullOrWhiteSpace(serviceContext.ResponseValue))
            {
                response.ContentType = "text/plain; charset=utf-8";
                await response.WriteAsync(serviceContext.ResponseValue);
            }
        }
    }
}
