using System;

namespace MartenTrial.Model.Quest.Events
{
    public class MembersDeparted
    {
        public Guid Id { get; set; }

        public Guid QuestId { get; set; }

        public int Day { get; set; }

        public string Location { get; set; }

        public string[] Members { get; set; }

        public override string ToString()
        {
            return $"Members {string.Join(", ", Members)} departed at {Location} on Day {Day}";
        }
    }
}