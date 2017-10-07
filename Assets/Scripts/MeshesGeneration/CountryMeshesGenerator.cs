using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] //later add things instead
public class CountryMeshesGenerator : MonoBehaviour
{
    /*
    -Read parsed polygon points per country from ProcessedData/
    -Create meshes per country
    */
    private Mesh testMesh;

    private CountryBordersRepository countryBordersRepository;
    private MeshCreator meshCreator = new MeshCreator();
    private List<Vector3> vertices;

    void Awake()
    {
        TestGenerate();
    }

    private void TestGenerate()
    {
        /*
        const string countryBorderDirectory = "Assets/External Libraries/Dataformatter/ProcessedData/CountryInformation/";
        IJsonModelFactory<CountryGeoModel> countryGeoModelFactory = new CountryGeoModelFactory();
        var processor = new CountryBordersProcessor();

        var allCountryGeoModels =
           JsonToModel<CountryGeoModel>.ParseJsonDirectoryToModels(countryBorderDirectory, 
                                                                   countryGeoModelFactory, "*.geo.json");
        processor.SerializeDataToJson(allCountryGeoModels);
        */

        MeshCreator meshCreator = new MeshCreator();
        countryBordersRepository = new CountryBordersRepository();

        var netherlandsBordersEntity = countryBordersRepository.GetByCountry("NLD");
        this.vertices = meshCreator.GetVerticesForCountryBorders(netherlandsBordersEntity);
        testMesh.vertices = this.vertices.ToArray();
    }


    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Count; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.035f);
        }
    }
}
