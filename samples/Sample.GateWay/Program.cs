using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddWing()                
                .AddGateWay((downstreams,context) =>
                {
                    //此处添加业务授权逻辑，也可以将payload解析的内容通过请求头转发到下游服务context.Request.Headers.Add()
                    return Task.FromResult(true);
                }, new WebSocketOptions
                {
                    KeepAliveInterval = TimeSpan.FromMinutes(2)
                });
//.AddEventBus();    

var app = builder.Build();
app.Run();


