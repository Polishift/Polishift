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
        public void UpdateCountryColorForNewParty(ElectionEntity newRulingParty)
        {
            var currentRulingPartysFamily = newRulingParty.PartyClassification.ToLower();
            var colorForRulingParty = PoliticalFamilyColors.ColorPerFamily[currentRulingPartysFamily];
            gameObject.GetComponent<MeshRenderer>().material.color = colorForRulingParty;
        }
    }
}