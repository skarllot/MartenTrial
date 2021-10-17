using MediatR;

namespace MartenTrial.Common.Messaging
{
    public interface ITraceableNotification
        : INotification, ITraceableMessage
    {
    }
}