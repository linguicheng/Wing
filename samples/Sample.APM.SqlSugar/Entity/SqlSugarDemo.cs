using SqlSugar;

namespace Sample.APM.SqlSugar
{
    public class SqlSugarDemo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
