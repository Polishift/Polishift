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
    public class CountryInfoDisplayer : AbstractDisplayer
    {
        //onClick sound
        AudioSource onClickSoundSource;
        
        //UI object / names
        public static readonly string PREFIX = "INFO DISPLAYER";
        private static readonly string PANEL_OBJNAME = PREFIX + " Background image";
        private static readonly string COUNTRYNAME_OBJNAME = PREFIX + " CountryName";
        private static readonly string RULERINFO_OBJNAME = PREFIX + " RulerInfo";
        private static readonly string CLOSEBUTTON_OBJNAME = PREFIX + " CloseButton";
        private static readonly string MOREBUTTON_OBJNAME = PREFIX + " MoreButton";
        
        public override void Init()
        {
            base.BaseInit();
            
            onClickSoundSource = GameObject.Find("CountrySelectSound").GetComponent<AudioSource>();

            //Creating the panel, destroying previous just in case
            DestroyThisPanel();
            CreateBasicPanel();
        }

        protected override void CreateBasicPanel()
        {
            PlayClickSound();
            
            
            Sprite defaultSprite = Resources.Load<Sprite>("Square");

            UICreator.AddBackgroundPanelToCanvas(PANEL_OBJNAME, defaultSprite, new Vector3(0, 0), new Vector3(5.5f, 4.2f));
            
            UICreator.AddTextToCanvas(COUNTRYNAME_OBJNAME, GetCountryName(), 35, new Vector3(-200, 60), new Vector2(100, 200));
            UICreator.AddTextToCanvas(RULERINFO_OBJNAME, base.GetCurrentRulerText(), 20, new Vector3(-200, 0), new Vector2(100, 200));

            Sprite exitButtonSprite = Resources.Load<Sprite>("Sprites/ExitButton");
            UICreator.AddChildButtonToCanvas(CLOSEBUTTON_OBJNAME, exitButtonSprite, exitButtonSprite, DestroyThisPanel, new Vector3(220, 150), 0.3f);
            
            Sprite moreInfoButton = Resources.Load<Sprite>("Sprites/Detailicon");
            UICreator.AddChildButtonToCanvas(MOREBUTTON_OBJNAME, moreInfoButton, moreInfoButton, CreateCountryDetailsDisplayer, new Vector3(180, 150), 0.3f);
        }
        
        protected override void DestroyThisPanel()
        {
            Destroy(GameObject.Find(PANEL_OBJNAME));
            Destroy(GameObject.Find(COUNTRYNAME_OBJNAME));
            Destroy(GameObject.Find(RULERINFO_OBJNAME));
            Destroy(GameObject.Find(CLOSEBUTTON_OBJNAME));
            Destroy(GameObject.Find(MOREBUTTON_OBJNAME));
        }

        
        
        private void CreateCountryDetailsDisplayer()
        {
            base.gameObject.AddComponent<CountryDetailsDisplayer>().Init();
        }
        
        private void PlayClickSound()
        {
            onClickSoundSource.Play();
        }

        private string GetCountryName()
        {
            var potentiallyLowerCaseName = base.CountryInformation.Iso3166Country.Name;
            potentiallyLowerCaseName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(potentiallyLowerCaseName.ToLower());

            return potentiallyLowerCaseName;
        }
    }
}