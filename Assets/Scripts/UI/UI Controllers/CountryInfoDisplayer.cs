using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dataformatter.Dataprocessing.Entities;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using Dataformatter.Misc;
using Predicting;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
{
    [RequireComponent(typeof(CountryInformationReference), typeof(AudioSource))]
    public class CountryInfoDisplayer : MonoBehaviour
    {
        private CountryInformationReference _countryInformation;
        private AbstractRulerHandler _thisCountriesRulerHandler;

        //onClick sound
        AudioSource onClickSoundSource;
        
        //UI object / names
        private GameObject UI_Canvas;
        public static readonly string PREFIX = "INFO DISPLAYER";
        private static readonly string PANEL_OBJNAME = PREFIX + " Background image";
        private static readonly string COUNTRYNAME_OBJNAME = PREFIX + " CountryName";
        private static readonly string RULERINFO_OBJNAME = PREFIX + " RulerInfo";
        private static readonly string CLOSEBUTTON_OBJNAME = PREFIX + " CloseButton";
        
        
        public void Init()
        {
            UI_Canvas = GameObject.Find("UI_Canvas");
            onClickSoundSource = GameObject.Find("CountrySelectSound").GetComponent<AudioSource>();
            _countryInformation = gameObject.GetComponent<CountryInformationReference>();
            
            //Getting the correct handler
            if(GetComponent<SimulationRulerHandler>() != null)
                _thisCountriesRulerHandler = GetComponent<SimulationRulerHandler>();
            else
                _thisCountriesRulerHandler = GetComponent<PredictionRulerHandler>();

            //Creating the panel
            DestroyThisInfoPanel();
            CreateBasicInfoPanel();
        }

        private void CreateBasicInfoPanel()
        {
            PlayClickSound();
            
            
            Sprite defaultSprite = Resources.Load<Sprite>("Square");

            UICreator.AddBackgroundPanelToCanvas(PANEL_OBJNAME, defaultSprite, new Vector3(0, 0), new Vector3(5.5f, 4.2f));
            
            UICreator.AddTextToCanvas(COUNTRYNAME_OBJNAME, GetCountryName(), 35, new Vector3(-200, 60), new Vector2(100, 200));
            UICreator.AddTextToCanvas(RULERINFO_OBJNAME, GetCurrentRulerText(), 20, new Vector3(-200, 0), new Vector2(100, 200));

            Sprite buttonSprite = Resources.Load<Sprite>("Sprites/ExitButton");
            UICreator.AddChildButtonToCanvas(CLOSEBUTTON_OBJNAME, buttonSprite, buttonSprite, DestroyThisInfoPanel, new Vector3(220, 150), 0.3f);
        }

        private void PlayClickSound()
        {
            onClickSoundSource.Play();
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

        private void DestroyThisInfoPanel()
        {
            Destroy(GameObject.Find(PANEL_OBJNAME));
            Destroy(GameObject.Find(COUNTRYNAME_OBJNAME));
            Destroy(GameObject.Find(RULERINFO_OBJNAME));
            Destroy(GameObject.Find(CLOSEBUTTON_OBJNAME));
        }
    }
}