using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddWing()
                 .AddWingUI()
                 .AddPersistence()
                 .AddAPM();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

// 设置允许所有来源跨域
app.UseCors(options =>
{
    options.AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(x => true)
    .AllowCredentials();
});

app.MapControllers();

app.Run();
