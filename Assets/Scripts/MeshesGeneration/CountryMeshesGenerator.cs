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
            */
            Dataformatter.Paths.SetProcessedDataFolder(@"E:\Hogeschool\Polishift Organization\polishift\Assets\ProcessedData");
            Dataformatter.Paths.SetRawDataFolder(@"E:\Hogeschool\Polishift Organization\Datasources");

            var geoModelFactory = new CountryGeoModelFactory();
            var countryGeoJsonPath = Dataformatter.Paths.RawDataFolder;// + @"\CountryInformation";
            var countryBorderModels = JsonToModel<CountryGeoModel>.ParseJsonDirectoryToModels(countryGeoJsonPath,
                                                                                           geoModelFactory, 
                                                                                           "*.geo.json");

            var countryBordersProcessor = new CountryBordersProcessor();
            countryBordersProcessor.SerializeDataToJson(countryBorderModels);

            Debug.Log("Done parsing country borders from LL to XY");


            var countryBordersRepo = new CountryBordersRepository();
            var testCountryBorders = countryBordersRepo.GetByCountry("NLD").First();

            MeshCreator meshCreator = new MeshCreator();
            this._vertices = meshCreator.GetVerticesForCountryBorders(testCountryBorders);

            var algo = new BowyerAlgorithm.BowyerAlgorithm(this._vertices);
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
                Gizmos.DrawSphere(_vertices[i], 1f);
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

