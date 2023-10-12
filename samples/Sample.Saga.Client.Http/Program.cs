using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddWing().AddSaga().AddJwt().AddEventBus();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
