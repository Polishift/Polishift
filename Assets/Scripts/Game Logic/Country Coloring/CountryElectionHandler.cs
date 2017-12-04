using System.Collections.Generic;
using System.Linq;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    public class CountryElectionHandler : MonoBehaviour
    {
        private CountryInformationReference _thisCountrysInfo;
        private bool _countryInformationIsSet;
        
        private List<ElectionEntity> _allElectionsEverForThisCountry;
        private List<DictatorshipEntity> _allDictatorshipsEverForThisCountry;
        private ICountryRuler _currentCountryRuler;

        private int _currentYear; 
        
        
        //We dont use start(), since the CountryInformationReference is not filled in immediately.
        public void Init()
        {
            _thisCountrysInfo = gameObject.GetComponent<CountryInformationReference>();

            _allElectionsEverForThisCountry = RepositoryHub.ElectionsRepository.GetByCountry(_thisCountrysInfo.Iso3166Country.Alpha3).ToList();
            _allDictatorshipsEverForThisCountry = RepositoryHub.DictatorShipsRepository.GetByCountry(_thisCountrysInfo.Iso3166Country.Alpha3).ToList();
            
            //Setting this to a dummy value in case it is accessed before an election/dictatorship has happened.
            _currentCountryRuler = ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country);
            _countryInformationIsSet = true;
        }
        
        void Update()
        {
            _currentYear = YearCounter.GetCurrentYear();
            if (!_countryInformationIsSet) return;


            var currentElections = GetCurrentElections();
            var currentDictatorships = GetCurrentDictatorships();
            
            if (currentElections.Length > 0)
            {
                var biggestParty = currentElections.OrderByDescending(e => e.TotalVotePercentage).First();
                _currentCountryRuler = biggestParty;
            }
            else if (currentDictatorships.Length > 0)
            {
                Debug.Log("In " + _thisCountrysInfo.Iso3166Country.Name + " in " + _currentYear + " theres a dict");
                _currentCountryRuler = currentDictatorships.First();
            }
            else if(CurrentRulerIsDictator()) //if the currently set ruler is a dictator BUT his reign ends this year
            {
                _currentCountryRuler = ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country); 
            }
            
            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(_currentCountryRuler.PartyClassification);    
        }

        public override string ToString()
        {
            return _thisCountrysInfo.Iso3166Country.Name + " is currently ruled by " + _currentCountryRuler;
        }

        
        private DictatorshipEntity[] GetCurrentDictatorships()
        {
            return _allDictatorshipsEverForThisCountry.Where(d => d.From <= _currentYear && d.To >= _currentYear).ToArray();
        }
        
        private ElectionEntity[] GetCurrentElections()
        {
            return _allElectionsEverForThisCountry.Where(e => e.Year == _currentYear).ToArray();
        }
        
        
        private bool CurrentRulerIsDictator()
        {
            return _currentCountryRuler.GetRulerType() == RulerType.Dictator;
        }
    }
}