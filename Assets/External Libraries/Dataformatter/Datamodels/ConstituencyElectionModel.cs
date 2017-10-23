namespace Dataformatter.Datamodels
{
    class ConstituencyElectionModel : IModel
    {
        public int Year { get; set; }
        public string CountryName { get; set; }
        public string ConstituencyName { get; set; }
        public string PartyName { get; set; }
        public string CandidateName { get; set; }

        public int TotalAmountOfVotes { get; set; }
        public double AmountOfPartyVotes { get; set; }
        public int SecondRoundAmountOfPartyVotes { get; set; }

        public int SeatsGained { get; set; }
    }
}
