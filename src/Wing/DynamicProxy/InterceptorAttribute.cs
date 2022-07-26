using System;
using System.Threading.Tasks;

namespace Wing.DynamicProxy
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public abstract class InterceptorAttribute : Attribute
    {
        public abstract Task Invoke(IAspectContext context, AspectDelegate next);
    }
}
