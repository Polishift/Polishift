using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Datamodels;
using Dataformatter.Dataprocessing.Entities;
using Dataformatter.Dataprocessing.Parsing;
using Dataformatter.Dataprocessing.Processors;


public class MeshCreator
{
    /*
    -Read parsed polygon points per country from ProcessedData/
    -Create meshes per country
    */

    public List<Vector3> GetVerticesForCountryBorders(CountryBordersEntity countryBordersEntity)
    {
        var verticesList = new List<Vector3>();

        for (int i = 0; i < countryBordersEntity.Polygons.Count; i++)
        {
            var currentPolygon = countryBordersEntity.Polygons[i];

            for (int j = 0; j < currentPolygon.Points.Count; j++)
            {
                var currentPoint = currentPolygon.Points[j];
                verticesList.Add(new Vector3((float) currentPoint.X/10000,(float)  currentPoint.Y/10000));
            }
        }
        return verticesList;
    }
}
