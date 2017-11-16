using System.Collections.Generic;
using System.Linq;

using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using MeshesGeneration.Triangulator;
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

                
                //todo DEBUUGGGING TO SEE IF NORMAL TRIANGULATOR WORKS NOW FOR SOME REASON HAHAHAHAH
                if (0 == 0)
                {
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
                }
                else
                {
                    PSLG planarStraightLineGraph = new PSLG();
                    planarStraightLineGraph.AddVertexLoop(currentPolygonPointsAsVectors);
                
                    TriangleAPI triangle = new TriangleAPI();
                    Polygon2D polygon = triangle.Triangulate(planarStraightLineGraph);
                

                    CustomMeshBuilder builder = new CustomMeshBuilder();

                    for (int i = 0; i < polygon.triangles.Length; i += 3)
                    {
                        int[] indices = { polygon.triangles[i], polygon.triangles[i + 1], polygon.triangles[i + 2] };
                        Vector3[] tri = { polygon.vertices[indices[0]], polygon.vertices[indices[1]], polygon.vertices[indices[2]] };
                        Vector3 normal = new Plane(tri[0], tri[1], tri[2]).normal;
                        Vector3[] normals = { normal, normal, normal };
                        Vector2[] uvs = { tri[0], tri[1], tri[2] };
                        builder.AddTriangleToMesh(tri, normals, uvs);
                    }

                    Mesh meshForThisPolygon = builder.Build();
                    meshPerPolygon.Add(meshForThisPolygon);
                }
            }
            return meshPerPolygon;
        }

        private List<Vector2> PolygonTo2DVectorArray(Polygon<XYPoint> polygon)
        {
            var verticesList = new List<Vector2>();

            foreach (var currentPoint in polygon.Points)
            {
                var potentialNewVertice = new Vector2(currentPoint.X * VECTOR_ENLARGEMENT_FACTOR, 
                                                      currentPoint.Y * VECTOR_ENLARGEMENT_FACTOR);
                
                verticesList.Add(potentialNewVertice);
            }
            return verticesList;
        }
    }
}