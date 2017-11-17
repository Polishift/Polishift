﻿using Map_Displaying.Reference_Scripts;
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
            gameObject.name = spawnersCountryInfo.Iso3166Country.Name;

            //Setting this prefabs' country information to be that which the spawner assigned it.
            gameObject.GetComponent<CountryInformationReference>().Iso3166Country = spawnersCountryInfo.Iso3166Country;
            
            //Meshes can only be generated after the Country Information is known, obviously
            gameObject.GetComponent<MeshesGenerator>().GenerateMeshes();
        }
    }
}
