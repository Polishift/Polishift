using System;
using Assets.Data.Repositories.Models;
using Assets.Dataprocessing.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Assets.Data_accessing.Factories
{
    class DefaultElectionEntityFactory : AbstractElectionEntityFactory
    {
        public override ElectionEntity Create(ConstituencyElectionModel rawModel)
        {
            return new ElectionEntity
            {
                Year = rawModel.Year,
                CountryCode = CreateCountryCode(rawModel.CountryName),
                PartyClassification = "Unknown", //Where/How will we be doing this?
                PartyName = rawModel.PartyName,
                PartyCandidates = new HashSet<string>() { rawModel.CandidateName },
                TotalVotePercentage = rawModel.VoteFraction * 100
            };
        }
    
        private string CreateCountryCode(string FullCountryName) //maybe do this in a more central place?
        {
            return FullCountryName.Take(3).ToString().ToUpper();
        }
    }
}
