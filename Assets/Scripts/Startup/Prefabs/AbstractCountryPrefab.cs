using Dataformatter.Data_accessing.Repositories;
using Map_Displaying.Reference_Scripts;
using UnityEngine;

namespace Startup_Scripts
{
    public abstract class AbstractCountryPrefab : MonoBehaviour
    {
        public abstract void Initialize(Iso3166Country isoCountry);
    }
}