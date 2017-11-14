using UnityEngine;

using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Repositories;


namespace MeshesGeneration
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] //, typeof(CountryEntity)
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
            var testMesh = meshCreator.GetMeshPerPolygon(gameObject.GetComponent<CountryEntity>())[0];

            //Set up game object with mesh;
            gameObject.GetComponent<MeshFilter>().mesh = testMesh;
        }
    }
}

