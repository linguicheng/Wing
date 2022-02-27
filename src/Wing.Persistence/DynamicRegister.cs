using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Wing.DynamicMethod;

namespace Wing.Persistence
{
    public class DynamicRegister : GlobalInjection
    {
        public static void RegisterContext(IServiceCollection services, string gateWayConnectionString, string mqConnectionString)
        {
            var type = Assemblies.SelectMany(a =>
                      a.GetTypes().Where(t =>
                            t.GetInterfaces().Where(i =>
                                i == typeof(IRegisterContext)).Any())).First();
            var context = (IRegisterContext)Activator.CreateInstance(type);
            context.AddContext(services, gateWayConnectionString, mqConnectionString);
        }
    }
}
