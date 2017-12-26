using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using Predicting;
using UnityEngine;

namespace Startup_Scripts
{
    public class PredictableCountryPrefab : AbstractCountryPrefab
    {
        private AbstractCountryPrefab component;

        public PredictableCountryPrefab(AbstractCountryPrefab component)
        {
            this.component = component;
        }

        
        public override void Init(CountryInformationReference spawnersCountryInfo)
        {
            component.Init(spawnersCountryInfo);

            GetParentGameObject().AddComponent<PredictionCreator>();
            GetParentGameObject().GetComponent<PredictionCreator>().Predict();
        }

        public override GameObject GetParentGameObject()
        {
            return component.GetParentGameObject();
        }
    }
}