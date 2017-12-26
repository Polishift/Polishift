using Map_Displaying.Reference_Scripts;

namespace Startup_Scripts
{
    public class DefaultCountriesSpawner : CountriesSpawner
    {
        protected override void InitializeGivenCountry(CountryInformationReference countryInfo, AbstractCountryPrefab countryPrefab)
        {
            var simulationCountryPrefab = new SimulationCountryPrefab(countryPrefab);
            simulationCountryPrefab.Init(countryInfo);
        }
    }
}