using DefaultNamespace;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration;
using Repository;
using UnityEngine;


namespace Startup_Scripts
{
    public class CountriesSpawner : MonoBehaviour
    {
        public CountryPrefab _originalCountryPrefab;

        private void Awake()
        {
            //Probly oughta make the repohub a static gameObject of its own
            RepositoryHub.Init();

            foreach (var currentCountry in RepositoryHub.Iso3166Countries)
            {
                if (currentCountry.Alpha3 == "BEL")
                {
                    CountryInformationReference countryInformationReference =
                        new CountryInformationReference(currentCountry);

                    var baseMesh = CreateBaseMesh(countryInformationReference);
                    
                    
                    //TODO: Set this transform.position to be that of the mesh
                    CountryPrefab cloneForCurrentCountry = (CountryPrefab) Instantiate(_originalCountryPrefab,
                                                                                       baseMesh.bounds.center,
                                                                                       transform.rotation);


                    cloneForCurrentCountry.gameObject.AddComponent<MeshFilter>();
                    cloneForCurrentCountry.gameObject.AddComponent<MeshRenderer>();
                    cloneForCurrentCountry.gameObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));

                    cloneForCurrentCountry.gameObject.GetComponent<MeshFilter>().mesh = baseMesh;

                    cloneForCurrentCountry.gameObject.AddComponent<PivotOffsetter>();

                    //cloneForCurrentCountry.Init(countryInformationReference);
                }
            }
        }
    
        //todo: Needs to combine as well
        private Mesh CreateBaseMesh(CountryInformationReference spawnersCountryInfo)
        {
            var meshesForOurCountrysPolygons = MeshCreator.GetMeshPerPolygon(spawnersCountryInfo);
            var mesh = meshesForOurCountrysPolygons[0];

            return mesh;
        }
        
        private Mesh CreateOutlineMesh(Mesh originalMesh)
        {
            return null;
        }
    }
}