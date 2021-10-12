using System.Collections.Generic;
using MediatR;

namespace MartenTrial.Common.Domain
{
    public abstract class AggregateBase
    {
        private readonly List<INotification> events = new();

        public long Version { get; private set; }

        public IReadOnlyList<INotification> GetEvents() => events;

        public void Commit() => events.Clear();

        protected void OnEventApplied<TEvent>(TEvent @event)
            where TEvent : class, INotification
        {
            events.Add(@event);
            Version++;
        }
    }
}