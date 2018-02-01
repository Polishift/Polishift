using Dataformatter.Data_accessing.Filters;
using Dataformatter.Data_accessing.Repositories;
using DefaultNamespace.Map_Displaying.UI;
using Repository;
using UnityEngine;


namespace Startup_Scripts
{
    public abstract class CountriesSpawner : MonoBehaviour
    {
        public DefaultCountryPrefab OriginalCountryPrefab;

        private void Awake()
        {
            RepositoryHub.Init();

            EuropeFilter europeFilter = new EuropeFilter();

            foreach (var currentCountry in RepositoryHub.Iso3166Countries)
            {
                if (europeFilter.EuropeanSet.Contains(currentCountry.Alpha3))
                {
                    DefaultCountryPrefab countryPrefab = Instantiate(OriginalCountryPrefab, Vector3.zero, transform.rotation);

                    InitializeGivenCountry(currentCountry, countryPrefab);
                }
            }
            
            //Awakening the popupcreator, which needs to have all countries spawned.
            PopupCreator.Init();
        }

        protected abstract void InitializeGivenCountry(Iso3166Country isoCountry, AbstractCountryPrefab countryPrefab);
    }
}