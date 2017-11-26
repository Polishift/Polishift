using System.ComponentModel;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Repositories;
using UnityEngine;

namespace Map_Displaying.Reference_Scripts
{
    //This class will eventuall store references to all relevant datasources for a particular country in a given year.
    public class CountryInformationReference : MonoBehaviour
    {
        public Iso3166Country Iso3166Country;

        public CountryInformationReference(Iso3166Country iso3166Country)
        {
            Iso3166Country = iso3166Country;
        }
    }
}