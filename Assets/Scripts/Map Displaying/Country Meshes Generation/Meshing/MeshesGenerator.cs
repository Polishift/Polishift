﻿using UnityEngine;

using Dataformatter.Data_accessing.Repositories;
using Map_Displaying.Country_Meshes_Generation.Meshing;
using Map_Displaying.Reference_Scripts;


namespace MeshesGeneration
{
    public class MeshesGenerator : MonoBehaviour
    {
        private Material _countryMaterial;
        
        private void Awake()
        {
            _countryMaterial = Resources.Load<Material>("BorderMaterial"); 
        }
        

        public void GenerateMeshes()
        {
            var meshesForOurCountrysPolygons =
                MeshCreator.GetMeshPerPolygon(GetComponent<CountryInformationReference>());
            
            for (int i = 0; i < meshesForOurCountrysPolygons.Count; i++)
            {
                var mesh = meshesForOurCountrysPolygons[i];
                AddChildGameObjectWithMesh(mesh, "Child Polygon Mesh");
            }
            CombineChildMeshesIntoOne();
            RemoveChildren();

            AddMeshCollider();

            gameObject.AddComponent<BorderOutlineCreator>();
        }

        private void AddChildGameObjectWithMesh(Mesh mesh, string gameObjectName)
        {
            GameObject newChildObject = new GameObject() {name = gameObjectName};
            newChildObject.transform.parent = gameObject.transform;

            newChildObject.AddComponent<MeshFilter>();
            newChildObject.AddComponent<MeshRenderer>();

            newChildObject.GetComponent<MeshFilter>().mesh = mesh;
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

            GetComponent<MeshRenderer>().material = _countryMaterial;
        }

        private void RemoveChildren()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void AddMeshCollider()
        {
            gameObject.GetComponent<MeshCollider>().sharedMesh = gameObject.GetComponent<MeshFilter>().mesh;
            gameObject.GetComponent<MeshCollider>().name = gameObject.name;
        }
    }
}