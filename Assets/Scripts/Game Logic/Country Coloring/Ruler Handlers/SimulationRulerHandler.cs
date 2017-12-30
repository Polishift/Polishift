using System.Collections.Generic;
using System.Linq;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    //Todo: make this extend a common interface
    public class SimulationRulerHandler : AbstractRulerHandler
    {
        private int _currentYear = YearCounter.MinimumYear;

        public ElectionEntity[] LastElection;
        public ICountryRuler CurrentCountryRuler;


        //We dont use start(), since the CountryInformationReference is not filled in immediately.
        public override void Init()
        {
            ThisCountriesInfo = gameObject.GetComponent<CountryInformationReference>();
            
            //Setting this to a dummy value in case it is accessed before an election/dictatorship has happened.
            CurrentCountryRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
            LastElection = new ElectionEntity[1] {ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country)};
            IsInitialized = true;
        }

        public override void HandleRuler()
        {
            _currentYear = YearCounter.GetCurrentYear();
            
            var currentElections = GetCurrentElections();
            var currentDictatorships = GetCurrentDictatorships();            
            
            if (currentElections.Length > 0)
            {
                LastElection = currentElections;

                var biggestParty = currentElections.OrderByDescending(e => e.TotalVotePercentage).First();
                CurrentCountryRuler = biggestParty;
            }
            else if (currentDictatorships.Length > 0)
            {
                CurrentCountryRuler = currentDictatorships.First();
            }
            else if (CurrentRulerIsDictator()) //if the currently set ruler is a dictator BUT his reign ends this year
            {
                CurrentCountryRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
            }

            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(CurrentCountryRuler.PartyClassification);
        }
      


        private ElectionEntity[] GetCurrentElections()
        {
            if (ThisCountriesInfo.AllElectionsEverForThisCountry.Any())
                return ThisCountriesInfo.AllElectionsEverForThisCountry.Where(e => e.Year == _currentYear).ToArray();
            else
                return new ElectionEntity[0] {};

        }

        private DictatorshipEntity[] GetCurrentDictatorships()
        {
            if (ThisCountriesInfo.AllDictatorshipsEverForThisCountry.Any())
                return ThisCountriesInfo.AllDictatorshipsEverForThisCountry.Where(d => d.From <= _currentYear && d.To >= _currentYear).ToArray();
            else
                return new DictatorshipEntity[0] {};
        }


        private bool CurrentRulerIsDictator()
        {
            return CurrentCountryRuler.GetRulerType() == RulerType.Dictator;
        }
    }
}