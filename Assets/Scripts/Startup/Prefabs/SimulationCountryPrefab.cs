using DefaultNamespace;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using UnityEngine;

namespace Startup_Scripts
{
    public class SimulationCountryPrefab : AbstractCountryPrefab
    {
        private AbstractCountryPrefab component;

        public SimulationCountryPrefab(AbstractCountryPrefab component)
        {
            this.component = component;
        }
        
        
        public override void Init(CountryInformationReference spawnersCountryInfo)
        {
            component.Init(spawnersCountryInfo);
            
            GetParentGameObject().GetComponent<CountryElectionHandler>().Init(spawnersCountryInfo);
        }
        
        public override GameObject GetParentGameObject()
        {
            return component.GetParentGameObject();
        }
    }
}
