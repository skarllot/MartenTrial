using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MartenTrial.Common.Messaging;
using Validation;

namespace MartenTrial.Common.MediatR
{
    public sealed class ScopedNotificationDispatcher
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, Operation>> observers = new();

        public Task<TNotification> WaitEventAsync<TNotification>(Guid causation)
            where TNotification : class, ITraceableNotification
        {
            var typeObservers = observers
                .GetOrAdd(typeof(TNotification), static _ => new ConcurrentDictionary<object, Operation>());

            var operation = Operation<TNotification>.RegisterNew(typeObservers, causation);
            return operation.Promise;
        }

        internal void Notify<TNotification>(TNotification notification)
            where TNotification : class, ITraceableNotification
        {
            if (!observers.TryGetValue(typeof(TNotification), out var typeObservers))
            {
                return;
            }

            foreach (var operation in typeObservers.Values)
            {
                operation.Notify(notification);
            }
        }

        private abstract class Operation
        {
            public abstract void Notify(object notification);
        }

        private sealed class Operation<TNotification>
            : Operation
            where TNotification : class, ITraceableNotification
        {
            private readonly ConcurrentDictionary<object, Operation> registry;
            private readonly Guid causation;
            private readonly TaskCompletionSource<TNotification> completionSource;

            private Operation(ConcurrentDictionary<object, Operation> registry, Guid causation)
            {
                this.registry = registry;
                this.causation = causation;
                completionSource = new TaskCompletionSource<TNotification>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public Task<TNotification> Promise => completionSource.Task;

            public static Operation<TNotification> RegisterNew(
                ConcurrentDictionary<object, Operation> registry,
                Guid causation)
            {
                var operation = new Operation<TNotification>(registry, causation);
                registry.TryAdd(operation, operation);
                return operation;
            }

            public override void Notify(object notification)
            {
                Report.If(notification is not TNotification);
                Notify((TNotification)notification);
            }

            public void Notify(TNotification notification)
            {
                if (notification.Causation != causation)
                {
                    return;
                }

                if (completionSource.TrySetResult(notification))
                {
                    registry.TryRemove(this, out _);
                }
            }
        }
    }
}