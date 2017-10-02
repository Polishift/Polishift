using Assets.Data.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Data.Factories
{
    class ConstituencyElectionModelFactory : IModelFactory<ConstituencyElectionModel>
    {
        private const int COUNTRY_COLUMN_INDEX = 2;
        private const int YEAR_COLUMN_INDEX = 4;
        private const int PARTYNAME_COLUMN_INDEX = 10;
        private const int CANDIDATE_COLUMN_INDEX = 12;
        private const int VOTEFRACTION_COLUMN_INDEX = 19;
        private const int SEATSGAINED_COLUMN_INDEX = 31;


        public ConstituencyElectionModel Create(List<string> csvDataRow)
        {
            return new ConstituencyElectionModel
            {
                CountryName = csvDataRow[COUNTRY_COLUMN_INDEX],
                Year = Int32.Parse(csvDataRow[YEAR_COLUMN_INDEX]),
                PartyName = csvDataRow[PARTYNAME_COLUMN_INDEX],
                CandidateName = csvDataRow[CANDIDATE_COLUMN_INDEX],
                VoteFraction = Double.Parse(csvDataRow[VOTEFRACTION_COLUMN_INDEX]),
                SeatsGained  = Int32.Parse(csvDataRow[SEATSGAINED_COLUMN_INDEX])
            };
        }
    }
}
