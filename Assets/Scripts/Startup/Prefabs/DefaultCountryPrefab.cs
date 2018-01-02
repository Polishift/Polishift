using Dataformatter.Data_accessing.Repositories;
using DefaultNamespace;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using UnityEngine;
using UnityEngine.TestTools;

namespace Startup_Scripts
{
    //The default prefab implements AbstractCountryPrefab, but is not a decorator.
    public class DefaultCountryPrefab : AbstractCountryPrefab
    {
        public override void Initialize(Iso3166Country isoCountry)
        {
            Debug.Log("Initialzing default prefab");
            
            gameObject.name = isoCountry.Name;
            gameObject.GetComponent<CountryInformationReference>().Init(isoCountry);

            //Making sure the pivot == the center of the mesh for scaling purposes later
            gameObject.AddComponent<PivotOffsetter>();

            //Meshes can only be generated after the Country Information is known, obviously
            gameObject.GetComponent<MeshesGenerator>().GenerateMeshes();
        }
    }
}