using System.Collections.Generic;
using System.Reflection;

namespace Wing.DynamicProxy
{
    public class AspectContext : IAspectContext
    {
        public IDictionary<string, object> AdditionalData { get; set; }

        public object ReturnValue { get; set; }

        public MethodInfo ServiceMethod { get; set; }

        public object Implementation { get; set; }

        public MethodInfo ImplementationMethod { get; set; }

        public object[] Parameters { get; set; }

        public MethodInfo ProxyMethod { get; set; }

        public AspectContext NewContext
        {
            get
            {
                return new AspectContext
                {
                    AdditionalData = AdditionalData,
                };
            }
        }
    }
}
