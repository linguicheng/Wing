using Sample.Auth;
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddWing()
                    .AddJwt()
                    .AddPersistence()
                    .AddGateWay(new WebSocketOptions
                    {
                        KeepAliveInterval = TimeSpan.FromMinutes(2)
                    });
//.AddEventBus();    

var app = builder.Build();
app.Run();


