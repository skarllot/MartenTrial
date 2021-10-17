using System.Threading;
using System.Threading.Tasks;
using MartenTrial.Common.Messaging;
using MediatR;

namespace MartenTrial.Common.MediatR
{
    internal sealed class NotificationRouter<TNotification>
        : INotificationHandler<TNotification>
        where TNotification : class, ITraceableNotification
    {
        private readonly ScopedNotificationDispatcher dispatcher;

        public NotificationRouter(ScopedNotificationDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            dispatcher.Notify(notification);
            return Task.CompletedTask;
        }
    }
}