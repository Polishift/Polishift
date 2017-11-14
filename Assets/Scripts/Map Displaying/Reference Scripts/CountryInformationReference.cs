using Dataformatter.Data_accessing.Repositories;
using UnityEngine;

namespace Map_Displaying.Reference_Scripts
{
    public class CountryInformationReference : MonoBehaviour
    {
        public readonly Iso3166Country Iso3166Country;

        public CountryInformationReference(Iso3166Country iso3166Country)
        {
            Iso3166Country = iso3166Country;
        }
    }
}