using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.WebSockets;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.Gateway.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogProvider _logProvider;

        public WebSocketMiddleware(ServiceRequestDelegate next,
            IHttpClientFactory clientFactory,
            IServiceFactory serviceFactory,
            ILogProvider logProvider)
        {
            _next = next;
            _clientFactory = clientFactory;
            _serviceFactory = serviceFactory;
            _logProvider = logProvider;
        }

        public async Task InvokeAsync(ServiceContext serviceContext)
        {
            if (string.IsNullOrWhiteSpace(serviceContext.ServiceName) || !serviceContext.IsWebSocket)
            {
                await _next(serviceContext);
                return;
            }

            var context = serviceContext.HttpContext;

            try
            {
                using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await Echo(webSocket);


                var resMsg = await _serviceFactory.InvokeAsync(serviceContext.ServiceName, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    var reqMsg = await context.Request.ToHttpRequestMessage(serviceAddr, serviceContext.DownstreamPath);
                    var client = _clientFactory.CreateClient(serviceContext.ServiceName);
                    return await client.SendAsync(reqMsg);
                });
                await context.Response.FromHttpResponseMessage(resMsg, (statusCode, content) =>
                {
                    serviceContext.StatusCode = statusCode;
                    serviceContext.ResponseValue = content;
                    _logProvider.Add(serviceContext);
                });
            }
            catch (ServiceNotFoundException)
            {
                serviceContext.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.StatusCode = serviceContext.StatusCode;
                await _logProvider.Add(serviceContext);
            }
            catch
            {
                serviceContext.StatusCode = (int)HttpStatusCode.BadGateway;
                context.Response.StatusCode = serviceContext.StatusCode;
                await _logProvider.Add(serviceContext);
            }
        }


        private static async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
