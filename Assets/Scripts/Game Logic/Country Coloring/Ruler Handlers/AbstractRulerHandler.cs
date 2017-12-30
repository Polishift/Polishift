using Map_Displaying.Reference_Scripts;
using UnityEngine;

namespace Game_Logic.Country_Coloring
{
    public abstract class AbstractRulerHandler : MonoBehaviour
    {
        protected bool IsInitialized;
        protected CountryInformationReference ThisCountriesInfo;

        private void Update()
        {
            if(IsInitialized)
                HandleRuler();
        }

        public abstract void Init();
        public abstract void HandleRuler();

    }
}