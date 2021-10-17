using System.Threading;
using System.Threading.Tasks;
using MartenTrial.Common.Messaging;
using MediatR;

namespace MartenTrial.Common.MediatR
{
    internal sealed class AsyncMediator
        : IAsyncSender
    {
        private readonly IMediator mediator;
        private readonly ScopedNotificationDispatcher dispatcher;

        public AsyncMediator(IMediator mediator, ScopedNotificationDispatcher dispatcher)
        {
            this.mediator = mediator;
            this.dispatcher = dispatcher;
        }

        public async Task<TNotification> SendAsync<TNotification>(
            ITraceableCommand request,
            CancellationToken cancellationToken = default)
            where TNotification : class, ITraceableNotification
        {
            var eventPromise = dispatcher.WaitEventAsync<TNotification>(request.Id);
            await mediator.Send(request, cancellationToken).ConfigureAwait(false);
            return await eventPromise.ConfigureAwait(false);
        }
    }
}