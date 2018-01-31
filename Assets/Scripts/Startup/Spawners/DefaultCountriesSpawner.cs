using Dataformatter.Data_accessing.Repositories;
using Map_Displaying.Reference_Scripts;

namespace Startup_Scripts
{
    public class DefaultCountriesSpawner : CountriesSpawner
    {
        protected override void InitializeGivenCountry(Iso3166Country isoCountry, AbstractCountryPrefab countryPrefab)
        {
            var gameObjectOfCountry = countryPrefab.gameObject;
            var simulationCountryPrefab = gameObjectOfCountry.AddComponent<SimulationCountryPrefabDecorator>();   
            
            simulationCountryPrefab.SetDecoratorVariables(countryPrefab, gameObjectOfCountry);
            simulationCountryPrefab.Initialize(isoCountry);
        }
    }
}