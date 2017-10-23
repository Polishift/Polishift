using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;

namespace Dataformatter.Data_accessing.Factories.EntityFactories
{
    public class DefaultTurnoutEntityFactory : EntityFactory<TurnoutModel,
                                                             TurnoutEntity>
    {
        public override TurnoutEntity Create(TurnoutModel rawModel)
        {
            return new TurnoutEntity
            {
                InvalidVotes = rawModel.InvalidVotes,
                CountryCode = CreateCountryCode(rawModel.CountryName),
                Population = rawModel.Population,
                Registration = rawModel.Registration,
                TotalVotes = rawModel.TotalVotes,
                Type = rawModel.Type,
                VapTurnout = rawModel.VapTurnout,
                VoterTurnout = rawModel.VoterTurnout,
                VotingAge = rawModel.VotingAge,
                Year = rawModel.Year
            };
        }

        
    }
}