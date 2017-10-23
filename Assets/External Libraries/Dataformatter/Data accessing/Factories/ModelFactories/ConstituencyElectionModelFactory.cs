using System.Collections.Generic;
using System.Globalization;
using Dataformatter.Datamodels;

namespace Dataformatter.Data_accessing.Factories.ModelFactories
{
    class ConstituencyElectionModelFactory : ICsvModelFactory<ConstituencyElectionModel>
    {
        //Missingcodes
        private readonly HashSet<string> _invalidValueCodes = new HashSet<string> { "-990", "-992", "-994"};

        //Indexes
        private const int YearColumnIndex = 4;
        private const int CountryColumnIndex = 2;
        private const int ConstituencyColumnIndex = 7;

        private const int PartynameColumnIndex = 10;
        private const int CandidateColumnIndex = 12;

        private const int TotalvotesamountColumnIndex = 15;
        private const int PartyvotesamountColumnIndex = 20;
        
        private const int SecondRoundPartyvotesamountColumnIndex = 29;
        private const int SeatsgainedColumnIndex = 31;

        public ConstituencyElectionModel Create(List<string> csvDataRow)
        {
            var filteredCsvDataRow = FilterInvalidValues(csvDataRow);

            return new ConstituencyElectionModel
            {
                CountryName = filteredCsvDataRow[CountryColumnIndex],
                ConstituencyName = filteredCsvDataRow[ConstituencyColumnIndex],
                Year = int.Parse(filteredCsvDataRow[YearColumnIndex]),
                PartyName = filteredCsvDataRow[PartynameColumnIndex],
                CandidateName = filteredCsvDataRow[CandidateColumnIndex],
                TotalAmountOfVotes = int.Parse(filteredCsvDataRow[TotalvotesamountColumnIndex], CultureInfo.InvariantCulture),
                AmountOfPartyVotes = double.Parse(filteredCsvDataRow[PartyvotesamountColumnIndex], CultureInfo.InvariantCulture),
                SecondRoundAmountOfPartyVotes = int.Parse(filteredCsvDataRow[SecondRoundPartyvotesamountColumnIndex],
                    CultureInfo.InvariantCulture),
                SeatsGained = int.Parse(filteredCsvDataRow[SeatsgainedColumnIndex])
            };
        }

        private List<string> FilterInvalidValues(List<string> csvDataRow)
        {
            var filteredDataRow = csvDataRow;

            for(var i = 0; i < csvDataRow.Count; i++)
            {
                if(i == CandidateColumnIndex && IsInvalidValue(csvDataRow[i])) 
                {
                    filteredDataRow[i] = "Unknown";
                }
                 //Anything but candidate names that can also be invalid is represented as a number, hence 0 instead of "Unknown"
                else if(IsInvalidValue(csvDataRow[i]))
                {
                    filteredDataRow[i] = "0";
                }
            }
            return filteredDataRow;
        }

        private bool IsInvalidValue(string value)
        {
            return _invalidValueCodes.Contains(value);
        }
    }
}