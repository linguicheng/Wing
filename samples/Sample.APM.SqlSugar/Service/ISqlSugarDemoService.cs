using System.Threading.Tasks;

namespace Sample.APM.SqlSugar
{
    public interface ISqlSugarDemoService
    {
        Task<int> Add(SqlSugarDemo entity);
    }
}
