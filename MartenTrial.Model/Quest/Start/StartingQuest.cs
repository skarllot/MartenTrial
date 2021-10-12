using System.Threading;
using System.Threading.Tasks;
using Marten;
using MartenTrial.Common;
using MartenTrial.Common.MediatR;
using MediatR;

namespace MartenTrial.Model.Quest.Start
{
    public class StartingQuest
        : AsyncRequestHandler<StartQuest>
    {
        private readonly IPublisher publisher;
        private readonly IDocumentStore store;

        public StartingQuest(IPublisher publisher, IDocumentStore store)
        {
            this.publisher = publisher;
            this.store = store;
        }

        protected override async Task Handle(StartQuest request, CancellationToken cancellationToken)
        {
            await using var session = store.LightweightSession();

            var quest = Quest.Start(request.Name);

            session.Events.StartStream<Quest>(quest.Id, quest.GetEvents());
            await session.SaveChangesAsync(cancellationToken);
            
            await publisher.PublishAll(quest.GetEvents(), cancellationToken);
        }
    }
}