using System;
using System.Collections.Generic;
using System.Reflection;

namespace Wing.DynamicProxy
{
    public interface IAspectContext
    {
        IDictionary<string, object> AdditionalData { get; set; }

        object ReturnValue { get; set; }

        MethodInfo ServiceMethod { get; set; }

        object Implementation { get; set; }

        MethodInfo ImplementationMethod { get; set; }

        object[] Parameters { get; set; }

        MethodInfo ProxyMethod { get; set; }
    }
}
