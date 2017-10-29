﻿using System.Collections.Generic;
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
            Dataformatter.Paths.SetProcessedDataFolder(@"E:\Hogeschool\Polishift Organization\ProcessedData\");
            Dataformatter.Paths.SetRawDataFolder(@"E:\Hogeschool\Polishift Organization\ProcessedData\CountryInformation");

            var geoModelFactory = new CountryGeoModelFactory();
            var countryGeoJsonPath = Dataformatter.Paths.RawDataFolder;
            var countryBorderModels = JsonToModel<CountryGeoModel>.ParseJsonDirectoryToModels(countryGeoJsonPath,
                                                                                           geoModelFactory, 
                                                                                           "*.geo.json");
            var countryBordersProcessor = new CountryBordersProcessor();
            countryBordersProcessor.SerializeDataToJson(countryBorderModels);

            //Debugging from here on out
            var countryBordersRepo = new CountryBordersRepository();
            var testCountryBordersTwo = countryBordersRepo.GetByCountry("BEL").First();
            
            MeshCreator meshCreator = new MeshCreator();
            this._vertices.AddRange(meshCreator.GetVerticesForCountryBorders(testCountryBordersTwo));
            */                           
                           
            
            this._vertices = new List<Vector3>()
            {
                //left
                new Vector3(11f, 10),
                new Vector3(11f, 20),
                new Vector3(12f, 30),
                new Vector3(13.5f, 70),
                new Vector3(13.5f, -10),

                //up 
                new Vector3(11, 40),
                new Vector3(11, 30),
                new Vector3(22, 40),
                new Vector3(33, 40),
                new Vector3(44, 40),

                //below
                new Vector3(11, 10),
                new Vector3(22, 10),
                new Vector3(33, 10),
                new Vector3(44, 10),
                
                //right
                new Vector3(40, 10),
                new Vector3(40, 20),
                new Vector3(40, 30),
                new Vector3(40, 40),
            }; 

            //Algo doesnt like points that are very close (< 1 on X or Y) together.
            //Also, investigate what kind of supertriangle you need. (not float min max)
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
                //Debug.Log(_vertices[i]);
                Gizmos.DrawSphere(_vertices[i], 0.44f);
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

