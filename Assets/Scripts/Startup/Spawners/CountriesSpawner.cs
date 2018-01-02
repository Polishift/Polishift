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
             RepositoryHub.Init();
 
             EuropeFilter europeFilter = new EuropeFilter();
 
             foreach (var currentCountry in RepositoryHub.Iso3166Countries)
             {
                 if(europeFilter.EuropeanSet.Contains(currentCountry.Alpha3))
                 //if (currentCountry.Alpha3.Equals("BGR"))
                 {
                     DefaultCountryPrefab countryPrefab = Instantiate(OriginalCountryPrefab, Vector3.zero, transform.rotation);
                     
                     InitializeGivenCountry(currentCountry, countryPrefab);
                 }
             }
         }
 
         protected abstract void InitializeGivenCountry(Iso3166Country isoCountry, AbstractCountryPrefab countryPrefab);
     }
 }