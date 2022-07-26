using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wing.DynamicProxy;

namespace Wing.Saga
{
    public class SagaTestAttribute : InterceptorAttribute
    {
        public override Task Invoke(IAspectContext context, AspectDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
