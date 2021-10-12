using System;
using MediatR;

namespace MartenTrial.Model.Quest.Start
{
    public sealed record QuestStarted : INotification
    {
        public QuestStarted(Guid questId, string name)
        {
            QuestId = questId;
            Name = name;
        }
        
        public Guid QuestId { get; }

        public string Name { get; }

        public override string ToString() => $"Quest {Name} started";
    }
}