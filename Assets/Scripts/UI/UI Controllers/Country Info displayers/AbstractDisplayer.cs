using System.ComponentModel;
using Game_Logic;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using Predicting;
using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI
{
    public abstract class AbstractDisplayer : MonoBehaviour
    {
        protected int YearOfCreation;

        //this gameobject needs to be set beforehand, which sucks.
        protected GameObject CountryGameObject;

        protected GameObject UI_Canvas;

        protected CountryInformationReference CountryInformation;
        protected AbstractRulerHandler ThisCountriesRulerHandler;


        public void BaseInit()
        {
            YearOfCreation = YearCounter.GetCurrentYear();

            CountryGameObject = gameObject;
            UI_Canvas = GameObject.Find("UI_Canvas");

            CountryInformation = CountryGameObject.GetComponent<CountryInformationReference>();

            //Getting the correct handler
            if (CountryGameObject.GetComponent<SimulationRulerHandler>() != null)
                ThisCountriesRulerHandler = CountryGameObject.GetComponent<SimulationRulerHandler>();
            else
                ThisCountriesRulerHandler = CountryGameObject.GetComponent<PredictionRulerHandler>();
        }

        protected string GetCurrentCountryText()
        {
            return CountryInformation.Iso3166Country.Name + " in " + YearOfCreation;
        }

        protected string GetCurrentRulerText()
        {
            return ThisCountriesRulerHandler.RulerToText();
        }

        public abstract void Init();

        protected abstract void CreateBasicPanel();
        protected abstract void DestroyThisPanel();
    }
}