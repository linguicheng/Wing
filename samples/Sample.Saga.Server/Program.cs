using Sample.Auth;
using Wing;
using Wing.Saga.Server;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

builder.Services.AddWing()
                .AddJwt()
                .AddPersistence()
                .AddSaga(new SagaOptions
                     {
                         Headers = () =>
                         {
                             var token = $"Bearer {App.GetRequiredService<IAuth>().GetToken()}";
                             return new Dictionary<string, string> { { "Authorization", token } };
                         }
                     });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
