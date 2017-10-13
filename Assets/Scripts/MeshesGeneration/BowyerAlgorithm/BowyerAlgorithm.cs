using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using Dataformatter.Datamodels;


public class BowyerAlgorithm
{
    private Triangle superTriangle;
    private List<XYPoint> inputPoints;
    private HashSet<Triangle> triangulation = new HashSet<Triangle>();
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

        int counter = 0;
        foreach (var currentPoint in inputPoints)
        {
            //debug
            Debug.Log("Current point: " + counter + ": " + currentPoint.ToString());
            Debug.Log("we have triangles: ");
            currentTriangles.ForEach(t => Debug.Log(t.ToString()));

            counter++;
            //end debug

            List<Triangle> badTriangles = DetermineBadTriangles(currentPoint);
            Dictionary<Triangle, List<Edge>> polygonsWithOriginalTriangles = DeterminePolygons(badTriangles);

            RemoveBadTrianglesFromTriangulation(badTriangles);
            CreateNewTriangles(currentPoint, polygonsWithOriginalTriangles);

            //if(counter >= 3)
                //break;
        }
        RemoveSuperTriangleVertices(superTriangle);

        return this.triangulation;
    }

    //TODO: Change these points to be float min max's
    private void CreateSuperTriangle()
    {
        var lowerLeftCorner = new XYPoint { X = -100, Y = -20 };
        var lowerRightCorner = new XYPoint { X = 300, Y = -20 };
        var pyramidTop = new XYPoint
        {
            X = 100,
            Y = 200
        };

        var a = new Edge(lowerLeftCorner, pyramidTop);
        var b = new Edge(pyramidTop, lowerRightCorner);
        var c = new Edge(lowerRightCorner, lowerLeftCorner);
        var superTriangle = new Triangle();
        superTriangle.Edges = new List<Edge> { a, b, c };

        this.superTriangle = superTriangle;
        currentTriangles.Add(superTriangle);
        triangulation.Add(superTriangle);
    }

    private List<Triangle> DetermineBadTriangles(XYPoint currentPoint)
    {
        var badTriangles = new List<Triangle>();

        foreach (var triangle in currentTriangles)
        {
            if (triangle.IsWithinCircumCircle(currentPoint))
                badTriangles.Add(triangle);
        }
        return badTriangles;
    }

    private Dictionary<Triangle, List<Edge>> DeterminePolygons(List<Triangle> badTriangles)
    {
        var polygonEdgesWithOriginalTriangles = new Dictionary<Triangle, List<Edge>>();

        foreach (var badTriangle in badTriangles)
        {
            polygonEdgesWithOriginalTriangles.Add(badTriangle, new List<Edge>());

            foreach (var edge in badTriangle.Edges)
            {
                var edgeIsSharedWithOtherBadTriangles = badTriangles.Where(bt => bt.HasEdge(edge)).Count() > 1;
                RemoveIntersectingEdges(edge);

                if (edgeIsSharedWithOtherBadTriangles == false)
                    polygonEdgesWithOriginalTriangles[badTriangle].Add(edge);

            }
        }
        return polygonEdgesWithOriginalTriangles;
    }

    private void RemoveBadTrianglesFromTriangulation(List<Triangle> badTriangles)
    {
        for (int i = 0; i < badTriangles.Count; i++)
        {
            var currentBadTriangle = badTriangles[i];

            if (currentTriangles.Contains(currentBadTriangle))
            {
                triangulation.Remove(currentBadTriangle);
                currentTriangles.Remove(currentBadTriangle);
            }
        }
    }

    private void CreateNewTriangles(XYPoint currentPoint, Dictionary<Triangle, List<Edge>> allTrianglesWithPolygonEdges)
    {
        foreach (var triangleWithPolygonEdges in allTrianglesWithPolygonEdges)
        {
            foreach (var edge in triangleWithPolygonEdges.Value)
            {
                var a = new Edge(edge.startPoint, currentPoint);
                var b = new Edge(currentPoint, edge.endPoint);
                var c = new Edge(edge.endPoint, edge.startPoint);

                var newTriangle = new Triangle { Edges = new List<Edge> { a, b, c } };

                currentTriangles.Add(newTriangle);
                triangulation.Add(newTriangle);
            }
        }
    }

    private void RemoveSuperTriangleVertices(Triangle superTriangle)
    {
        var uniqueSuperTriangleVertices = new HashSet<XYPoint> { superTriangle.Edges[0].startPoint,
                                                                     superTriangle.Edges[1].startPoint,
                                                                     superTriangle.Edges[2].startPoint,
                                                                   };

        var trianglesToBeRemoved = new List<Triangle>();
        foreach (var triangle in triangulation)
        {
            foreach (var edge in triangle.Edges)
            {
                var anyEdgesEqualToSuperTriangleVertices = uniqueSuperTriangleVertices.Contains(edge.startPoint)
                                                           || uniqueSuperTriangleVertices.Contains(edge.endPoint);

                if (anyEdgesEqualToSuperTriangleVertices)
                    trianglesToBeRemoved.Add(triangle);
            }
        }

        trianglesToBeRemoved.ForEach(t => triangulation.Remove(t));
    }

    private void RemoveIntersectingEdges(Edge e)
    {
        var guiltyTrianglesAndEdges = new Dictionary<Triangle, List<int>>();

        foreach (var triangle in currentTriangles)
        {
            guiltyTrianglesAndEdges.Add(triangle, new List<int>());

            for (int i = 0; i < triangle.Edges.Count; i++)
            {
                var currEdge = triangle.Edges[i];

                if(currEdge.CrossesThrough(e))
                {
                    Debug.Log("We'll remove " + currEdge.ToString() + " since it intersects with " + e.ToString());
                    guiltyTrianglesAndEdges[triangle].Add(i);
                }
            }
        }

        //TODO: Actually remove these edges, and we'll be golden :)
        /*
        foreach (var triangle in triangulation)
        {
            for(int i = 0; i < guiltyTrianglesAndEdges[triangle].Count; i++)
            {
                triangle.Edges.RemoveAt(guiltyTrianglesAndEdges[triangle][i]);
            }
        }
         */
    }
}
