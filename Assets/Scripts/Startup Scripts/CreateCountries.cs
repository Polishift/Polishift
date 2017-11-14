using System.Linq;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using UnityEngine;

namespace Startup_Scripts
{
    public class CreateCountries : MonoBehaviour
    {
        void Awake()
        {
            /*
             * Loop through all EU Countries
             * Create prefabs for each
             * Each prefab having
             * - Country (ISO1336) script reference
             * - OnClick script
             * - Mesh
             */
            foreach (var VARIABLE in Iso3166Repository)
            {
                
            }
        }
    }
}