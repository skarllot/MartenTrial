using System.Threading;
using System.Threading.Tasks;
using Marten;
using MediatR;
using Validation;

namespace MartenTrial.Model.Quest.JoinMembers
{
    public sealed class JoiningMembersToQuest
        : AsyncRequestHandler<JoinMembersToQuest>
    {
        private readonly IDocumentStore store;

        public JoiningMembersToQuest(IDocumentStore store)
        {
            this.store = store;
        }

        protected override async Task Handle(JoinMembersToQuest request, CancellationToken cancellationToken)
        {
            await using var session = store.LightweightSession();

            var quest = await session.Events.AggregateStreamAsync<Quest>(request.Quest, token: cancellationToken);
            Verify.Operation(quest is not null, "The quest {0} was not found", request.Quest);
            quest.Commit();

            quest.JoinMembers(request.Day, request.Location, request.Members);

            session.Events.Append(quest.Id, quest.Version, quest.GetEvents());
            await session.SaveChangesAsync(cancellationToken);
        }
    }
}