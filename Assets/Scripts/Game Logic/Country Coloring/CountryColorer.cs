using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Dataformatter.Dataprocessing.Entities;
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
                gameObject.GetComponent<MeshRenderer>().material.color = colorForRulingParty;
            }
            catch (Exception e)
            {
                Debug.Log("Classification " + classification + " caused an exception in country " + 
                          gameObject.GetComponent<CountryInformationReference>().Iso3166Country.Name);
            }
        }
    }
}