using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data.Repositories.Models
{
    class ConstituencyElectionModel : IModel
    {
        public int Year { get; set; }
        public string CountryName { get; set; }
        public string PartyName { get; set; }
        public string CandidateName { get; set; }
        public double VoteFraction { get; set; }
    }
}
