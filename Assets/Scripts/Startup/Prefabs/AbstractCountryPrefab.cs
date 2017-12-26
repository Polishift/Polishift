using Map_Displaying.Reference_Scripts;
using UnityEngine;

namespace Startup_Scripts
{
    public abstract class AbstractCountryPrefab : MonoBehaviour
    {
        public abstract void Init(CountryInformationReference spawnersCountryInfo);
        public abstract GameObject GetParentGameObject();
    }
}