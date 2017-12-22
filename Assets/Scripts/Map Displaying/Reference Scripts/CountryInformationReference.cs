using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Repositories;
using Repository;
using UnityEngine;

namespace Map_Displaying.Reference_Scripts
{
    //This class will eventuall store references to all relevant datasources for a particular country in a given year.
    public class CountryInformationReference : MonoBehaviour
    {
        public Iso3166Country Iso3166Country;
        
        public List<ElectionEntity> AllElectionsEverForThisCountry;
        public List<DictatorshipEntity> AllDictatorshipsEverForThisCountry;
        public List<WarEntity> AllWarsEverForThisCountry;
        public List<GdpPerCapitaEntity> AllGdpPerCapitaEverForThisCountry;
        public List<GdpTotalEntity> AllGdpTotalEverForThisCountry;
        public List<PopulationEntity> AllPopulationEverForThisCountry;
        public List<TvEntity> AllTvEverForThisCountry;
        public List<ReligionEntity> AllReligionEverForThisCountry;
        public List<InterestEntity> AllInterestEverForThisCountry;
        

        public CountryInformationReference(Iso3166Country iso3166Country)
        {
            Iso3166Country = iso3166Country;

            AllElectionsEverForThisCountry = RepositoryHub.ElectionsRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllDictatorshipsEverForThisCountry = RepositoryHub.DictatorShipsRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllWarsEverForThisCountry = RepositoryHub.WarRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllGdpPerCapitaEverForThisCountry = RepositoryHub.GdpPerCapitaRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllGdpTotalEverForThisCountry = RepositoryHub.GdpTotalRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllPopulationEverForThisCountry = RepositoryHub.PopulationRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllTvEverForThisCountry = RepositoryHub.TvRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllReligionEverForThisCountry = RepositoryHub.ReligionRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
            AllInterestEverForThisCountry = RepositoryHub.InterestRepository.GetByCountry(Iso3166Country.Alpha3).ToList();
        }

        public Dictionary<string, bool> CreatePredictionFactors(int previousYear, int currentYear)
        {
            var lastYearsTotalGDP = AllGdpTotalEverForThisCountry.First(g => g.Year == previousYear).Total;
            var currentTotalGDP = AllGdpTotalEverForThisCountry.First(g => g.Year == currentYear).Total;
            var lastYearsPerCapitaGDP = AllGdpPerCapitaEverForThisCountry.First(g => g.Year == previousYear).Total;
            var currentPerCapitaGDP = AllGdpPerCapitaEverForThisCountry.First(g => g.Year == currentYear).Total;
            
            return new Dictionary<string, bool>()
            {
                { "hasHadDictatorships", AllDictatorshipsEverForThisCountry.Any() },
                { "wasEverAtWar", AllWarsEverForThisCountry.Any() },
                { "highInterestRate", AllInterestEverForThisCountry.First(i => i.Year == currentYear).Value > 3 },
                { "lowInterestRate", AllInterestEverForThisCountry.First(i => i.Year == currentYear).Value < 1 },
                { "HigherTotalGdpThanLastYear", lastYearsTotalGDP < currentTotalGDP },
                { "LowerTotalGdpThanLastYear", lastYearsTotalGDP > currentTotalGDP },
                { "HigherPerCapitaGdpThanLastYear", lastYearsPerCapitaGDP < currentPerCapitaGDP },
                { "LowerPerCapitaGdpThanLastYear", lastYearsPerCapitaGDP > currentPerCapitaGDP }
            };
        }
    }
}