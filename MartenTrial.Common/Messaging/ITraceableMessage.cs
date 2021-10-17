using System;

namespace MartenTrial.Common.Messaging
{
    public interface ITraceableMessage
    {
        Guid Id { get; }
        Guid Correlation { get; }
        Guid Causation { get; }
    }
}