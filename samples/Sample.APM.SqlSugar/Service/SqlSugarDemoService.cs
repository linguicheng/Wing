using SqlSugar;
using System.Threading.Tasks;
using Wing.Injection;

namespace Sample.APM.SqlSugar
{
    public class SqlSugarDemoService : ISqlSugarDemoService, IScoped
    {
        private readonly ISqlSugarClient db;

        public SqlSugarDemoService(ISqlSugarClient client)
        {
            db = client;
        }
        public Task<int> Add(SqlSugarDemo entity)
        {
            db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(SqlSugarDemo));
            return db.Insertable(entity).ExecuteCommandAsync();
        }
    }
}
