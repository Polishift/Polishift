using System;
using System.Collections.Generic;
using System.Linq;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Factories.EntityFactories;

namespace Dataformatter.Dataprocessing.Processors
{
    class ElectionsProcessor : AbstractDataProcessor<ConstituencyElectionModel,
        ElectionEntity>
    {
        private readonly DefaultElectionEntityFactory _electionEntityFactory = new DefaultElectionEntityFactory();
        private readonly HashSet<string> _knownConstituencies = new HashSet<string>();

        private readonly Dictionary<Tuple<string, int>, int> _totalAmountOfVotesPerCountryAndYear =
            new Dictionary<Tuple<string, int>, int>();

        private readonly Dictionary<Tuple<string, int>, ElectionEntity> _electionsPerParty =
            new Dictionary<Tuple<string, int>, ElectionEntity>();

        private readonly Dictionary<string, int> _specialPartyNamesAndAmount = new Dictionary<string, int>
        {
            {"Independent", 0},
            {"independent", 0},
            {"Independents", 0},
            {"Blank", 0}
        };


        public override void SerializeDataToJson(List<ConstituencyElectionModel> rawModels)
        {
            var previousRowsYear = int.MinValue;

            for (var i = 0; i < rawModels.Count; i++)
            {
                var currentRowsYear = rawModels[i].Year;

                //Checking for independent/blank parties
                if (IsSpecialParty(rawModels[i]))
                {
                    var artificialPartyName = GetArtificalPartyName(rawModels[i]);
                    rawModels[i].PartyName = artificialPartyName;
                }
                HandlePartyRow(rawModels[i]);

                AddCurrentConstituencyVotesToTotal(rawModels[i]);
                SumConstituencyPartyVotes(rawModels[i]);

                //Known constituencies are used to ensure that the summation of all votes is only done once per constituency, per election.
                EmptyKnownConstituenciesEachYear(previousRowsYear, currentRowsYear);

                previousRowsYear = rawModels[i].Year;
            }
            //Calculating actual vote percentages
            CalculateVotePercentages();

            WriteEntitiesToJson(EntityNames.Election, _electionsPerParty.Values.ToList());
        }


        private bool IsSpecialParty(ConstituencyElectionModel currentRawElectionModel)
        {
            return _specialPartyNamesAndAmount.ContainsKey(currentRawElectionModel.PartyName);
        }

        private string GetArtificalPartyName(ConstituencyElectionModel currentRawElectionModel)
        {
            var currentRowPartyName = currentRawElectionModel.PartyName;
            _specialPartyNamesAndAmount[currentRowPartyName] += 1;

            return currentRowPartyName + " " + _specialPartyNamesAndAmount[currentRowPartyName];
        }


        private void HandlePartyRow(ConstituencyElectionModel currentRawElectionModel)
        {
            var currentPartyAndYearCombination = new Tuple<string, int>(currentRawElectionModel.PartyName,
                currentRawElectionModel.Year);
            var currentRowCandidate = currentRawElectionModel.CandidateName;

            if (_electionsPerParty.ContainsKey(currentPartyAndYearCombination) == false)
            {
                //As of yet undiscovered party/year combination 
                _electionsPerParty.Add(currentPartyAndYearCombination,
                    _electionEntityFactory.Create(currentRawElectionModel));
            }
            else if (_electionsPerParty[currentPartyAndYearCombination].PartyCandidates
                         .Contains(currentRowCandidate) == false)
            {
                //Existing party/year combination, but undiscovered candidate
                _electionsPerParty[currentPartyAndYearCombination].PartyCandidates.Add(currentRowCandidate);
            }
        }

        private void AddCurrentConstituencyVotesToTotal(ConstituencyElectionModel currentRawElectionModel)
        {
            var currentConstituency = currentRawElectionModel.ConstituencyName;

            if (_knownConstituencies.Contains(currentConstituency) == false)
            {
                _knownConstituencies.Add(currentConstituency);

                var currentCountryAndYearCombination = new Tuple<string, int>(currentRawElectionModel.CountryName,
                    currentRawElectionModel.Year);

                if (_totalAmountOfVotesPerCountryAndYear.ContainsKey(currentCountryAndYearCombination))
                    _totalAmountOfVotesPerCountryAndYear[currentCountryAndYearCombination] +=
                        currentRawElectionModel.TotalAmountOfVotes;
                else
                    _totalAmountOfVotesPerCountryAndYear.Add(currentCountryAndYearCombination,
                        currentRawElectionModel.TotalAmountOfVotes);

                //do second round?
            }
        }

        private void SumConstituencyPartyVotes(ConstituencyElectionModel currentRawElectionModel)
        {
            var currentPartyAndYearCombination = new Tuple<string, int>(currentRawElectionModel.PartyName,
                currentRawElectionModel.Year);

            _electionsPerParty[currentPartyAndYearCombination].TotalAmountOfVotes +=
                (int) currentRawElectionModel.AmountOfPartyVotes;
            //do second round?
        }

        private void
            EmptyKnownConstituenciesEachYear(int previousRowsYear, int currentRowsYear) //if previous year is greater
        {
            if (currentRowsYear > previousRowsYear)
                _knownConstituencies.Clear();
        }


        private void CalculateVotePercentages()
        {
            foreach (var partyElectionResult in _electionsPerParty)
            {
                var currentElectionKey = partyElectionResult.Key;
                var currentCountry = partyElectionResult.Value.CountryName;
                var currentYear = partyElectionResult.Key.Item2;
                var currentCountryAndYear = new Tuple<string, int>(currentCountry, currentYear);

                double totalAmountOfVotesInCountry = _totalAmountOfVotesPerCountryAndYear[currentCountryAndYear];
                var amountOfVotesForCurrentParty = partyElectionResult.Value.TotalAmountOfVotes;

                if (totalAmountOfVotesInCountry > 0
                ) //This ocassionally happens with seat-based elections like Andorra's
                {
                    _electionsPerParty[currentElectionKey].TotalVotePercentage =
                        (amountOfVotesForCurrentParty / totalAmountOfVotesInCountry) * 100;
                }
            }
        }
    }
}