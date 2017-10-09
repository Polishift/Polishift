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
    private List<Vector3> vertices = new List<Vector3>();
    private List<Triangle> triangles = new List<Triangle>();

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
      
        MeshCreator meshCreator = new MeshCreator();
      */

        countryBordersRepository = new CountryBordersRepository();

        var netherlandsBordersEntity = countryBordersRepository.GetByCountry("BEL");
        //this.vertices = meshCreator.GetVerticesForCountryBorders(netherlandsBordersEntity);
  
       
        var testPoints = new List<XYPoint>
        {
            new XYPoint { X = 0, Y = 45 },
            new XYPoint { X = 45, Y = 0 },
            new XYPoint { X = 51, Y = 100 },
            new XYPoint { X = 90, Y =  10 },
            new XYPoint { X = 100, Y = 55 },
        };
        for (int i = 0; i < testPoints.Count; i++)
        {
            vertices.Add(new Vector3(testPoints[i].X, testPoints[i].Y));
        }
         /*

        var testPoints = new List<XYPoint>();
        for (int i = 0; i < vertices.Count; i++)
        {
            testPoints.Add(new XYPoint {X = vertices[i].x, Y = vertices[i].y});
            i+=5;
        }
         */

        var algo = new BowyerAlgorithm(testPoints);
        triangles = algo.ComputeFinalTriangulation().ToList();
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
            Gizmos.DrawSphere(vertices[i], 1.44f);
        }


        for (int j = 0; j < triangles.Count; j++)
        {
            for (int k = 0; k < triangles[j].Edges.Count; k++)
            {
                var currentEdge = triangles[j].Edges[k];
                var startVector3 = new Vector3(currentEdge.startPoint.X, currentEdge.startPoint.Y);
                var endVector3 = new Vector3(currentEdge.endPoint.X, currentEdge.endPoint.Y);

                Gizmos.DrawLine(startVector3, endVector3);
            }
        }
    }
}
