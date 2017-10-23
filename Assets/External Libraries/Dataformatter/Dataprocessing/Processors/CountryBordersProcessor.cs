using System.Collections.Generic;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Factories.EntityFactories;

namespace Dataformatter.Dataprocessing.Processors
{
    class CountryBordersProcessor : AbstractDataProcessor<CountryGeoModel,  
                                                          CountryBordersEntity>
    {
        private readonly DefaultCountryBordersEntityFactory _defaultCountryBordersEntityFactory = new DefaultCountryBordersEntityFactory();

        public override void SerializeDataToJson(List<CountryGeoModel> rawModels)
        {
            var countryBordersEntities = new List<CountryBordersEntity>();

            for(var i = 0; i < rawModels.Count; i++)
            {
                var currentRawModel = rawModels[i];
                countryBordersEntities.Add(_defaultCountryBordersEntityFactory.Create(currentRawModel));
            }
            WriteEntitiesToJson(EntityNames.CountryBorders, countryBordersEntities);
        }
    }
}