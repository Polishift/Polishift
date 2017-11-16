using System.Collections.Generic;
using System.Linq;

using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using Repository;
using UnityEngine;


namespace MeshesGeneration
{
    public class MeshCreator
    {
        private static readonly int VECTOR_ENLARGEMENT_FACTOR = 1000;


        public List<Mesh> GetMeshPerPolygon(CountryInformationReference countryInformationReference)
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
                /*
                    // Use the triangulator to get indices for creating triangles
                    AlternativeTriangulator tr = new AlternativeTriangulator(currentPolygonPointsAsVectors);
                    int[] indices = tr.Triangulate();
 
                    // Create the Vector3 vertices
                    Vector3[] vertices = new Vector3[currentPolygonPointsAsVectors.Count];
                    for (int i=0; i<vertices.Length; i++) {
                        vertices[i] = new Vector3(currentPolygonPointsAsVectors[i].x, currentPolygonPointsAsVectors[i].y, 0);
                    }
 
                    // Create the mesh
                    Mesh msh = new Mesh();
                    msh.vertices = vertices;
                    msh.triangles = indices;
                    msh.RecalculateNormals();
                    msh.RecalculateBounds();
                    
                    meshPerPolygon.Add(msh);
                 */
            }
            return meshPerPolygon;
        }

        private List<Vector3> PolygonTo2DVectorArray(Polygon<XYPoint> polygon)
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