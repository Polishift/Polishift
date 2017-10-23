namespace Dataformatter.Dataprocessing.Entities
{
    public class TurnoutEntity : IEntity
    {
        public string CountryCode { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public double VoterTurnout { get; set; }
        public int TotalVotes { get; set; }
        public int Registration { get; set; }
        public double VapTurnout { get; set; }
        public int VotingAge { get; set; }
        public int Population { get; set; }
        public double InvalidVotes { get; set; }
    }
}