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


        public void GenerateMeshes()
        {
            MeshCreator meshCreator = new MeshCreator();

            var meshesForOurCountrysPolygons = meshCreator.GetMeshPerPolygon(GetComponent<CountryInformationReference>());
            
            
            for(int i = 0; i < meshesForOurCountrysPolygons.Count; i++)
            {
                var mesh = meshesForOurCountrysPolygons[i];
                AddChildMesh(mesh);
            }
            CombineChildMeshes();
        }

        private void AddChildMesh(Mesh childMesh)
        {
            /*
            * DEBUGGING
              - Combining or not doesnt matter 
              - Index of the polygon being meshified doesnt matter
              - Reversing the tri's only flips
            */


            GameObject newChildObject = new GameObject {name = "Child mesh"};
            newChildObject.transform.parent = gameObject.transform;

            newChildObject.AddComponent<MeshFilter>();
            newChildObject.AddComponent<MeshRenderer>();

            newChildObject.GetComponent<MeshFilter>().mesh = childMesh;
        }


        private void CombineChildMeshes()
        {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

                meshFilters[i].gameObject.active = false;

                i++;
            }

            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.gameObject.active = true;

            GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));
        }
    }
}