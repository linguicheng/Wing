using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Wing.EventBus;
using Wing.Policy;

namespace Wing.DynamicMethod
{
    public class GlobalInjection
    {
        public static readonly Assembly[] Assemblies;

        static GlobalInjection()
        {
            Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        public static void CreateSubscribe(IEventBus eventBus)
        {
            var types = Assemblies
                .SelectMany(a =>
                    a.GetTypes()
                    .Where(t =>
                        t.GetInterfaces()
                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ISubscribe<>))
                        .Any()))
                   .ToArray();
            var subscribeMethod = typeof(IEventBus).GetMethod("Subscribe");
            foreach (Type consumerType in types)
            {
                if (consumerType.IsInterface)
                {
                    continue;
                }

                var eventMessageType = consumerType.GetInterfaces()
                                       .Where(type =>
                                            type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(ISubscribe<>))
                                       .Select(type =>
                                           type.GetGenericArguments()
                                           .Single())
                                       .First();
                MethodInfo curMethod = subscribeMethod.MakeGenericMethod(eventMessageType, consumerType);
                curMethod.Invoke(eventBus, null);
            }
        }

        public static void RegisterCommandService(IServiceCollection services)
        {
            foreach (var type in Assemblies.SelectMany(a =>
            a.GetTypes()
            .Where(t => !t.IsInterface && System.Convert.ToBoolean(
                t.GetInterfaces().FirstOrDefault()?.GetMethods()
                .Any(m =>
                     m.GetCustomAttribute(typeof(PolicyAttribute)) != null))))
            .ToArray())
            {
                services.AddSingleton(type.GetInterfaces().First(), type);
            }
        }
    }
}
