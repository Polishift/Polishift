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

        //We dont use start(), since the CountryInformationReference is not filled in immediately.
        public override void Init()
        {
            ThisCountriesInfo = gameObject.GetComponent<CountryInformationReference>();
            
            //Setting this to a dummy value in case it is accessed before an election/dictatorship has happened.
            base.CurrentRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
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
                base.CurrentRuler = biggestParty;
            }
            else if (currentDictatorships.Length > 0)
            {
                base.CurrentRuler = currentDictatorships.First();
            }
            else if (CurrentRulerIsDictator()) //if the currently set ruler is a dictator BUT his reign ends this year
            {
                base.CurrentRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
            }

            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(base.CurrentRuler.PartyClassification);
        }

        public override string RulerToText()
        {
            return ThisCountriesInfo.Iso3166Country.Name + " is currently ruled by the " + CurrentRuler
                   + "\n " + GetRunnersUpText();
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
            return base.CurrentRuler.GetRulerType() == RulerType.Dictator;
        }
        
        //Methods for stringyfying        
        private string GetRunnersUpText()
        {
            string returnStr = "";

            if (CurrentRuler.GetRulerType() != RulerType.Dictator)
            {
                returnStr = "The runners up were: \n";
                foreach (var runnerUp in GetRunnerUpsForLastElection())
                {
                    returnStr += runnerUp.PartyName + ": " + runnerUp.TotalVotePercentage.ToString ("0.##") + "% \n";
                }
            }
            
            return returnStr;
        }

        private ElectionEntity[] GetRunnerUpsForLastElection()
        {
            var biggestFiveParties = LastElection.OrderByDescending(e => e.TotalVotePercentage).Take(5);

            return biggestFiveParties.Skip(1).ToArray();
        }
        
    }
}