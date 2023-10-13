using Sample.GrpcService;
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddGrpc(options =>
{
    options.MaxReceiveMessageSize = 1 * 1024 * 1024; // 1 MB
    options.MaxSendMessageSize = 1 * 1024 * 1024; // 1 MB
});
builder.Services.AddWing();

var app = builder.Build();
app.MapGrpcService<GreeterService>();
app.MapGrpcService<HealthCheck>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();
