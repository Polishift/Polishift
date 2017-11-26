using DefaultNamespace;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using UnityEngine;

namespace Startup_Scripts
{
    [RequireComponent(typeof(CountryInformationReference), typeof(MeshesGenerator))]
    public class CountryPrefab : MonoBehaviour
    {
        public void Init(CountryInformationReference spawnersCountryInfo)
        {
            gameObject.name = spawnersCountryInfo.Iso3166Country.Name;            
            
            gameObject.GetComponent<CountryInformationReference>().Iso3166Country = spawnersCountryInfo.Iso3166Country;
            gameObject.GetComponent<CountryElectionHandler>().Init();
            
            //Making sure the pivot == the center of the mesh for scaling purposes later
            gameObject.AddComponent<PivotOffsetter>();
            
            //Meshes can only be generated after the Country Information is known, obviously
            gameObject.GetComponent<MeshesGenerator>().GenerateMeshes();
        }
    }
}
