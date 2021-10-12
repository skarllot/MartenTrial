namespace MartenTrial.Model.Quest.Events
{
    public class ArrivedAtLocation
    {
        public int Day { get; set; }

        public string Location { get; set; }

        public override string ToString()
        {
            return $"Arrived at {Location} on Day {Day}";
        }
    }
}