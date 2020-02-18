using System;
using Microsoft.Extensions.DependencyInjection;

namespace TestAppApi.Infrastructure
{
    public class ServiceResolvingLazy<T> : Lazy<T>
    {
        public ServiceResolvingLazy(IServiceProvider serviceProvider)
            : base(serviceProvider.GetRequiredService<T>)
        {

        }
    }
}
