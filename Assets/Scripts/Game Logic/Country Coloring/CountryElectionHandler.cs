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
        private ElectionEntity _currentRulingPartyElectionData;

        private int _currentYear = 0000; 
        
        //We dont use start(), since the CountryInformationReference is not filled in immediately.
        public void Init()
        {
            _thisCountrysInfo = gameObject.GetComponent<CountryInformationReference>();

            _allElectionsEverForThisCountry =
                RepositoryHub.ElectionsRepository.GetByCountry(_thisCountrysInfo.Iso3166Country.Alpha3).ToList();
            
            //Setting this to a dummy value in case it is accessed before an election/regime has happened.
            _currentRulingPartyElectionData = ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country);
            _countryInformationIsSet = true;
        }
        
        void Update()
        {
            _currentYear = YearCounter.GetCurrentYear();
            if (!_countryInformationIsSet) return;

            
            var currentDictatorships = GetCurrentDictatorships();
            
            if (CurrentYearIsElectionYear() ^ currentDictatorships.Length > 0)
            {
                UpdateRulingParty(); //TODO: MAKE PARTY AND DICTATORSHIP MEMBER OF SAME INTERFACE SO THIS DOESNT HAVE TO CHANGE AT ALL
                gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(_currentRulingPartyElectionData.PartyClassification);    
            }
                
                
                  
        }

        public override string ToString()
        {
            var thisCountry = _thisCountrysInfo.Iso3166Country;
            
            if (_currentRulingPartyElectionData == null)
            {
                return thisCountry.Name + " currently has no ruling party.";
            }
            else
            {
                return _currentRulingPartyElectionData.CountryName + " is currently ruled by the '" 
                       + _currentRulingPartyElectionData.PartyName + "' " 
                       + " who are of type '" + _currentRulingPartyElectionData.PartyClassification + "'. "
                       + " They won the elections of " + _currentRulingPartyElectionData.Year;
            }    
        }


        private DictatorshipEntity[] GetCurrentDictatorships()
        {
            var thisCountrysCode = _thisCountrysInfo.Iso3166Country.Alpha3;
            var allDictatorShipsEverInThisCountry = RepositoryHub.DictatorShipsRepository.GetByCountry(thisCountrysCode);

            return allDictatorShipsEverInThisCountry.Where(d => d.From <= _currentYear && d.To >= _currentYear).ToArray();
        }
        
        private DictatorshipEntity[]
        
        
        
        private bool CurrentYearIsElectionYear()
        {
            var currentYear = YearCounter.GetCurrentYear();
            return _allElectionsEverForThisCountry.Count(e => e.Year == currentYear) >= 1;
        }

        private void UpdateRulingParty()
        {
            //duplication
            var currentYear = YearCounter.GetCurrentYear();
            var currentYearsElection = _allElectionsEverForThisCountry.Where(e => e.Year == currentYear).ToList();

            var newRulingPoliticalParty = currentYearsElection.OrderByDescending(p => p.TotalVotePercentage).First();
            _currentRulingPartyElectionData = newRulingPoliticalParty;
        }
    }
}