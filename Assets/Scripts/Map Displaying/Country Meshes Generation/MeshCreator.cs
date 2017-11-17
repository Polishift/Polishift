using System.Collections.Generic;
using System.Linq;

using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using UnityEngine;


namespace MeshesGeneration
{
    public static class MeshCreator
    {
        private static readonly int VECTOR_ENLARGEMENT_FACTOR = 1000;


        public static List<Mesh> GetMeshPerPolygon(CountryInformationReference countryInformationReference)
        {
            var meshPerPolygon = new List<Mesh>();

            var countryAlpha3 = countryInformationReference.Iso3166Country.Alpha3;
            var countryBorders = RepositoryHub.CountryBordersRepository.GetByCountry(countryAlpha3).First();
            
            for (int j = 0; j < countryBorders.Polygons.Count; j++)
            {
                var countryBordersPolygon = countryBorders.Polygons[j];
                var currentPolygonPointsAsVectors = PolygonTo2DVectorArray(countryBordersPolygon); 

                Poly2Mesh.Polygon poly = new Poly2Mesh.Polygon();
                poly.outside = currentPolygonPointsAsVectors;
                
                meshPerPolygon.Add(Poly2Mesh.CreateMesh(poly));
            }
            return meshPerPolygon;
        }

        public static Mesh GetScaledMesh(Mesh mesh, float scale)
        {
            Vector3[] _baseVertices;
            _baseVertices = mesh.vertices;

            var vertices = new Vector3[_baseVertices.Length];
            for (var i = 0; i < vertices.Length; i++)
            {
                var vertex = _baseVertices[i];
                vertex.x = vertex.x * scale;
                vertex.y = vertex.y * scale;
                vertex.z = vertex.z * scale;
                vertices[i] = vertex;
            }
            mesh.vertices = vertices;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private static List<Vector3> PolygonTo2DVectorArray(Polygon<XYPoint> polygon)
        {
            var verticesList = new List<Vector3>();

            foreach (var currentPoint in polygon.Points)
            {
                var potentialNewVertice = new Vector3(currentPoint.X * VECTOR_ENLARGEMENT_FACTOR, 
                                                      currentPoint.Y * VECTOR_ENLARGEMENT_FACTOR);
                
                verticesList.Add(potentialNewVertice);
            }
            return verticesList;
        }
    }
}