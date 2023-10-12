using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddWing()
                    //.AddJwt()
                    .AddPersistence()
                    .AddGateWay();
                   //.AddEventBus();    

var app = builder.Build();

app.Run();
