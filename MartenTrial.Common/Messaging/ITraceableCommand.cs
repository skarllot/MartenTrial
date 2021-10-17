using MediatR;

namespace MartenTrial.Common.Messaging
{
    public interface ITraceableCommand
        : IRequest, ITraceableMessage
    {
    }
}