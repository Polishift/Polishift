using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        //TODO FIX THIS AND INTEREST
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
            _allWarsEverForThisCountry = RepositoryHub.WarRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allGdpPerCapitaEverForThisCountry = RepositoryHub.GdpPerCapitaRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allGdpTotalEverForThisCountry = RepositoryHub.GdpTotalRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allPopulationEverForThisCountry = RepositoryHub.PopulationRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allTvEverForThisCountry = RepositoryHub.TvRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allReligionEverForThisCountry = RepositoryHub.ReligionRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            _allInterestEverForThisCountry = RepositoryHub.InterestRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
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

        private int BoolToInt(bool b)
        {
            return b ? 1 : 0;
        }
    }
}