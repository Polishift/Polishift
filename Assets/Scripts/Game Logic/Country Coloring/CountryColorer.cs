using System;
using System.Linq;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    public class CountryColorer : MonoBehaviour
    {
        private CountryInformationReference _thisCountrysInfo;
        private ElectionEntity[] electionsForThisCountry;
        
        private void Start()
        {
            _thisCountrysInfo = gameObject.GetComponent<CountryInformationReference>();
            Debug.Log("_thisCountrysInfo.Iso3166Country.Alpha3 = " + _thisCountrysInfo.Iso3166Country.Alpha3);
            
            electionsForThisCountry =
                RepositoryHub.ElectionsRepository.GetByCountry(_thisCountrysInfo.Iso3166Country.Alpha3);
        }

        private void Update()
        {
            var currentYear = YearCounter.GetCurrentYear();
            var currentYearsElection = electionsForThisCountry.Where(e => e.Year == currentYear).ToList();

            var rulingPoliticalParty = currentYearsElection.OrderBy(p => p.TotalVotePercentage).First();
            var rulingPoliticalPartyFamily = rulingPoliticalParty.PartyClassification.ToLower();
            
            var colorForRulingParty = PoliticalFamilyColors.ColorPerFamily[rulingPoliticalPartyFamily];

            gameObject.GetComponent<MeshRenderer>().material.color = colorForRulingParty;
        }
    }
}