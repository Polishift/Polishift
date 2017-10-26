using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Dataformatter.Datamodels;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Dataprocessing.Processors;
using Dataformatter.Dataprocessing.Parsing;

using MeshesGeneration.BowyerAlgorithm;


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
        private List<Vector3> _vertices = new List<Vector3>();
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


            _countryBordersRepository = new CountryBordersRepository();

            var netherlandsBordersEntity = _countryBordersRepository.GetByCountry("AFG").First();
            this._vertices = meshCreator.GetVerticesForCountryBorders(netherlandsBordersEntity);
            
            var verticesAsPoints = new List<XYPoint>();
            foreach (var vert in this._vertices)
                verticesAsPoints.Add(new XYPoint(){X = vert.x, Y = vert.y});
            */

            
            var testPointsOne = new List<XYPoint>
            {
                new XYPoint { X = 0, Y = 45 },
                new XYPoint { X = 45, Y = 0 },
                new XYPoint { X = 55, Y = 100 },
                new XYPoint { X = 90, Y =  10 },
                new XYPoint { X = 150, Y = 55 },
                new XYPoint { X = 120, Y = -50 },
                new XYPoint { X = 180, Y = 155 },
                
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

            Debug.Log(_vertices.Count);
            Gizmos.color = Color.black;
            for (int i = 0; i < _vertices.Count; i++)
            {
                //Debug.Log("Drawing vert at " + _vertices[i]);
                Gizmos.DrawSphere(_vertices[i], 0.74f);
            }


            for (int j = 0; j < _triangles.Count; j++)
            {
                for (int k = 0; k < _triangles[j].Edges.Count; k++)
                {
                    Gizmos.color = Color.black;

                    var currentEdge = _triangles[j].Edges[k];
                    var startVector3 = new Vector3((float)currentEdge.StartPoint.X, (float)currentEdge.StartPoint.Y);
                    var endVector3 = new Vector3((float)currentEdge.EndPoint.X, (float)currentEdge.EndPoint.Y);

                    Gizmos.DrawLine(startVector3, endVector3);
                }
            }
        }
    }
}

