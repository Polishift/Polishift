using Assets.Data.Repositories.Models;
using Assets.Data_accessing.Factories;
using Assets.Dataprocessing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Dataprocessing.Processers
{
    class ElectionsProcesser : IDataProcesser<ConstituencyElectionModel>
    {
        private const string RESULT_FILE = "Data/Processed/Elections.json";


        public void SerializeDataToJSON(List<ConstituencyElectionModel> rawModels)
        {
            AbstractElectionEntityFactory electionEntityFactory = new DefaultElectionEntityFactory();
            var electionsPerParty = new Dictionary<Tuple<string, int>, ElectionEntity>();

            for (int i = 0; i < rawModels.Count(); i++)
            {
                var currentPartyAndYearCombination = new Tuple<string, int>(rawModels[i].PartyName, rawModels[i].Year);
                var currentRowCandidate = rawModels[i].CandidateName;

                if (electionsPerParty.ContainsKey(currentPartyAndYearCombination) == false)
                {
                    //As of yet undiscovered party/year combination
                    electionsPerParty.Add(currentPartyAndYearCombination, electionEntityFactory.Create(rawModels[i]));
                }
                else if (electionsPerParty[currentPartyAndYearCombination].PartyCandidates.Contains(currentRowCandidate) == false)
                {
                    //Existing party/year combination, but undiscovered candidate
                    electionsPerParty[currentPartyAndYearCombination].PartyCandidates.Add(currentRowCandidate);
                }
            }
            WriteElectionEntitiesToJSON(electionsPerParty.Values.ToList());
        }

        private void WriteElectionEntitiesToJSON(List<ElectionEntity> entities)
        {
            using (StreamWriter file = File.CreateText(RESULT_FILE))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, entities);
            }
        }
    }
}
