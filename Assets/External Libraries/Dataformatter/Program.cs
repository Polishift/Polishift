using System;
using Dataformatter.Datamodels;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;
using Dataformatter.Data_accessing.Repositories;

namespace Dataformatter
{
    internal static class Program
    {
        private static void Main()
        {
            #region ParseCode

            //            const string electionsCsvLocation = "Datasources/Political/ElectionResults/election_data.csv";
            //            const string partyClassificationCsvLocation = "Datasources/Political/PartyClassification/classificationData.csv";
            //            const string turnoutCsvLocation = "Datasources/Political/Turnout/turnout_data.csv";
            //
            //            ICsvModelFactory<ConstituencyElectionModel> modelFactory =
            //                new ConstituencyElectionModelFactory();
            //
            //            var allItemsAsModels = CsvToModel<ConstituencyElectionModel>.ParseAllCsvLinesToModels(
            //                electionsCsvLocation, modelFactory);
            //
            //            var processor = new ElectionsProcessor();
            //            processor.SerializeDataToJson(allItemsAsModels);

            #endregion

            #region Repositry talking

            //var repo = new ElectionsRepository();
            //foreach (var entity in repo.GetByCountry("NLD"))
            //{
                //Console.WriteLine(entity);
            //}

            #endregion


            #region Parsing country borders

            const string countryBorderDirectory = "ProcessedData/CountryInformation/";
            IJsonModelFactory<CountryGeoModel> countryGeoModelFactory = new CountryGeoModelFactory();
            var processor = new CountryBordersProcessor();

            var allCountryGeoModels =
                JsonToModel<CountryGeoModel>.ParseJsonDirectoryToModels(countryBorderDirectory, countryGeoModelFactory,
                    "*.geo.json");
            processor.SerializeDataToJson(allCountryGeoModels);

            CountryBordersRepository countryBordersRepo = new CountryBordersRepository();
            foreach (var country in countryBordersRepo.GetAll())
            {
                Console.WriteLine("Countrycode: " + country.CountryCode);    
            }

            #endregion

            Console.WriteLine("All done!");
        }
    }
}