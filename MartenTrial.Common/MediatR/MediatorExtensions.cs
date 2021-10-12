using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace MartenTrial.Common.MediatR
{
    public static class MediatorExtensions
    {
        public static async Task PublishAll(
            this IPublisher publisher,
            IEnumerable<INotification> notifications,
            CancellationToken cancellationToken = default)
        {
            foreach (var notification in notifications)
            {
                await publisher.Publish(notification, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}