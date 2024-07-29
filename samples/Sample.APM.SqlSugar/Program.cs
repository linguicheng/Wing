using SqlSugar;
using Wing;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddWing(builder => builder.AddConsul());

builder.Services.AddControllers();
builder.Services.AddWing()
                .AddPersistence(FreeSql.DataType.SqlServer)
                .AddAPM(x => x.AddSqlSugar());
builder.Services.AddScoped<ISqlSugarClient>(s =>
{
    SqlSugarClient sqlSugar = new SqlSugarClient(new ConnectionConfig()
    {
        DbType = DbType.SqlServer,
        ConnectionString = builder.Configuration["ConnectionStrings:Wing.Demo"],
        IsAutoCloseConnection = true,
    },
   db => db.AddWingAPM());
    return sqlSugar;
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
