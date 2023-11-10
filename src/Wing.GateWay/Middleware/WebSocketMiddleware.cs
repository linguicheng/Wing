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
            WebSocket webSocket = null;
            WebSocketReceiveResult receiveResult = null;
            ClientWebSocket client = null;
            WebSocketReceiveResult clientReceiveResult = null;
            WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure;
            try
            {
                webSocket = await context.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];
                receiveResult = await webSocket.ReceiveAsync(
                   new ArraySegment<byte>(buffer), CancellationToken.None);
                if (receiveResult.CloseStatus.HasValue)
                {
                    return;
                }

                client = new ClientWebSocket();
                await _serviceFactory.InvokeAsync(serviceContext.ServiceName, Tools.RemoteIp, async serviceAddr =>
                {
                    serviceContext.ServiceAddress = serviceAddr.ToString();
                    var scheme = serviceAddr.Sheme == "https" ? "wss" : "ws";
                    await client.ConnectAsync(new Uri($"{scheme}://{serviceAddr.Host}:{serviceAddr.Port}{serviceContext.DownstreamPath}"), CancellationToken.None);
                });
                while (!receiveResult.CloseStatus.HasValue)
                {
                    serviceContext.RequestTime = DateTime.Now;
                    serviceContext.RequestValue = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    await client.SendAsync(new ArraySegment<byte>(buffer, 0, receiveResult.Count),
                        receiveResult.MessageType,
                        receiveResult.EndOfMessage,
                        CancellationToken.None);

                    clientReceiveResult = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
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
                        new ArraySegment<byte>(buffer, 0, clientReceiveResult.Count),
                        clientReceiveResult.MessageType,
                        clientReceiveResult.EndOfMessage,
                        CancellationToken.None);

                    receiveResult = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), CancellationToken.None);
                }
            }
            catch (ServiceNotFoundException ex)
            {
                closeStatus = WebSocketCloseStatus.EndpointUnavailable;
                serviceContext.StatusCode = (int)closeStatus;
                serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
                await _logProvider.Add(serviceContext);
            }
            catch (Exception ex)
            {
                closeStatus = WebSocketCloseStatus.InternalServerError;
                serviceContext.StatusCode = (int)closeStatus;
                serviceContext.Exception = $"{ex.Message} {ex.StackTrace}";
                await _logProvider.Add(serviceContext);
            }
            finally
            {
                if (receiveResult.CloseStatus.HasValue)
                {
                    closeStatus = receiveResult.CloseStatus.Value;
                }

                if (client != null)
                {
                    if (client.State != WebSocketState.Aborted
                        && client.State != WebSocketState.Closed
                        && client.State != WebSocketState.None)
                    {
                        await client.CloseAsync(closeStatus, receiveResult.CloseStatusDescription, CancellationToken.None);
                    }

                    client.Dispose();
                }

                if (webSocket != null)
                {
                    if (webSocket.State != WebSocketState.Aborted 
                        && webSocket.State != WebSocketState.Closed
                        && webSocket.State != WebSocketState.None)
                    {
                        await webSocket.CloseAsync(closeStatus, receiveResult.CloseStatusDescription, CancellationToken.None);
                    }

                    webSocket.Dispose();
                }
            }
        }
    }
}
