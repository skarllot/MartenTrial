using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MediatR;
using Validation;

namespace MartenTrial.Common.MediatR
{
    public sealed class ScopedNotificationDispatcher
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, Operation>> observers = new();

        public Task<TNotification> WaitEventAsync<TNotification>(Func<TNotification, bool> filter)
            where TNotification : class, INotification
        {
            var typeObservers = observers
                .GetOrAdd(typeof(TNotification), static _ => new ConcurrentDictionary<object, Operation>());

            var operation = Operation<TNotification>.RegisterNew(typeObservers, filter);
            return operation.Promise;
        }

        internal void Notify<TNotification>(TNotification notification)
            where TNotification : class, INotification
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
            where TNotification : class, INotification
        {
            private readonly ConcurrentDictionary<object, Operation> registry;
            private readonly Func<TNotification, bool> filter;
            private readonly TaskCompletionSource<TNotification> completionSource;

            private Operation(ConcurrentDictionary<object, Operation> registry, Func<TNotification, bool> filter)
            {
                this.registry = registry;
                this.filter = filter;
                completionSource = new TaskCompletionSource<TNotification>(
                    TaskCreationOptions.RunContinuationsAsynchronously);
            }

            public Task<TNotification> Promise => completionSource.Task;

            public static Operation<TNotification> RegisterNew(
                ConcurrentDictionary<object, Operation> registry,
                Func<TNotification, bool> filter)
            {
                var operation = new Operation<TNotification>(registry, filter);
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
                if (!filter(notification))
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