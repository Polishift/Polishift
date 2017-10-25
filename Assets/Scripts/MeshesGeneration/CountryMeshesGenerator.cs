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
                new XYPoint { X = 45, Y = 0 },
                new XYPoint { X = 55, Y = 100 },
                new XYPoint { X = 90, Y =  10 },
                new XYPoint { X = 110, Y = 55 },
            };
            for (var i = 0; i < testPointsOne.Count; i++)
            {
                _vertices.Add(new Vector3(testPointsOne[i].X, testPointsOne[i].Y));
            }

            var algo = new BowyerAlgorithm.BowyerAlgorithm(testPointsOne);
            _triangles = algo.ComputeFinalTriangulation().ToList();
        }


        private void OnDrawGizmos()
        {
            if (_vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;
            for (int i = 0; i < _vertices.Count; i++)
            {
                Gizmos.DrawSphere(_vertices[i], 1.44f);
            }


            for (int j = 0; j < _triangles.Count; j++)
            {
                //Well composed triangles: 0, 1, 2, 3 
                // (1 and 2 are both composites of an intersectee)

                

                //if (j == 0)//j == 0 || j == 1 || j == 2 || j == 3)
                //{
                //    Debug.Log("Is -184,65 within the circle of triangle 0: " 
                //          + _triangles[0].IsWithinCircumCircle(new XYPoint(){ X = -184, Y = 170}));
                //    Debug.Log("Is 86,13 within the circle of triangle 0: " 
                //          + _triangles[0].IsWithinCircumCircle(new XYPoint(){ X = 86, Y = 13}));

                    //draw circumcircles
                    var circumCircle = _triangles[j].GetCircumCircle();
                    var transparentYellowColor = new Color(1, 0.92f, 0.016f, 1f);


                    Gizmos.color = transparentYellowColor;

                    for (int k = 0; k < _triangles[j].Edges.Count; k++)
                    {
                        Gizmos.color = Color.black;

                        var currentEdge = _triangles[j].Edges[k];
                        var startVector3 = new Vector3((float)currentEdge.StartPoint.X, (float)currentEdge.StartPoint.Y);
                        var endVector3 = new Vector3((float)currentEdge.EndPoint.X, (float)currentEdge.EndPoint.Y);

                        Gizmos.DrawLine(startVector3, endVector3);
                    }
                //}
            }
        }
    }
}

