using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MartenTrial.Common.DependencyInjection
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection TryAddSingletonAbstractions<TService>(this IServiceCollection services)
            where TService : class
        {
            return TryAddAbstractions<TService>(services, ServiceLifetime.Singleton);
        }

        public static IServiceCollection TryAddScopedAbstractions<TService>(this IServiceCollection services)
            where TService : class
        {
            return TryAddAbstractions<TService>(services, ServiceLifetime.Scoped);
        }

        public static IServiceCollection TryAddAbstractions<TService>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
            where TService : class
        {
            services.TryAdd(ServiceDescriptor.Describe(typeof(TService), typeof(TService), lifetime));

            Func<Type, ServiceDescriptor> mapper = lifetime switch
            {
                ServiceLifetime.Singleton => MapSingletonDescriptorFromType,
                ServiceLifetime.Scoped => MapScopedDescriptorFromType,
                ServiceLifetime.Transient => MapTransientDescriptorFromType,
                _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
            };

            services.TryAdd(typeof(TService).GetInterfaces().Select(mapper));
            return services;

            static ServiceDescriptor MapSingletonDescriptorFromType(Type t) =>
                ServiceDescriptor.Singleton(t, static c => c.GetRequiredService<TService>());

            static ServiceDescriptor MapScopedDescriptorFromType(Type t) =>
                ServiceDescriptor.Scoped(t, static c => c.GetRequiredService<TService>());

            static ServiceDescriptor MapTransientDescriptorFromType(Type t) =>
                ServiceDescriptor.Transient(t, static c => c.GetRequiredService<TService>());
        }
    }
}