using System.Threading;
using System.Threading.Tasks;
using MartenTrial.Common.Messaging;

namespace MartenTrial.Common.MediatR
{
    /// <summary>
    /// Send a request through the mediator and capture the collateral event with the result.
    /// </summary>
    public interface IAsyncSender
    {
        Task<TNotification> SendAsync<TNotification>(
            ITraceableCommand request,
            CancellationToken cancellationToken = default)
            where TNotification : class, ITraceableNotification;
    }
}