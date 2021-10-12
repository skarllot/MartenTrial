using System;
using System.Collections.Generic;
using MartenTrial.Model.Quest.Events;
using MartenTrial.Model.Quest.JoinMembers;
using MartenTrial.Model.Quest.Start;

namespace MartenTrial.Model.Quest.Views
{
    public class QuestParty
    {
        public List<string> Members { get; set; } = new();
        public IList<string> Slayed { get; } = new List<string>();
        public string Key { get; set; }
        public string Name { get; set; }

        // In this particular case, this is also the stream id for the quest events
        public Guid Id { get; set; }

        // These methods take in events and update the QuestParty
        public void Apply(MembersJoined joined) => Members.AddRange(joined.Members);
        public void Apply(MembersDeparted departed) => Members.RemoveAll(x => Array.IndexOf(departed.Members, x) >= 0);
        public void Apply(QuestStarted started) => Name = started.Name;

        public override string ToString()
        {
            return $"Quest party '{Name}' is {string.Join(", ", Members)}";
        }
    }
}