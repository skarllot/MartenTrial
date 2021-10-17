using System;
using MartenTrial.Common.Messaging;

namespace MartenTrial.Model.Quest.Start
{
    public sealed record StartQuest : ITraceableCommand
    {
        private StartQuest(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }
        public Guid Correlation => Id;
        public Guid Causation => Id;
        public string Name { get; }

        public static StartQuest Create(string name) => new(Guid.NewGuid(), name);
    }
}