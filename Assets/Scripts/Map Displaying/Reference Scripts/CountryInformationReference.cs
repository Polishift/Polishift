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
    [RequireComponent(typeof(CountryElectionHandler))]
    public class CountryInformationReference : MonoBehaviour
    {
        public Iso3166Country Iso3166Country;

        public List<ElectionEntity> AllElectionsEverForThisCountry = new List<ElectionEntity>();
        public List<DictatorshipEntity> AllDictatorshipsEverForThisCountry = new List<DictatorshipEntity>();

        private readonly List<WarEntity> _allWarsEverForThisCountry = new List<WarEntity>();
        private readonly List<GdpPerCapitaEntity> _allGdpPerCapitaEverForThisCountry = new List<GdpPerCapitaEntity>();
        private readonly List<GdpTotalEntity> _allGdpTotalEverForThisCountry = new List<GdpTotalEntity>();
        private List<PopulationEntity> _allPopulationEverForThisCountry = new List<PopulationEntity>();
        private List<TvEntity> _allTvEverForThisCountry = new List<TvEntity>();
        private List<ReligionEntity> _allReligionEverForThisCountry = new List<ReligionEntity>();
        private readonly List<InterestEntity> _allInterestEverForThisCountry = new List<InterestEntity>();


        public CountryInformationReference(Iso3166Country iso3166Country)
        {
            Iso3166Country = iso3166Country;

            AllElectionsEverForThisCountry = RepositoryHub.ElectionsRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllDictatorshipsEverForThisCountry = RepositoryHub.DictatorShipsRepository.GetByCountry(Iso3166Country.Alpha3).ToList();

            try
            {
                _allWarsEverForThisCountry = RepositoryHub.WarRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
                _allGdpPerCapitaEverForThisCountry = RepositoryHub.GdpPerCapitaRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
                _allGdpTotalEverForThisCountry = RepositoryHub.GdpTotalRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
                _allPopulationEverForThisCountry = RepositoryHub.PopulationRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
                _allTvEverForThisCountry = RepositoryHub.TvRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
                _allReligionEverForThisCountry = RepositoryHub.ReligionRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
                _allInterestEverForThisCountry = RepositoryHub.InterestRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            }
            catch (Exception e)
            {
                //Debug.Log("Country " + Iso3166Country.Alpha3 + " constructor exception: " + e);
            }
        }

        //Do the year loop in here instead
        public List<Record> CreateTrainingSetForThisCountry()
        {
            var classificationRecordsForEachYear = new List<Record>();
            var countriesElectionHandler = gameObject.GetComponent<CountryElectionHandler>();


            for (int currentYear = YearCounter.MinimumYear; currentYear < YearCounter.MaximumYear; currentYear++)
            {
                int previousYear;

                if (currentYear != YearCounter.MinimumYear)
                    previousYear = currentYear - 1;
                else
                    previousYear = currentYear;

                //Retrieving the currently ruling family
                countriesElectionHandler.CurrentYear = currentYear;
                var currentCountrysPoliticalFamily = countriesElectionHandler.CurrentCountryRuler.PartyClassification;

                classificationRecordsForEachYear.Add(new Record(currentCountrysPoliticalFamily, GetPredictorFactors(previousYear, currentYear)));
            }
            return classificationRecordsForEachYear;
        }

        //Todo temporary so we can test the predictor without having to give in future (estimated) values
        public Dictionary<string, bool> GetPredictorFactors(int previousYear, int currentYear)
        {
            var lastYearsTotalGDP = _allGdpTotalEverForThisCountry.First(g => g.Year == previousYear).Total;
            var currentTotalGDP = _allGdpTotalEverForThisCountry.First(g => g.Year == currentYear).Total;
            var lastYearsPerCapitaGDP = _allGdpPerCapitaEverForThisCountry.First(g => g.Year == previousYear).Total;
            var currentPerCapitaGDP = _allGdpPerCapitaEverForThisCountry.First(g => g.Year == currentYear).Total;

            var factorDict = new Dictionary<string, bool>() {{"hasHadDictatorships", AllDictatorshipsEverForThisCountry.Any()}, {"wasEverAtWar", _allWarsEverForThisCountry.Any()}, {"highInterestRate", _allInterestEverForThisCountry.First(i => i.Year == currentYear).Value > 3}, {"lowInterestRate", _allInterestEverForThisCountry.First(i => i.Year == currentYear).Value < 1}, {"HigherTotalGdpThanLastYear", lastYearsTotalGDP < currentTotalGDP}, {"LowerTotalGdpThanLastYear", lastYearsTotalGDP > currentTotalGDP}, {"HigherPerCapitaGdpThanLastYear", lastYearsPerCapitaGDP < currentPerCapitaGDP}, {"LowerPerCapitaGdpThanLastYear", lastYearsPerCapitaGDP > currentPerCapitaGDP}};

            return factorDict;
        }
    }
}