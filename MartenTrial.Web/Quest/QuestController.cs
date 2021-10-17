using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using MartenTrial.Common.MediatR;
using MartenTrial.Model.Quest.Start;
using MartenTrial.Model.Quest.Views;
using Microsoft.AspNetCore.Mvc;

namespace MartenTrial.Web.Quest
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController
        : ControllerBase
    {
        private readonly IAsyncSender sender;
        private readonly IDocumentStore store;

        public QuestController(IAsyncSender sender, IDocumentStore store)
        {
            this.sender = sender;
            this.store = store;
        }

        [HttpPost("start")]
        public async Task<StartQuestResponse> Start(StartQuestRequest request, CancellationToken cancellationToken)
        {
            var command = StartQuest.Create(request.Name);
            var questStarted = await sender.SendAsync<QuestStarted>(command, cancellationToken);

            return new StartQuestResponse(questStarted.QuestId);
        }

        [HttpGet]
        public async Task<QuestParty?> Get([FromQuery] Guid id)
        {
            await using var session = store.LightweightSession();
            return await session.Events.AggregateStreamAsync<QuestParty>(id);
        }
    }
}