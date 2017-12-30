using System;
using Dataformatter.Data_accessing.Repositories;
using UnityEngine;

namespace Startup_Scripts
{
    public abstract class CountryPrefabDecorator : AbstractCountryPrefab
    {
        protected AbstractCountryPrefab ParentPrefab;
        protected GameObject ThisCountriesGameObject;

        public abstract void SetDecoratorVariables(AbstractCountryPrefab parentPrefab, GameObject countriesGameObject);
        public abstract override void Initialize(Iso3166Country isoCountry);
    }
}