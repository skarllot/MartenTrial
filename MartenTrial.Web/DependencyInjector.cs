using System.ComponentModel;
using MartenTrial.Common.MediatR;
using MartenTrial.Model.Quest.Start;
using Microsoft.Extensions.DependencyInjection;

namespace MartenTrial.Web
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class DependencyInjector
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            return services
                .AddNotificationRouter<QuestStarted>();
        }
    }
}