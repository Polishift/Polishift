using System;
using System.Linq;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Filters;
using Dataformatter.Data_accessing.Repositories;
using UnityEngine;

namespace Repository
{
    public static class RepositoryHub
    {
        public static Iso3166Country[] Iso3166Countries;
        public static CountryBordersRepository CountryBordersRepository;
        public static ElectionsRepository ElectionsRepository;
        public static DictatorShipsRepository DictatorShipsRepository;
        
        public static WarRepository WarRepository;
        public static GdpPerCapitaRepository GdpPerCapitaRepository;
        public static GdpTotalRepository GdpTotalRepository;
        public static PopulationRepository PopulationRepository;
        public static TvRepository TvRepository;
        public static ReligionRepository ReligionRepository;
        public static InterestRepository InterestRepository;
        
        public static void Init()
        {
            //Parsing
            SetDataPaths();

            //Countries
            //ParseCountryBorders();

            Iso3166Countries = Iso3166Repository.GetCollection();
            
            CountryBordersRepository = new CountryBordersRepository();
            ElectionsRepository = new ElectionsRepository();
            DictatorShipsRepository = new DictatorShipsRepository();
            WarRepository = new WarRepository();
            GdpPerCapitaRepository = new GdpPerCapitaRepository();
            GdpTotalRepository = new GdpTotalRepository();
            PopulationRepository = new PopulationRepository();
            TvRepository = new TvRepository();
            ReligionRepository = new ReligionRepository();
            InterestRepository = new InterestRepository();
            
            Debug.Log("Amount of dictators: " + DictatorShipsRepository.GetAll().Length);
        }

        private static void SetDataPaths()
        {
            Dataformatter.Paths.SetProcessedDataFolder(@"E:\Hogeschool\Polishift Organization\ProcessedData\");
            Dataformatter.Paths.SetRawDataFolder(@"E:\Hogeschool\Polishift Organization\ProcessedData\CountryInformation");
        }

        private static void ParseCountryBorders()
        {
            var geoModelFactory = new CountryGeoModelFactory();
            var countryGeoJsonPath = Dataformatter.Paths.RawDataFolder;

            var countryBorderModels = JsonToModel<CountryGeoModel>.ParseJsonDirectoryToModels(countryGeoJsonPath,
                geoModelFactory,
                "*.geo.json");

            var countryBordersProcessor = new CountryBordersProcessor();
            countryBordersProcessor.SerializeDataToJson(countryBorderModels);
        }
    }
}