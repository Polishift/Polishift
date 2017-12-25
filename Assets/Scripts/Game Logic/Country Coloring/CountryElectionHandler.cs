using System.Collections.Generic;
using System.Linq;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    [RequireComponent(typeof(CountryInformationReference), typeof(CountryColorer))]
    public class CountryElectionHandler : MonoBehaviour
    {
        private CountryInformationReference _thisCountrysInfo;
        private bool _countryInformationIsSet;

        public ElectionEntity[] LastElection;
        public ICountryRuler CurrentCountryRuler;

        //todo: This is dirty, but necessary for the predicitions. Fix this
        public int CurrentYear = YearCounter.MinimumYear;


        //We dont use start(), since the CountryInformationReference is not filled in immediately.
        public void Init(CountryInformationReference thisCountriesInformationReference)
        {
            _thisCountrysInfo = thisCountriesInformationReference;
            
            //Setting this to a dummy value in case it is accessed before an election/dictatorship has happened.
            CurrentCountryRuler = ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country);
            LastElection = new ElectionEntity[1] {ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country)};
            _countryInformationIsSet = true;
        }

        private void Update()
        {
            CurrentYear = YearCounter.GetCurrentYear();
            if (!_countryInformationIsSet) return;

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
                CurrentCountryRuler = ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country);
            }

            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(CurrentCountryRuler.PartyClassification);
        }

        private ElectionEntity[] GetCurrentElections()
        {
            if (_thisCountrysInfo.AllElectionsEverForThisCountry.Any())
                return _thisCountrysInfo.AllElectionsEverForThisCountry.Where(e => e.Year == CurrentYear).ToArray();
            else
                return new ElectionEntity[0] {};

        }

        private DictatorshipEntity[] GetCurrentDictatorships()
        {
            if (_thisCountrysInfo.AllDictatorshipsEverForThisCountry.Any())
                return _thisCountrysInfo.AllDictatorshipsEverForThisCountry.Where(d => d.From <= CurrentYear && d.To >= CurrentYear).ToArray();
            else
                return new DictatorshipEntity[0] {};
        }


        private bool CurrentRulerIsDictator()
        {
            return CurrentCountryRuler.GetRulerType() == RulerType.Dictator;
        }
    }
}