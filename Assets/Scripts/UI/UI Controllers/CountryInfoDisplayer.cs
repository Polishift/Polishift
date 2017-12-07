using System.Collections.Generic;
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


        public void Init()
        {
            _countryInformation = gameObject.GetComponent<CountryInformationReference>();
            CreateBasicInfoPanel();
        }

        private void CreateBasicInfoPanel()
        {
            GenericPanel newPanel = new GenericPanel();            
            
            string name = _countryInformation.Iso3166Country.Name + "_InfoPanel";
            Font defaultFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            Material defaultMaterial = new Material(Shader.Find("UI/Default_Overlay"));
            Sprite defaultSprite = Resources.Load<Sprite>("Square");
            Sprite buttonSprite = Resources.Load<Sprite>("Triangle");
            
            //GetComponent<CountryElectionHandler>().ToString()
            var textLabels = new List<TextLabelInfo>
            {
                new TextLabelInfo(GetComponent<CountryElectionHandler>().ToString(), 
                                  defaultFont, 12, 
                                  defaultMaterial, Color.black, 
                                  new Vector3(0, 0))
            };

            var buttons = new List<ButtonInfo>
            {
                new ButtonInfo(new Vector3(80, 80), new Vector3(0.3f, 0.3f), 
                               buttonSprite, () =>  newPanel.Destroy())
            }; 
            
            ImageInfo backgroundImageInfo = new ImageInfo(new Vector3(0, 0), 
                                                          new Vector3(2, 2), 
                                                          defaultSprite);
            
            
            newPanel.Create(new Vector2(0, 0), name, backgroundImageInfo, textLabels, buttons);
        }
    }
}