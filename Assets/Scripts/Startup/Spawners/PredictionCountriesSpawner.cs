using Dataformatter.Data_accessing.Repositories;
using Map_Displaying.Reference_Scripts;
using UnityEngine;

namespace Startup_Scripts
{
    public class PredictionCountriesSpawner : CountriesSpawner
    {
        protected override void InitializeGivenCountry(Iso3166Country isoCountry, AbstractCountryPrefab countryPrefab)
        {
            var gameObjectOfCountry = countryPrefab.gameObject;
            var predictionCountryPrefab = gameObjectOfCountry.AddComponent<PredictionCountryPrefabDecorator>();   
            
            predictionCountryPrefab.SetDecoratorVariables(countryPrefab, gameObjectOfCountry);
            predictionCountryPrefab.Initialize(isoCountry);
        }
    }
}