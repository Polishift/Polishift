using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dataformatter.Dataprocessing.Entities;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using Dataformatter.Misc;
using DefaultNamespace.Map_Displaying.UI.Country_Info_Popup;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
{
    [RequireComponent(typeof(CountryInformationReference))]
    public class CountryInfoDisplayer : MonoBehaviour
    {
        private CountryInformationReference _countryInformation;
        private AbstractRulerHandler _thisCountriesRulerHandler;

        public void Init()
        {
            _countryInformation = gameObject.GetComponent<CountryInformationReference>();
            
            //Getting the correct handler
            if(GetComponent<SimulationRulerHandler>() != null)
                _thisCountriesRulerHandler = GetComponent<SimulationRulerHandler>();
            else
                _thisCountriesRulerHandler = GetComponent<PredictionRulerHandler>();

            //Creating the panel
            CreateBasicInfoPanel();
        }

        private void CreateBasicInfoPanel()
        {
            GenericPanel newPanel = new GenericPanel();

            string name = _countryInformation.Iso3166Country.Name + "_InfoPanel";
            Font defaultFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            Material defaultMaterial = new Material(Shader.Find("UI/Default_Overlay"));
            Sprite defaultSprite = Resources.Load<Sprite>("Square");
            Sprite buttonSprite = Resources.Load<Sprite>("Sprites/ExitButton");

            //todo: set width height
            var textLabels = new List<TextLabelInfo> {new TextLabelInfo(GetCountryName(), defaultFont, 35, defaultMaterial, 
                                                                        Color.black, new Vector3(0, 60), new Vector2(300, 200)), 
                                                      new TextLabelInfo(GetCurrentRulerText(), defaultFont, 20, defaultMaterial, 
                                                                        Color.black, new Vector3(0, 0), new Vector2(300, 200))};

            var buttons = new List<ButtonInfo> {new ButtonInfo(new Vector3(150, 150), new Vector3(0.3f, 0.3f), buttonSprite, () => newPanel.Destroy())};
            
            
            
            ImageInfo backgroundImageInfo = new ImageInfo(new Vector3(0, 0), new Vector3(4.2f, 4.2f), defaultSprite);

            newPanel.Create(new Vector2(0, 0), name, backgroundImageInfo, textLabels, buttons);
        }

        private string GetCountryName()
        {
            var potentiallyLowerCaseName = _countryInformation.Iso3166Country.Name;
            potentiallyLowerCaseName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(potentiallyLowerCaseName.ToLower());

            return potentiallyLowerCaseName;
        }

        private string GetCurrentRulerText()
        {
            return _thisCountriesRulerHandler.RulerToText();
        }
    }
}