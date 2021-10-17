using System;
using MartenTrial.Common.Messaging;
using Newtonsoft.Json;

namespace MartenTrial.Model.Quest.Start
{
    public sealed record QuestStarted : ITraceableNotification
    {
        [JsonConstructor]
        private QuestStarted(Guid id, Guid correlation, Guid causation, Guid questId, string name)
        {
            Id = id;
            Correlation = correlation;
            Causation = causation;
            QuestId = questId;
            Name = name;
        }

        public Guid Id { get; }
        public Guid Correlation { get; }
        public Guid Causation { get; }
        public Guid QuestId { get; }
        public string Name { get; }

        public static QuestStarted CreateFrom(StartQuest causation) => new(
            id: Guid.NewGuid(),
            correlation: causation.Correlation,
            causation: causation.Id,
            questId: causation.Id,
            name: causation.Name);

        public override string ToString() => $"Quest {Name} started";
    }
}