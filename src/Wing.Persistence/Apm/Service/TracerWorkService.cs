namespace Wing.Persistence.Apm
{
    public class TracerWorkService : ITracerWorkService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public TracerWorkService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }

        public long HttpTimeoutTotal()
        {
            return _fsql.Select<HttpTracer>()
                 .Where(x => x.UsedMillSeconds >= Config.ApmWorkServiceHttpTimeOut && x.RequestTime > Config.SearchTime)
                 .Count();
        }

        public long SqlTimeoutTotal()
        {
            return _fsql.Select<SqlTracer>()
                .Where(x => x.UsedMillSeconds >= Config.ApmWorkServiceSqlTimeOut && x.BeginTime > Config.SearchTime)
                .Count();
        }
    }
}
