using System.ComponentModel;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Dataprocessing.Processors;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Datamodels;


using UnityEngine;

namespace Map_Displaying.Reference_Scripts
{
    public static class RepositoryHub
    {
        public static Iso3166Country[] Iso3166Countries;
        public static CountryBordersRepository CountryBordersRepository; 

        public static void Init()
        {
            ParseCountries();

            Iso3166Repository.InitializeCollection();    
            Iso3166Countries = Iso3166Repository.GetCollection();

            CountryBordersRepository = new CountryBordersRepository();
        }

        //Add other parsers here
        private static void ParseCountries()
        {
            Dataformatter.Paths.SetProcessedDataFolder(@"C:\Users\gover\OneDrive\Documenten\Minor\ProcessedData\");		
            Dataformatter.Paths.SetRawDataFolder(@"C:\Users\gover\OneDrive\Documenten\Minor\ProcessedData\CountryInformation\");		
 		
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