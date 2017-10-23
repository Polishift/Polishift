using System.Collections.Generic;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;

namespace Dataformatter.Data_accessing.Repositories
{
    public class PartyClassificationRepository : IRepository<PartyClassificationEntity>
    {
        //Keeping one static reference instead of recalling the parser means less GC work :)
        private static readonly Dictionary<string, PartyClassificationEntity[]> AllPartyClassificationsByCountry =
            JsonReader<PartyClassificationEntity>.ParseJsonToListOfObjects(EntityNames.PartyClassification);

        public PartyClassificationEntity[] GetAll()
        {
            var result = new List<PartyClassificationEntity>();
            foreach (var keyValuePair in AllPartyClassificationsByCountry)
            {
                result.AddRange(keyValuePair.Value);
            }
            return result.ToArray();
        }

        public PartyClassificationEntity[] GetByCountry(string countryCode)
        {
            return AllPartyClassificationsByCountry[countryCode];
        }
    }
}