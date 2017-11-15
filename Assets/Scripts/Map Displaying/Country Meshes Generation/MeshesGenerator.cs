using UnityEngine;

using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Repositories;
using Map_Displaying.Reference_Scripts;


namespace MeshesGeneration
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshesGenerator : MonoBehaviour
    {
        private Mesh _testMesh;

        private CountryBordersRepository _countryBordersRepository;

        private void Awake()
        {
            GenerateMesh();
        }

        private void GenerateMesh()
        {
            MeshCreator meshCreator = new MeshCreator();
            
            //Expand this to all polygons per country
            var testMesh = meshCreator.GetMeshPerPolygon(gameObject.GetComponent<CountryInformationReference>())[0];

            //Set up game object with mesh;
            gameObject.GetComponent<MeshFilter>().mesh = testMesh;
        }
    }
}

