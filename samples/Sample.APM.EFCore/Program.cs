using Microsoft.EntityFrameworkCore;
using Sample.APM.EFCore;
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddDbContext<EFCoreDemoContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("Wing.Demo")));

builder.Services.AddWing()
       .AddPersistence(FreeSql.DataType.SqlServer)
       .AddAPM(x => x.AddEFCore());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
