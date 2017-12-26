using Dataformatter.Data_accessing.Filters;
using DefaultNamespace;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using Repository;
using UnityEditor;
using UnityEngine;


namespace Startup_Scripts
{
    public abstract class CountriesSpawner : MonoBehaviour
    {
        public DefaultCountryPrefab OriginalCountryPrefab;

        private void Awake()
        {
            //Probly oughta make the repohub a static gameObject of its own
            RepositoryHub.Init();

            EuropeFilter europeFilter = new EuropeFilter();

            foreach (var currentCountry in RepositoryHub.Iso3166Countries)
            {
                if (europeFilter.EuropeanSet.Contains(currentCountry.Alpha3))
                {
                    CountryInformationReference countryInformationReference = new CountryInformationReference(currentCountry);
                    DefaultCountryPrefab x = Instantiate(OriginalCountryPrefab, Vector3.zero, transform.rotation);

                    InitializeGivenCountry(countryInformationReference, x);
                }
            }
        }

        protected abstract void InitializeGivenCountry(CountryInformationReference countryInfo, AbstractCountryPrefab countryPrefab);
    }
}