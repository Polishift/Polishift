using Dataformatter.Data_accessing.Repositories;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using UnityEngine;

namespace Startup_Scripts
{
    //Decorator
    public class PredictionCountryPrefabDecorator : CountryPrefabDecorator
    {
        public override void SetDecoratorVariables(AbstractCountryPrefab parentPrefab, GameObject countriesGameObject)
        {
            base.ParentPrefab = parentPrefab;
            base.ThisCountriesGameObject = countriesGameObject;
        }
        
        
        public override void Initialize(Iso3166Country isoCountry)
        {
            base.ParentPrefab.Initialize(isoCountry);
            
            base.ThisCountriesGameObject.GetComponent<CountryInformationReference>().Init(isoCountry);
            base.ThisCountriesGameObject.GetComponent<PredictionRulerHandler>().Init();
        }
    }
}
