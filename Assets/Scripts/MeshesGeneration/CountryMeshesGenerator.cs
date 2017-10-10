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

    //temp
    private List<Edge> testEdges = new List<Edge>();


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


        var testPointsOne = new List<XYPoint>
          {
              new XYPoint { X = 0, Y = 45 },
              new XYPoint { X = 45, Y = 40 },
              new XYPoint { X = 51, Y = 100 },
              new XYPoint { X = 90, Y =  10 },
              new XYPoint { X = 100, Y = 55 },
          };
        for (int i = 0; i < testPointsOne.Count; i++)
        {
            vertices.Add(new Vector3((float)testPointsOne[i].X, (float)testPointsOne[i].Y));
        }

        var algo = new BowyerAlgorithm(testPointsOne);
        triangles = algo.ComputeFinalTriangulation().ToList();

        /*
        var lineOnePointOne = new XYPoint { X = 1, Y = 2 };
        var lineOnePointTwo = new XYPoint { X = 2, Y = 1 };

        var lineTwoPointOne = new XYPoint { X = 10, Y = 10 };
        var lineTwoPointTwo = new XYPoint { X = 50, Y = 30 };

        var edgeOne = new Edge(lineOnePointOne, lineOnePointTwo);
        var edgeTwo = new Edge(lineTwoPointOne, lineTwoPointTwo);

        testEdges = new List<Edge>() { edgeOne, edgeTwo };

        Debug.Log("Does e1 cross through e2: " + edgeOne.CrossesThrough(edgeTwo));
        Debug.Log("Does e2 cross through e1: " + edgeTwo.CrossesThrough(edgeOne));
         */
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

        //temp
        for (int z = 0; z < testEdges.Count; z++)
        {
            var currentEdge = testEdges[z];
            var startVector3 = new Vector3((float)currentEdge.startPoint.X, (float)currentEdge.startPoint.Y);
            var endVector3 = new Vector3((float)currentEdge.endPoint.X, (float)currentEdge.endPoint.Y);

            Gizmos.DrawLine(startVector3, endVector3);
        }



        for (int j = 0; j < triangles.Count; j++)
        {
            for (int k = 0; k < triangles[j].Edges.Count; k++)
            {
                var currentEdge = triangles[j].Edges[k];
                var startVector3 = new Vector3((float)currentEdge.startPoint.X, (float)currentEdge.startPoint.Y);
                var endVector3 = new Vector3((float)currentEdge.endPoint.X, (float)currentEdge.endPoint.Y);

                Gizmos.DrawLine(startVector3, endVector3);
            }
        }
    }
}
