using Dataformatter.Data_accessing.Repositories;
using DefaultNamespace;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using UnityEngine;

namespace Startup_Scripts
{
    //Decorator
    public class SimulationCountryPrefabDecorator : CountryPrefabDecorator
    {
        public override void SetDecoratorVariables(AbstractCountryPrefab parentPrefab, GameObject countriesGameObject)
        {
            base.ParentPrefab = parentPrefab;
            base.ThisCountriesGameObject = countriesGameObject;
        }
        
        
        public override void Initialize(Iso3166Country isoCountry)
        {
            base.ParentPrefab.Initialize(isoCountry);            
            
            //We need to pass in a reference to the country gameObject and use THAT instead.
            base.ThisCountriesGameObject.GetComponent<CountryInformationReference>().Init(isoCountry);
            base.ThisCountriesGameObject.GetComponent<SimulationRulerHandler>().Init();
        }
    }
}
