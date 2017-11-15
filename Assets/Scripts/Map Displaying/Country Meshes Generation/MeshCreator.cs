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
            var countryBorders = RepositoryHub.CountryBordersRepository.GetByCountry(countryAlpha3).FirstOrDefault();
            
            foreach (var polygon in countryBorders.Polygons)
            {
                var currentPolygonPointsAsVectors = PolygonToVector3List(polygon); 
                
                //Use the triangulator to get indices for creating triangles
                NaiveTriangulator tr = new NaiveTriangulator(currentPolygonPointsAsVectors);
                int[] indices = tr.Triangulate();
 
                //Create the mesh
                Mesh msh = new Mesh();
                msh.vertices = currentPolygonPointsAsVectors.ToArray();
                msh.triangles = indices.Reverse().ToArray();
                msh.RecalculateNormals();
                msh.RecalculateBounds();
                
                meshPerPolygon.Add(msh);
            }
            return meshPerPolygon;
        }

        //todo make this private later
        public List<Vector3> PolygonToVector3List(Polygon<XYPoint> polygon)
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