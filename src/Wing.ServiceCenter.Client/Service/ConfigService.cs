using Wing.Model;
using Wing.Result;
using Wing.ServiceProvider.Dto;

namespace Wing.ServiceCenter.Client.Service
{
    public class ConfigService
    {
        private readonly IFreeSql<WingDbFlag> _fsql;

        public ConfigService(IFreeSql<WingDbFlag> fsql)
        {
            _fsql = fsql;
        }
    }
}
