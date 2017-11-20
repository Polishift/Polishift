using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using UnityEngine;

namespace Startup_Scripts
{
    [RequireComponent(typeof(CountryInformationReference), typeof(MeshesGenerator))]
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
            /*
            //Meshes can only be generated after the Country Information is known, obviously
            gameObject.GetComponent<MeshesGenerator>().GenerateMeshes();
            */
            
            gameObject.name = spawnersCountryInfo.Iso3166Country.Name;            
            gameObject.GetComponent<CountryInformationReference>().Iso3166Country = spawnersCountryInfo.Iso3166Country;

            /*
            var meshesForOurCountrysPolygons = MeshCreator.GetMeshPerPolygon(GetComponent<CountryInformationReference>());
            
            
            //Making the nonoutline mesh a child as well
            var mesh = meshesForOurCountrysPolygons[0];
            GameObject newChildObject = new GameObject() {name = "Non outline mesh child"};
            newChildObject.transform.parent = gameObject.transform;

            newChildObject.AddComponent<MeshFilter>();
            newChildObject.AddComponent<MeshRenderer>();

            newChildObject.GetComponent<MeshFilter>().mesh = mesh;
            
            
            newChildObject.AddComponent<TestChildCreator>();
            */
        }
    }
}
