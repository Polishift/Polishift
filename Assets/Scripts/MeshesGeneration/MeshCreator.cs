using System.Collections.Generic;
using Dataformatter.Dataprocessing.Entities;
using UnityEngine;

namespace MeshesGeneration
{
    public class MeshCreator
    {
        /*
    -Read parsed polygon points per country from ProcessedData/
    -Create meshes per country
    */

        public List<Vector3> GetVerticesForCountryBorders(CountryBordersEntity countryBordersEntity)
        {
            var verticesList = new List<Vector3>();

            for (var i = 0; i < countryBordersEntity.Polygons.Count; i++)
            {
                var currentPolygon = countryBordersEntity.Polygons[i];

                for (var j = 0; j < currentPolygon.Points.Count; j++)
                {
                    var currentPoint = currentPolygon.Points[j];
                    verticesList.Add(new Vector3(currentPoint.X * 1000, currentPoint.Y * 1000));
                }
            }
            return verticesList;
        }
    }
}