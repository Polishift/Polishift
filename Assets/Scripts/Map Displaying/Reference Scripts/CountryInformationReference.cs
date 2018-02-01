using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Repositories;
using Game_Logic;
using Game_Logic.Country_Coloring;
using NaiveBayesClassifier;
using Repository;
using UnityEngine;

namespace Map_Displaying.Reference_Scripts
{
    public class CountryInformationReference : MonoBehaviour
    {
        public Iso3166Country Iso3166Country;

        public List<ElectionEntity> AllElectionsEverForThisCountry = new List<ElectionEntity>();
        public List<DictatorshipEntity> AllDictatorshipsEverForThisCountry = new List<DictatorshipEntity>();
        public List<TurnoutEntity> AllTurnoutEverForThisCountry = new List<TurnoutEntity>();        
        public string FutureRulerClassification; 

        private List<WarEntity> _allWarsEverForThisCountry = new List<WarEntity>();
        private List<GdpPerCapitaEntity> _allGdpPerCapitaEverForThisCountry = new List<GdpPerCapitaEntity>();
        private List<GdpTotalEntity> _allGdpTotalEverForThisCountry = new List<GdpTotalEntity>();
        private List<PopulationEntity> _allPopulationEverForThisCountry = new List<PopulationEntity>();
        private List<TvEntity> _allTvEverForThisCountry = new List<TvEntity>();
        private List<ReligionEntity> _allReligionEverForThisCountry = new List<ReligionEntity>();
        private List<InterestEntity> _allInterestEverForThisCountry = new List<InterestEntity>();


        public void Init(Iso3166Country iso3166Country)
        {
            Iso3166Country = iso3166Country;

            AllElectionsEverForThisCountry = RepositoryHub.ElectionsRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllDictatorshipsEverForThisCountry = RepositoryHub.DictatorShipsRepository.GetByCountry(Iso3166Country.Alpha3).ToList();

            FutureRulerClassification = RepositoryHub.FutureRulersRepository.GetByCountry(Iso3166Country.Alpha3).First().FutureRulingPartyClassification;
            
            _allWarsEverForThisCountry = RepositoryHub.WarRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allGdpPerCapitaEverForThisCountry = RepositoryHub.GdpPerCapitaRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allGdpTotalEverForThisCountry = RepositoryHub.GdpTotalRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allPopulationEverForThisCountry = RepositoryHub.PopulationRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allTvEverForThisCountry = RepositoryHub.TvRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allReligionEverForThisCountry = RepositoryHub.ReligionRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allInterestEverForThisCountry = RepositoryHub.InterestRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllTurnoutEverForThisCountry = RepositoryHub.TurnoutRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
        }

        public Dictionary<string, int> GetPredictorFactors(int previousYear, int currentYear)
        {
            //Doing where->sum here to avoid nullpointers when the gdp is unknown for a given year
            var lastYearsTotalGDP = _allGdpTotalEverForThisCountry.Where(g => g.Year == previousYear).Sum(g => g.Total);
            var currentTotalGDP = _allGdpTotalEverForThisCountry.Where(g => g.Year == currentYear).Sum(g => g.Total);
            var lastYearsPerCapitaGDP = _allGdpPerCapitaEverForThisCountry.Where(g => g.Year == previousYear).Sum(g => g.Total);
            var currentPerCapitaGDP = _allGdpPerCapitaEverForThisCountry.Where(g => g.Year == currentYear).Sum(g => g.Total);

            var factorDict = new Dictionary<string, int>()
            {
                {"hasHadDictatorships", BoolToInt(AllDictatorshipsEverForThisCountry.Any())}, 
                {"wasEverAtWar", BoolToInt(_allWarsEverForThisCountry.Any())}, 
                {"highInterestRate", BoolToInt(_allInterestEverForThisCountry.Where(i => i.Year == currentYear).Sum(i => i.Value) > 3)}, 
                {"lowInterestRate", BoolToInt(_allInterestEverForThisCountry.Where(i => i.Year == currentYear).Sum(i => i.Value) < 1)}, 
                {"HigherTotalGdpThanLastYear", BoolToInt(lastYearsTotalGDP < currentTotalGDP)}, 
                {"LowerTotalGdpThanLastYear", BoolToInt(lastYearsTotalGDP > currentTotalGDP)},
                {"HigherPerCapitaGdpThanLastYear", BoolToInt(lastYearsPerCapitaGDP < currentPerCapitaGDP)}, 
                {"LowerPerCapitaGdpThanLastYear", BoolToInt(lastYearsPerCapitaGDP > currentPerCapitaGDP)}
            };
            return factorDict;
        }

        public string GetPrettifiedDetails()
        {
            var returnString = "";
            var currentYear = YearCounter.GetCurrentYear();

            var anyCurrentWars = _allWarsEverForThisCountry.Any(w => w.StartYear >= currentYear && w.EndYear <= currentYear);
            var curentPerCapitaGdp = _allGdpPerCapitaEverForThisCountry.FirstOrDefault(g => g.Year == currentYear);
            var currentGdpTotal = _allGdpTotalEverForThisCountry.FirstOrDefault(g => g.Year == currentYear);
            var currentPopSize = _allPopulationEverForThisCountry.FirstOrDefault(g => g.Year == currentYear);
            var currentInterest = _allInterestEverForThisCountry.FirstOrDefault(g => g.Year == currentYear);
            var currentTurnout = AllTurnoutEverForThisCountry.FirstOrDefault(g => g.Year == currentYear);
            
            
            //Quite dirty
            returnString += ("At war: " + PrettyBool(anyCurrentWars) + "\n");

            returnString += ("Current GDP Per capita: ");
            if (curentPerCapitaGdp != null)
                returnString += curentPerCapitaGdp.Total + "\n";
            else
                returnString += "unknown" + "\n";
            
            returnString += ("Current GDP Total: ");
            if(currentGdpTotal != null)
                returnString += currentGdpTotal.Total + "\n";
            else
                returnString += "unknown" + "\n";
            
            
            returnString += ("Current Population size: ");
            if(currentPopSize != null)
                returnString += currentPopSize.Value + "\n";
            else
                returnString += "unknown" + "\n";
            
            
            returnString += ("Current interest rate: ");
            if(currentInterest != null)
                returnString += currentInterest.Value.ToString("P", CultureInfo.InvariantCulture) + "\n";
            else
                returnString += "unknown" + "\n";
           
            returnString += ("Current election turnout (if any): ");
            if(currentTurnout != null)
                returnString += currentTurnout.VoterTurnout + "% \n";
            else
                returnString += "unknown" + "\n";
            
            return returnString;
        }


        private string PrettyBool(bool b)
        {
            return b ? "Yes" : "No";
        }

        private int BoolToInt(bool b)
        {
            return b ? 1 : 0;
        }
    }
}