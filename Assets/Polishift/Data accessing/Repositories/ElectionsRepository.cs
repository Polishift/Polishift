using Assets.Data.Factories;
using Assets.Data.Repositories.Models;
using Assets.Dataprocessing.CsvParsing;
using Assets.Dataprocessing.Entities;
using Mono.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Data.Repositories
{
    class ElectionsRepository : IRepository<ElectionEntity>
    {
        /*
            Read JSON file as efficiently as possible. 
            Then return deserialized Election Entities.
        */
        private const string FILE_LOCATION = "../../Data/Processed/Elections.json";

        //Keeping one static reference instead of recalling the parser means less GC work :)
        private static ElectionEntity[] allElections = JsonReader<ElectionEntity>.ParseJsonToListOfObjects(FILE_LOCATION);

        public ElectionEntity[] GetAll()
        {
            return allElections; 
        }

        public ElectionEntity[] GetByCountry(string CountryCode)
        {
            return allElections.Where(e => e.CountryCode == CountryCode).ToArray();
        }

        public ElectionEntity[] GetByYear(int year)
        {
            return allElections.Where(e => e.Year == year).ToArray();
        }
    }
}