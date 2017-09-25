using Assets.Data.Factories;
using Assets.Data.Repositories.Models;
using Assets.Dataprocessing.Processers;
using Mono.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Simulation_Logic
{
    class Startup : MonoBehaviour
    {
        private void Start()
        {
            var electionsCsvLocation = "Data/Raw/election_data.csv";
            IModelFactory<ConstituencyElectionModel> constituencyElectionModelFactory = new ConstituencyElectionModelFactory();
            var allElectionLinesAsModels = CsvToModel<ConstituencyElectionModel>.ParseAllCsvLinesToModels(electionsCsvLocation,
                                                                                                          constituencyElectionModelFactory);
            ElectionsProcesser processer = new ElectionsProcesser();
            processer.SerializeDataToJSON(allElectionLinesAsModels);
        }
    }
}
