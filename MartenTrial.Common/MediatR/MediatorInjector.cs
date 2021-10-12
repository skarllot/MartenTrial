using System.ComponentModel;
using MartenTrial.Common.DependencyInjection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MartenTrial.Common.MediatR
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class MediatorInjector
    {
        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            services.TryAddTransient<ServiceFactory>(p => p.GetRequiredService);
            services.TryAdd(new ServiceDescriptor(typeof(IMediator), typeof(Mediator), lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(ISender), static sp => sp.GetRequiredService<IMediator>(), lifetime));
            services.TryAdd(new ServiceDescriptor(typeof(IPublisher), static sp => sp.GetRequiredService<IMediator>(), lifetime));

            services.TryAdd(new ServiceDescriptor(typeof(ScopedNotificationDispatcher),
                typeof(ScopedNotificationDispatcher), lifetime));

            return services;
        }

        public static IServiceCollection AddNotificationRouter<TNotification>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TNotification : class, INotification
        {
            return services.TryAddAbstractions<NotificationRouter<TNotification>>(lifetime);
        }
    }
}