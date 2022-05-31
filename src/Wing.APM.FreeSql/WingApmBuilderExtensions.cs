using System;

namespace Wing.APM.FreeSql
{
    public static class WingApmBuilderExtensions
    {
        public static WingApmBuilder AddFreeSql(this WingApmBuilder wingApmBuilder, IFreeSql fsql)
        {
            if (fsql == null)
            {
                throw new ArgumentNullException(nameof(fsql));
            }

            return wingApmBuilder;
        }
    }
}
