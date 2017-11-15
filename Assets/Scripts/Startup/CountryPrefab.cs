using Map_Displaying.Handlers;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using UnityEngine;

namespace Startup_Scripts
{
    [RequireComponent(typeof(CountryInformationReference), typeof(MeshesGenerator), typeof(OnClickHandler))]
    public class CountryPrefab : MonoBehaviour
    {
        private void Awake()
        {
        }

        private void Update()
        {
        }
        
        //Not too clean since we cant guarantee that this will be called
        public void Init(CountryInformationReference spawnersCountryInfo)
        {
            gameObject.name = spawnersCountryInfo.Iso3166Country.Name;
            
            //Adding mesh filter/renderer for the meshesGenerator to use
            gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>();
            
            
            gameObject.AddComponent<MeshesGenerator>();
            gameObject.AddComponent<OnClickHandler>();

            //Setting this prefabs' country information to be that which the spawner assigned it.
            gameObject.AddComponent<CountryInformationReference>();
            gameObject.GetComponent<CountryInformationReference>().Iso3166Country = spawnersCountryInfo.Iso3166Country;
        }
    }
}
