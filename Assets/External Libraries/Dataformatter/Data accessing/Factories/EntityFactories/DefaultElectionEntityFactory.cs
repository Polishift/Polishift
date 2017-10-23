using System.Collections.Generic;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;

namespace Dataformatter.Data_accessing.Factories.EntityFactories
{
    class DefaultElectionEntityFactory : EntityFactory<ConstituencyElectionModel, ElectionEntity>
    {
        private const int MissingValueKey = -990;

        public override ElectionEntity Create(ConstituencyElectionModel rawModel)
        {
            return new ElectionEntity
            {
                Year = rawModel.Year,
                CountryName = rawModel.CountryName,
                CountryCode = CreateCountryCode(rawModel.CountryName),
                PartyClassification = "Unknown", //Where/How will we be doing this?
                PartyName = rawModel.PartyName,
                PartyCandidates = new HashSet<string> { rawModel.CandidateName },
                TotalAmountOfVotes = 0,
                TotalVotePercentage = 0.0,
                TotalAmountOfSeatsGained = GetFormattedSeatsGained(rawModel.SeatsGained)
            };
        }

        private static int GetFormattedSeatsGained(int rawSeatsGained)
        {
            return rawSeatsGained == MissingValueKey ? 0 : rawSeatsGained;
        }
    }
}