using DefaultNamespace;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using Repository;
using UnityEngine;


namespace Startup_Scripts
{
    public class CountriesSpawner : MonoBehaviour
    {
        public CountryPrefab OriginalCountryPrefab;

        private void Awake()
        {
            //Probly oughta make the repohub a static gameObject of its own
            RepositoryHub.Init();

            foreach (var currentCountry in RepositoryHub.Iso3166Countries)
            {
                CountryInformationReference countryInformationReference = new CountryInformationReference(currentCountry);
                CountryPrefab cloneForCurrentCountry = Instantiate(OriginalCountryPrefab, Vector3.zero, transform.rotation);

                cloneForCurrentCountry.Init(countryInformationReference);
            }
        }
    }
}