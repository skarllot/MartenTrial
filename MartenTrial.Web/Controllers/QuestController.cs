using System;
using System.Threading;
using System.Threading.Tasks;
using Marten;
using MartenTrial.Common.MediatR;
using MartenTrial.Model.Quest.Start;
using MartenTrial.Model.Quest.Views;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MartenTrial.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestController
        : ControllerBase
    {
        private readonly ISender sender;
        private readonly ScopedNotificationDispatcher dispatcher;
        private readonly IDocumentStore store;

        public QuestController(ISender sender, ScopedNotificationDispatcher dispatcher, IDocumentStore store)
        {
            this.sender = sender;
            this.dispatcher = dispatcher;
            this.store = store;
        }

        [HttpPost("start")]
        public async Task<Guid> Start(StartQuest request, CancellationToken cancellationToken)
        {
            var eventPromise = dispatcher.WaitEventAsync<QuestStarted>(n => n.Name == request.Name);
            await sender.Send(request, cancellationToken);

            return (await eventPromise).QuestId;
        }

        [HttpGet]
        public async Task<QuestParty?> Get([FromQuery] Guid id)
        {
            await using var session = store.LightweightSession();
            return await session.Events.AggregateStreamAsync<QuestParty>(id);
        }
    }
}