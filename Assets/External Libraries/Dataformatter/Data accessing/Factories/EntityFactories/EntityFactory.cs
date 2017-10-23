using System;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;

namespace Dataformatter.Data_accessing.Factories.EntityFactories
{
    public abstract class EntityFactory<I, O> where I : IModel
        where O : IEntity
    {
        public abstract O Create(I rawModel);

        protected string CreateCountryCode(string fullCountryName)
        {
            var result = Iso3166Repository.FromName(fullCountryName.ToLower()) ??
                         (Iso3166Repository.FromAlpha2(fullCountryName.ToUpper()) ??
                          Iso3166Repository.FromAlternativeName(fullCountryName.ToLower()) ??
                          Iso3166Repository.FromAlpha3(fullCountryName.ToUpper()));


            if (result != null) return result.Alpha3;
            Console.WriteLine("nothing found by " + fullCountryName);
            return new Iso3166Country("UNKNOWN", "UNKNOWN", "UNKNOWN").Alpha3;
        }
    }
}