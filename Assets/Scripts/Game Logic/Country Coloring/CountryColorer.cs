using System;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    public class CountryColorer : MonoBehaviour
    {
        public void UpdateCountryColorForNewRuler(string classification, double turnout)
        {
            try
            {
                var lowerCaseClassification = classification.ToLower(); //just in case (get it?)
                var colorForRulingParty = PoliticalFamilyRecords.ColorPerFamily[lowerCaseClassification];

                //Converting the base RGB to HSV so we can change the Saturation
                float h;
                float s;
                float v;
                Color.RGBToHSV(colorForRulingParty, out h, out s, out v);

                //Converting
                if (s > 0)
                    s = CalculateTurnoutSaturation(s, turnout);


                //Getting an rbg back again
                var rgbWithUpdatedSaturation = Color.HSVToRGB(h, s, v);


                gameObject.GetComponent<MeshRenderer>().material.color = rgbWithUpdatedSaturation;
            }
            catch (Exception e)
            {
                Debug.Log("Classification " + classification + " caused an exception in country " + gameObject.GetComponent<CountryInformationReference>().Iso3166Country.Name);
            }
        }

        private float CalculateTurnoutSaturation(float originalSaturation, double turnout)
        {
            if (turnout >= 100)
            {
                return 1f;
            }
            else if (turnout > 80 && turnout < 100)
            {
                return originalSaturation - ((float) turnout / 500f);
            }
            else if (turnout > 60 && turnout < 80)
            {
                return originalSaturation - ((float) turnout / 200f);
            }
            else
            {
                return originalSaturation - ((float) turnout / 100f);
            }
        }
    }
}