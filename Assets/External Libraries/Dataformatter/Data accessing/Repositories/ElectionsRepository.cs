using System.Collections.Generic;
using System.Linq;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;

namespace Dataformatter.Data_accessing.Repositories
{
    public class ElectionsRepository : IRepository<ElectionEntity>
    {

        //Keeping one static reference instead of recalling the parser means less GC work :)
        private static readonly Dictionary<string, ElectionEntity[]> AllElectionsByCountry =
            JsonReader<ElectionEntity>.ParseJsonToListOfObjects(EntityNames.Election);

        public ElectionEntity[] GetAll()
        {
            var result = new List<ElectionEntity>();
            foreach (var keyValuePair in AllElectionsByCountry)
            {
                result.AddRange(keyValuePair.Value);
            }
            return result.ToArray();
        }

        public ElectionEntity[] GetByCountry(string countryCode)
        {
            return AllElectionsByCountry[countryCode]; 
        }

        public ElectionEntity[] GetByYear(int year)
        {
            var result = new List<ElectionEntity>();
            foreach (var keyValuePair in AllElectionsByCountry)
            {
                result.AddRange(keyValuePair.Value);
            }
            return result.Where(e => e.Year == year).ToArray();
        }
    }
}