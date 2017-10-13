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

    //testEdges
    private List<Edge> testEdges = new List<Edge>();


    void Awake()
    {
        TestGenerate();
    }

    private void TestGenerate()
    {
        
        const string countryBorderDirectory = "Assets/External Libraries/Dataformatter/ProcessedData/CountryInformation/";
        IJsonModelFactory<CountryGeoModel> countryGeoModelFactory = new CountryGeoModelFactory();
        var processor = new CountryBordersProcessor();

        var allCountryGeoModels =
           JsonToModel<CountryGeoModel>.ParseJsonDirectoryToModels(countryBorderDirectory, 
                                                                   countryGeoModelFactory, "*.geo.json");
        processor.SerializeDataToJson(allCountryGeoModels);
      
        MeshCreator meshCreator = new MeshCreator();
      
        
        countryBordersRepository = new CountryBordersRepository();

        var netherlandsBordersEntity = countryBordersRepository.GetByCountry("NLD").First();
        this.vertices = meshCreator.GetVerticesForCountryBorders(netherlandsBordersEntity);
       
/*
        var testPointsOne = new List<XYPoint>
          {
              new XYPoint { X = 0, Y = 45 },
              new XYPoint { X = 45, Y = 10 },
              new XYPoint { X = 51, Y = 100 },
              new XYPoint { X = 90, Y =  10 },
              new XYPoint { X = 100, Y = 55 },
          };*/
        var testPoints = new List<XYPoint>();
        for (int i = 0; i < vertices.Count; i++)
        {
            testPoints.Add(new XYPoint(){X = vertices[i].x/10000, Y = vertices[i].y/10000});
        }
 
        var algo = new BowyerAlgorithm(testPoints);
        triangles = algo.ComputeFinalTriangulation().ToList();

        //The current point is 45,10, so the edge to it oughta be removed next iteration.
        //Since its not seen yet


        //Debug
        /*
        var lineOnePointOne = new XYPoint { X = 0, Y = 45 };
        var lineOnePointTwo = new XYPoint { X = 300, Y = -20 };

        var lineTwoPointOne = new XYPoint { X = 0, Y = 45 };
        var lineTwoPointTwo = new XYPoint { X = 100, Y = 200 };

        var lineThreePointOne = new XYPoint { X = 300, Y = -20 };
        var lineThreePointTwo = new XYPoint { X = 0, Y = 45 };


        var edgeOne = new Edge(lineOnePointOne, lineOnePointTwo);
        var edgeTwo = new Edge(lineTwoPointOne, lineTwoPointTwo);

        Debug.Log("Does testEdgeOne intersect testEdgeTwo = " + edgeOne.CrossesThrough(edgeTwo));
        testEdges.Add(edgeOne);
        testEdges.Add(edgeTwo);



        
        var loosePoint = new XYPoint { X = 45, Y = 10 };

        var lineOnePointOne = new XYPoint { X = 0, Y = 45 };
        var lineOnePointTwo = new XYPoint { X = 100, Y = 200 };

        var lineTwoPointOne = new XYPoint { X = 100, Y = 200 };
        var lineTwoPointTwo = new XYPoint { X = 300, Y = -20 };

        var lineThreePointOne = new XYPoint { X = 300, Y = -20 };
        var lineThreePointTwo = new XYPoint { X = 0, Y = 45 };


        var edgeOne = new Edge(lineOnePointOne, lineOnePointTwo);
        var edgeTwo = new Edge(lineTwoPointOne, lineTwoPointTwo);
        var edgeThree = new Edge(lineThreePointOne, lineThreePointTwo);
        

        List<Edge> testTriangleEdges = new List<Edge>() { edgeOne, edgeTwo, edgeThree };
        Triangle testTriangle = new Triangle() { Edges = testTriangleEdges };

        triangles.Add(testTriangle);
        vertices.Add(new Vector3((float) loosePoint.X, (float) loosePoint.Y));

        Debug.Log("Is loosePoint ( " + loosePoint.ToString() + " ) in triangle ( "
                  + testTriangle.ToString() + " ): " + testTriangle.IsWithinCircumCircle(loosePoint));
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

         /*
        for (int j = 0; j < testEdges.Count; j++)
        {
            var currentEdge = testEdges[j];
            var startVector3 = new Vector3((float)currentEdge.startPoint.X, (float)currentEdge.startPoint.Y);
            var endVector3 = new Vector3((float)currentEdge.endPoint.X, (float)currentEdge.endPoint.Y);

            Gizmos.DrawLine(startVector3, endVector3);
        }
        */

       
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
