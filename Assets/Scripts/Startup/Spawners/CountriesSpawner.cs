using Dataformatter.Data_accessing.Filters;
using Dataformatter.Data_accessing.Repositories;
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
                //europeFilter.EuropeanSet.Contains(currentCountry.Alpha3)
                
                if (currentCountry.Alpha3.Equals("DEU"))
                {
                    //Init 1
                    DefaultCountryPrefab countryPrefab = Instantiate(OriginalCountryPrefab, Vector3.zero, transform.rotation);
                    
                    //Init 2 calls the base, too
                    InitializeGivenCountry(currentCountry, countryPrefab);
                }
            }
        }

        protected abstract void InitializeGivenCountry(Iso3166Country isoCountry, AbstractCountryPrefab countryPrefab);
    }
}