using System.Collections.Generic;
using Dataformatter.Datamodels;

namespace Dataformatter.Data_accessing.Factories.ModelFactories
{
    class PartyClassificationModelFactory : ICsvModelFactory<PartyClassificationModel>
    {
        private const int CountryColumnIndex = 0;
        private const int ClassificationColumnIndex = 12;
        private const int PartynameColumnIndex = 7;
        private const int PartyIdColumnIndex = 5;

        public PartyClassificationModel Create(List<string> csvDataRow)
        {
            return new PartyClassificationModel
            {
                CountryName = csvDataRow[CountryColumnIndex],
                Id = int.Parse(csvDataRow[PartyIdColumnIndex]),
                Classification = csvDataRow[ClassificationColumnIndex],
                Name = csvDataRow[PartynameColumnIndex]
            };
        }
    }
}