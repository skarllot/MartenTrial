using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace MartenTrial.Model.Quest.JoinMembers
{
    public sealed record MembersJoined : INotification
    {
        public MembersJoined(int day, string location, params string[] members)
        {
            Day = day;
            Location = location;
            Members = members;
        }

        public int Day { get; }

        public string Location { get; }

        public IReadOnlyList<string> Members { get; }

        public override string ToString()
        {
            return $"Members {string.Join(", ", Members)} joined at {Location} on Day {Day}";
        }

        public bool Equals(MembersJoined? other)
        {
            return other is not null &&
                   Day == other.Day &&
                   Location == other.Location &&
                   Members.SequenceEqual(other.Members);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Day, Location, Members);
        }
    }
}