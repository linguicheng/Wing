using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Wing.ServiceProvider;

namespace Wing.Injection
{
    public class Scoped
    {
        public static Task Create(Func<IServiceScope, Task> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var scoped = ServiceLocator.GetService<IServiceScopeFactory>().CreateScope();
            return func(scoped);
        }

        public static Task<T> Create<T>(Func<IServiceScope, Task<T>> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var scoped = ServiceLocator.GetService<IServiceScopeFactory>().CreateScope();
            return func(scoped);
        }

        public static void Create(Action<IServiceScope> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var scoped = ServiceLocator.GetService<IServiceScopeFactory>().CreateScope();
            action(scoped);
        }

        public static T Create<T>(Func<IServiceScope, T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            var scoped = ServiceLocator.GetService<IServiceScopeFactory>().CreateScope();
            return func(scoped);
        }
    }
}
