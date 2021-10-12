using System;
using System.Collections.Generic;
using MartenTrial.Common.Domain;
using MartenTrial.Model.Quest.JoinMembers;
using MartenTrial.Model.Quest.Start;
using Validation;

namespace MartenTrial.Model.Quest
{
    internal class Quest
        : AggregateBase
    {
        private readonly List<string> members = new();

        private Quest(QuestStarted started)
        {
            Id = started.QuestId;
            Name = started.Name;
            OnEventApplied(started);
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public static Quest Start(string name)
        {
            Requires.NotNullOrWhiteSpace(name, nameof(name));
            return new Quest(new QuestStarted(Guid.NewGuid(), name));
        }

        public void JoinMembers(int day, string location, params string[] newMembers)
        {
            Requires.Range(day > 0, nameof(day));
            Requires.NotNullOrWhiteSpace(location, nameof(location));
            Requires.NotNullOrEmpty(newMembers, nameof(newMembers));

            Apply(new MembersJoined(day, location, newMembers));
        }

        private void Apply(MembersJoined joined)
        {
            members.AddRange(joined.Members);
            OnEventApplied(joined);
        }
    }
}