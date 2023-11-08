using System.Net;
using System.Net.WebSockets;
using System.Text;
using Wing.Exceptions;
using Wing.ServiceProvider;

namespace Wing.Gateway.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly ServiceRequestDelegate _next;
        private readonly IServiceFactory _serviceFactory;
        private readonly ILogProvider _logProvider;

        public WebSocketMiddleware(ServiceRequestDelegate next,
            IServiceFactory serviceFactory,
            ILogProvider logProvider)
        {
            _next = next;
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
                await Echo(webSocket, serviceContext);
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

        private async Task Echo(WebSocket webSocket, ServiceContext serviceContext)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);
            if (receiveResult.CloseStatus.HasValue)
            {
                await webSocket.CloseAsync(
               receiveResult.CloseStatus.Value,
               receiveResult.CloseStatusDescription,
               CancellationToken.None);
                return;
            }

            var client = new ClientWebSocket();
            await _serviceFactory.InvokeAsync(serviceContext.ServiceName, Tools.RemoteIp, async serviceAddr =>
            {
                var scheme = serviceAddr.Sheme == "https" ? "wss" : "ws";
                await client.ConnectAsync(new Uri($"{scheme}://{serviceAddr.Host}:{serviceAddr.Port}{serviceContext.DownstreamPath}"), CancellationToken.None);
            });
            while (!receiveResult.CloseStatus.HasValue)
            {
                serviceContext.RequestValue = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                await client.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                var clientReceiveResult = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                serviceContext.ResponseValue = Encoding.UTF8.GetString(buffer, 0, clientReceiveResult.Count);
                if (clientReceiveResult.CloseStatus.HasValue)
                {
                    serviceContext.StatusCode = (int)clientReceiveResult.CloseStatus;
                    await _logProvider.Add(serviceContext);
                    break;
                }

                serviceContext.StatusCode = (int)HttpStatusCode.OK;
                await _logProvider.Add(serviceContext);
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
