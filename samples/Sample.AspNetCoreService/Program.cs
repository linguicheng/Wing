using FreeSql;
using Sample.AspNetCoreService;
using System.Security.Claims;
using Wing;
using Wing.ServiceProvider;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Host.AddWing(builder => builder.AddConsul());

var fsql = new FreeSqlBuilder()
                              .UseConnectionString(DataType.SqlServer, builder.Configuration["ConnectionStrings:Sample.Wing"])
                              .UseAutoSyncStructure(true) // 自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                              .Build<SampleWingDbFlag>();
builder.Services.AddSingleton(typeof(IFreeSql<SampleWingDbFlag>), serviceProvider => fsql);
builder.Services.AddSingleton<ITracerService, TracerService>();
builder.Services.AddWing().AddPersistence().AddEventBus().AddJwt(context =>
{
    var user = context.User.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;
    return user == "byron";
}).AddAPM(x => x.AddFreeSql().Build(fsql));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
