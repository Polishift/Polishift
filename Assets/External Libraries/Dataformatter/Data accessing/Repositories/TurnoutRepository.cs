using System.Collections.Generic;
using System.Linq;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;

namespace Dataformatter.Data_accessing.Repositories
{
    public class TurnoutRepository : IRepository<TurnoutEntity>
    {
        //Keeping one static reference instead of recalling the parser means less GC work :)
        private static readonly Dictionary<string, TurnoutEntity[]> AllTurnoutByCountry =
            JsonReader<TurnoutEntity>.ParseJsonToListOfObjects(EntityNames.Turnout);

        public TurnoutEntity[] GetAll()
        {
            var result = new List<TurnoutEntity>();
            foreach (var keyValuePair in AllTurnoutByCountry)
            {
                result.AddRange(keyValuePair.Value);
            }
            return result.ToArray();
        }

        public TurnoutEntity[] GetByCountry(string countryCode)
        {
            return AllTurnoutByCountry[countryCode];
        }

        public TurnoutEntity[] GetByYear(int year)
        {
            var result = new List<TurnoutEntity>();
            foreach (var keyValuePair in AllTurnoutByCountry)
            {
                result.AddRange(keyValuePair.Value);
            }
            return result.Where(e => e.Year == year).ToArray();
        }
    }
}