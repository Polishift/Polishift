using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Dataformatter.Datamodels;


public class BowyerAlgorithm
{
    private List<XYPoint> inputPoints;
    private HashSet<Triangle> Triangulation = new HashSet<Triangle>();
    private List<Triangle> currentTriangles = new List<Triangle>();


    public BowyerAlgorithm(List<XYPoint> inputPoints)
    {
        this.inputPoints = inputPoints;
    }

    public HashSet<Triangle> ComputeFinalTriangulation()
    {
        CreateSuperTriangle();
        var superTriangle = currentTriangles[0];

        inputPoints.OrderBy(p => p.X);

        foreach (var currentPoint in inputPoints)
        {
            List<Triangle> badTriangles = DetermineBadTriangles(currentPoint);
            List<Edge> polygons = DeterminePolygons(badTriangles);

            RemoveBadTrianglesFromTriangulation(badTriangles);
            CreateNewTriangles(currentPoint, polygons);
        }
        RemoveSuperTriangleVertices(superTriangle);

        return this.Triangulation;
    }


    private void CreateSuperTriangle()
    {
        var lowerLeftCorner = new XYPoint { X = float.MinValue, Y = float.MinValue }; 
        var lowerRightCorner = new XYPoint { X = float.MaxValue, Y = float.MinValue }; 
        var pyramidTop = new XYPoint
        {
            X = 10,
            Y = float.MaxValue 
        };

        var a = new Edge(lowerLeftCorner, pyramidTop);
        var b = new Edge(pyramidTop, lowerRightCorner);
        var c = new Edge(lowerRightCorner, lowerLeftCorner);
        var superTriangle = new Triangle();
        superTriangle.Edges = new List<Edge> { a, b, c };

        currentTriangles.Add(superTriangle);
        Triangulation.Add(superTriangle);
    }

    private List<Triangle> DetermineBadTriangles(XYPoint currentPoint)
    {
        var badTriangles = new List<Triangle>();

        foreach (var triangle in Triangulation)
        {
            if (triangle.IsWithinCircumCircle(currentPoint))
                badTriangles.Add(triangle);
        }
        return badTriangles;
    }

    private List<Edge> DeterminePolygons(List<Triangle> badTriangles)
    {
        var polygonEdges = new List<Edge>();

        foreach (var badTriangle in badTriangles)
        {
            foreach (var edge in badTriangle.Edges)
            {
                var edgeIsSharedWithOtherBadTriangles = badTriangles.Where(bt => bt.HasEdge(edge)).Count() > 1;

                if (edgeIsSharedWithOtherBadTriangles == false)
                    polygonEdges.Add(edge);
            }
        }
        return polygonEdges;
    }

    private void RemoveBadTrianglesFromTriangulation(List<Triangle> badTriangles)
    {
        for (int i = 0; i < badTriangles.Count; i++)
        {
            var currentBadTriangle = badTriangles[i];

            if (Triangulation.Contains(currentBadTriangle))
                Triangulation.Remove(currentBadTriangle);
        }
    }

    private void CreateNewTriangles(XYPoint currentPoint, List<Edge> polygons)
    {
        foreach (var polygonEdge in polygons)
        {

            var a = new Edge(polygonEdge.startPoint, currentPoint);
            var b = new Edge(currentPoint, polygonEdge.endPoint);
            var c = new Edge(polygonEdge.endPoint, polygonEdge.startPoint);

            var newTriangle = new Triangle { Edges = new List<Edge> { a, b, c } };

            currentTriangles.Add(newTriangle);
            Triangulation.Add(newTriangle);
        }
    }

    private void RemoveSuperTriangleVertices(Triangle superTriangle)
    {
        var uniqueSuperTriangleVertices = new HashSet<XYPoint> { superTriangle.Edges[0].startPoint,
                                                                     superTriangle.Edges[1].startPoint,
                                                                     superTriangle.Edges[2].startPoint,
                                                                   };

        var trianglesToBeRemoved = new List<Triangle>();
        foreach (var triangle in Triangulation)
        {
            foreach (var edge in triangle.Edges)
            {
                var anyEdgesEqualToSuperTriangleVertices = uniqueSuperTriangleVertices.Contains(edge.startPoint)
                                                           || uniqueSuperTriangleVertices.Contains(edge.endPoint);

                if (anyEdgesEqualToSuperTriangleVertices)
                    trianglesToBeRemoved.Add(triangle);
            }
        }

        trianglesToBeRemoved.ForEach(t => Triangulation.Remove(t));
    }
}
