using System.Collections.Generic;
using System.Globalization;
using Dataformatter.Datamodels;

namespace Dataformatter.Data_accessing.Factories.ModelFactories
{
    public class TurnoutModelFactory : ICsvModelFactory<TurnoutModel>
    {
        private const int InvalidVotesColumnIndex = 9;
        private const int CountryColumnIndex = 0;
        private const int PopulationColumnIndex = 8;
        private const int RegistrationColumnIndex = 5;
        private const int TotalVotesColumnIndex = 4;
        private const int TypeColumnIndex = 1;
        private const int VapTurnoutColumnIndex = 6;
        private const int VoterTurnoutColumnIndex = 3;
        private const int VotingAgeColumnIndex = 7;
        private const int YearColumnIndex = 2;

        public TurnoutModel Create(List<string> csvDataRow)
        {
            return new TurnoutModel
            {
                InvalidVotes = double.Parse(StripOnSpaces(csvDataRow[InvalidVotesColumnIndex]),
                    CultureInfo.InvariantCulture),
                CountryName = csvDataRow[CountryColumnIndex],
                Population = int.Parse(ReplaceCommasInThousands(csvDataRow[PopulationColumnIndex]),
                    CultureInfo.InvariantCulture),
                Registration = int.Parse(ReplaceCommasInThousands(csvDataRow[RegistrationColumnIndex]),
                    CultureInfo.InvariantCulture),
                TotalVotes = int.Parse(ReplaceCommasInThousands(csvDataRow[TotalVotesColumnIndex]),
                    CultureInfo.InvariantCulture),
                Type = csvDataRow[TypeColumnIndex],
                VapTurnout =
                    double.Parse(StripOnSpaces(csvDataRow[VapTurnoutColumnIndex]), CultureInfo.InvariantCulture),
                VoterTurnout = double.Parse(StripOnSpaces(csvDataRow[VoterTurnoutColumnIndex]),
                    CultureInfo.InvariantCulture),
                VotingAge = int.Parse(ReplaceCommasInThousands(csvDataRow[VotingAgeColumnIndex]),
                    CultureInfo.InvariantCulture),
                Year = int.Parse(csvDataRow[YearColumnIndex], CultureInfo.InvariantCulture)
            };

        }

        private string StripOnSpaces(string input)
        {
            if (input.Equals(""))
                return "-1";

            var output = input.Substring(0, input.IndexOf(' '));
            return output;
        }

        private string ReplaceCommasInThousands(string input)
        {
            if (input.Equals(""))
                return "-1";

            var output = input.Replace(",", "");
            return output;
        }
    }
}