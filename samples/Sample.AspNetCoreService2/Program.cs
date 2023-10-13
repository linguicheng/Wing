using System.Security.Claims;
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddWing().AddJwt(context =>
{
    var user = context.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
    return user == "byron";
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
