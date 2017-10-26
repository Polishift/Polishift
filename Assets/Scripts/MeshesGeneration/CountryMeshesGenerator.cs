using System.Collections.Generic;
using System.Linq;
using MeshesGeneration.BowyerAlgorithm;
using Dataformatter.Datamodels;
using Dataformatter.Data_accessing.Repositories;
using UnityEngine;

namespace MeshesGeneration
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))] //later add things instead
    public class CountryMeshesGenerator : MonoBehaviour
    {
        /*
    -Read parsed polygon points per country from ProcessedData/
    -Create meshes per country
    */
        private Mesh _testMesh;

        private CountryBordersRepository _countryBordersRepository;
        private MeshCreator _meshCreator = new MeshCreator();
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private List<Triangle> _triangles = new List<Triangle>();

        //testEdges
        private List<Edge> _testEdges = new List<Edge>();


        private void Awake()
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
      
        
        countryBordersRepository = new CountryBordersRepository();

        var netherlandsBordersEntity = countryBordersRepository.GetByCountry("BEL");
        //this.vertices = meshCreator.GetVerticesForCountryBorders(netherlandsBordersEntity);
        */

            var testPointsOne = new List<XYPoint>
            {
                new XYPoint { X = 0, Y = 45 },
                new XYPoint { X = 45, Y = 10 },
                new XYPoint { X = 51, Y = 100 },
                new XYPoint { X = 90, Y =  10 },
                new XYPoint { X = 100, Y = 55 },
            };
            for (var i = 0; i < testPointsOne.Count; i++)
            {
                _vertices.Add(new Vector3(testPointsOne[i].X, testPointsOne[i].Y));
            }

            var algo = new BowyerAlgorithm.BowyerAlgorithm(testPointsOne);
            _triangles = algo.ComputeFinalTriangulation().ToList();

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

            if (_vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;
            for (var i = 0; i < _vertices.Count; i++)
            {
                Gizmos.DrawSphere(_vertices[i], 1.44f);
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

       
            for (var j = 0; j < _triangles.Count; j++)
            {
                for (var k = 0; k < _triangles[j].Edges.Count; k++)
                {
                    var currentEdge = _triangles[j].Edges[k];
                    var startVector3 = new Vector3(currentEdge.StartPoint.X, currentEdge.StartPoint.Y);
                    var endVector3 = new Vector3(currentEdge.EndPoint.X, currentEdge.EndPoint.Y);

                    if(currentEdge.IS_BAD)
                        Gizmos.color = Color.red;
                    else
                        Gizmos.color = Color.black;

                    Gizmos.DrawLine(startVector3, endVector3);
                }
            } 
        }
    }
}
