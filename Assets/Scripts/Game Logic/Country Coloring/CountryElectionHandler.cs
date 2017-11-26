using System.Collections.Generic;
using System.Linq;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    public class CountryElectionHandler : MonoBehaviour
    {
        private CountryInformationReference _thisCountrysInfo;
        private bool _countryInformationIsSet = false;
        private List<ElectionEntity> _allElectionsEverForThisCountry;
        private ElectionEntity _currentRulingPartyElectionData;
        
        
        //We dont use start(), since the CountryInformationReference is not filled in immediately.
        public void Init()
        {
            _thisCountrysInfo = gameObject.GetComponent<CountryInformationReference>();

            _allElectionsEverForThisCountry =
                RepositoryHub.ElectionsRepository.GetByCountry(_thisCountrysInfo.Iso3166Country.Alpha3).ToList();
            Debug.Log("_allElectionsEverForThisCountry.Count = " + _allElectionsEverForThisCountry.Count);
            
            
            //Setting this to a dummy value in case it is accessed before an election/regime has happened.
            _currentRulingPartyElectionData = ElectionEntity.GetEmptyElectionEntity(_thisCountrysInfo.Iso3166Country);
            _countryInformationIsSet = true;
        }
        
        private void Update()
        {
            if (!_countryInformationIsSet) return;

            //todo: Optimize this, now it runs far too often.
            if (CurrentYearIsElectionYear())
            {
                UpdateRulingParty();
                gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewParty(_currentRulingPartyElectionData);    
            }
        }
        
        public ElectionEntity GetCurrentRulingParty()
        {
            return _currentRulingPartyElectionData;
        }
        

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