using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;


namespace Startup_Scripts
{
    public class CountriesSpawner : MonoBehaviour
    {
        public CountryPrefab _originalCountryPrefab;

        private void Awake()
        {
            //Probly oughta make the repohub a static gameObject of its own
            RepositoryHub.Init();

            foreach (var currentCountry in RepositoryHub.Iso3166Countries)
            {
                if (currentCountry.Alpha3 == "ITA")
                {
                    CountryPrefab cloneForCurrentCountry = (CountryPrefab) Instantiate(_originalCountryPrefab,
                                                                                       transform.position,
                                                                                       transform.rotation);

                    CountryInformationReference countryInformationReference = new CountryInformationReference(currentCountry);
                    cloneForCurrentCountry.Init(countryInformationReference);
                }
            }
        }
    }
}