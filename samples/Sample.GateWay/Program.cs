using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddWing()
                    .AddJwt()
                    .AddGateWay((urls,context) =>
                    {
                        var dd = urls;
                        return Task.FromResult(true);
                    }, new WebSocketOptions
                    {
                        KeepAliveInterval = TimeSpan.FromMinutes(2)
                    });
//.AddEventBus();    

var app = builder.Build();
app.Run();


