using UnityEngine;
using System.Collections.Generic;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Data_accessing.Repositories;
using Map_Displaying.Reference_Scripts;


namespace MeshesGeneration
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class MeshesGenerator : MonoBehaviour
    {
        //[Serializable]
        public readonly float OutlineScale = 1.1f; 

        private CountryBordersRepository _countryBordersRepository;


        public void GenerateMeshes()
        {
            var meshesForOurCountrysPolygons = MeshCreator.GetMeshPerPolygon(GetComponent<CountryInformationReference>());
            
            
            for(int i = 0; i < meshesForOurCountrysPolygons.Count; i++)
            {
                var mesh = meshesForOurCountrysPolygons[i];
                AddChildMesh(mesh);
            }
            CombineChildMeshesIntoOne();
            RemoveChildren();

            AddMeshCollider();

            AddOutlineMesh();
        }

        private void AddChildMesh(Mesh childMesh)
        {
            GameObject newChildObject = new GameObject {name = "Child mesh"};
            newChildObject.transform.parent = gameObject.transform;

            newChildObject.AddComponent<MeshFilter>();
            newChildObject.AddComponent<MeshRenderer>();

            newChildObject.GetComponent<MeshFilter>().mesh = childMesh;
        }


        private void CombineChildMeshesIntoOne()
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

        //Doesnt remove all meshes thoroughly
        private void RemoveChildren()
        {
            var children = new List<GameObject>();
            foreach (Transform child in transform) children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        private void AddMeshCollider()
        {
            gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
            gameObject.GetComponent<MeshCollider>().name = gameObject.name;
        }

        private void AddOutlineMesh()
        {
            //Scale down the original mesh and get a normal sized outline
            var outlineMesh = gameObject.GetComponent<MeshFilter>().mesh;
            var scaledDownOriginalMesh = MeshCreator.GetScaledMesh(gameObject.GetComponent<MeshFilter>().mesh, 1 - OutlineScale);

            //now: add outline as a child and change its color. set scaled down mesh to be new actual mesh
            //Extract to method
            Vector3[] vertices = outlineMesh.vertices;
            Color[] colors = new Color[vertices.Length];
            int i = 0;
            while (i < vertices.Length) {
                colors[i] = Color.Lerp(Color.green, Color.green, vertices[i].y);
                i++;
            }
            outlineMesh.colors = colors;

            AddChildMesh(outlineMesh);
            gameObject.GetComponent<MeshFilter>().mesh = scaledDownOriginalMesh;
        }
    }
}