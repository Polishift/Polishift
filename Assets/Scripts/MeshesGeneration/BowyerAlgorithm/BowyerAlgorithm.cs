using System.Collections.Generic;
using System.Linq;
using Dataformatter;
using Dataformatter.Datamodels;
using UnityEngine;

namespace MeshesGeneration.BowyerAlgorithm
{
    public class BowyerAlgorithm
    {
        private Triangle _superTriangle;
        private List<XYPoint> _inputPoints;
        private List<Triangle> _newTrianglesForCurrentIteration = new List<Triangle>();
        private readonly HashSet<Triangle> _triangulation = new HashSet<Triangle>();
        private readonly List<Triangle> _currentTriangles = new List<Triangle>();


        public BowyerAlgorithm(List<XYPoint> inputPoints)
        {
            _inputPoints = inputPoints;
        }

        /*
        Todo:
        If a new edge crosses through an existing edge,
        remove the existing edge.
        */


        public HashSet<Triangle> ComputeFinalTriangulation()
        {
            CreateSuperTriangle();
            var superTriangle = _currentTriangles[0];

            _inputPoints = _inputPoints.OrderBy(p => p.X).ToList();

            foreach (var currentPoint in _inputPoints)
            {
                var badTriangles = DetermineBadTriangles(currentPoint);
                var polygonsWithOriginalTriangles = DeterminePolygons(badTriangles);

                RemoveBadTrianglesFromTriangulation(badTriangles);
                CreateNewTriangles(currentPoint, polygonsWithOriginalTriangles);

                //Removing intersecting triangles
                for (int i = 0; i < _newTrianglesForCurrentIteration.Count(); i++)
                {
                    for (int j = 0; j < _newTrianglesForCurrentIteration[i].Edges.Count(); j++)
                        RemoveIntersectingEdges(_currentTriangles, _newTrianglesForCurrentIteration[i].Edges[j], currentPoint);
                }

                _newTrianglesForCurrentIteration.Clear();
            }
            RemoveSuperTriangleVertices(superTriangle);

            return _triangulation;
        }

        //TODO: Change these points to be float min max's
        private void CreateSuperTriangle()
        {
            var lowerLeftCorner = new XYPoint { X = -100, Y = -20 };
            var lowerRightCorner = new XYPoint { X = 220, Y = -20 };
            var pyramidTop = new XYPoint
            {
                X = 55,
                Y = 200
            };

            var a = new Edge(lowerLeftCorner, pyramidTop);
            var b = new Edge(pyramidTop, lowerRightCorner);
            var c = new Edge(lowerRightCorner, lowerLeftCorner);
            var superTriangle = new Triangle { Edges = new List<Edge> { a, b, c } };

            _superTriangle = superTriangle;
            _currentTriangles.Add(superTriangle);
            _triangulation.Add(superTriangle);
        }

        private List<Triangle> DetermineBadTriangles(XYPoint currentPoint)
        {
            var badTriangles = new List<Triangle>();

            foreach (var triangle in _currentTriangles)
            {
                if (triangle.IsWithinCircumCircle(currentPoint))
                {
                    Debug.Log("Triangle " + triangle + " is bad");
                    badTriangles.Add(triangle);
                }
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

                    if (edgeIsSharedWithOtherBadTriangles == false)
                        polygonEdgesWithOriginalTriangles[badTriangle].Add(edge);
                }
            }
            return polygonEdgesWithOriginalTriangles;
        }

        private void RemoveBadTrianglesFromTriangulation(List<Triangle> badTriangles)
        {
            for (var i = 0; i < badTriangles.Count; i++)
            {
                var currentBadTriangle = badTriangles[i];

                if (_currentTriangles.Contains(currentBadTriangle))
                {
                    _triangulation.Remove(currentBadTriangle);
                    _currentTriangles.Remove(currentBadTriangle);
                }
            }
        }

        private void CreateNewTriangles(XYPoint currentPoint,
            Dictionary<Triangle, List<Edge>> allTrianglesWithPolygonEdges)
        {
            foreach (var triangleWithPolygonEdges in allTrianglesWithPolygonEdges)
            {
                foreach (var edge in triangleWithPolygonEdges.Value)
                {
                    var a = new Edge(edge.StartPoint, currentPoint);
                    var b = new Edge(currentPoint, edge.EndPoint);
                    var c = new Edge(edge.EndPoint, edge.StartPoint);

                    var newTriangle = new Triangle { Edges = new List<Edge> { a, b, c } };

                    _newTrianglesForCurrentIteration.Add(newTriangle);
                    _currentTriangles.Add(newTriangle);
                    _triangulation.Add(newTriangle);
                }
            }
        }

        private void RemoveSuperTriangleVertices(Triangle superTriangle)
        {
            var uniqueSuperTriangleVertices = new HashSet<XYPoint>
            {
                superTriangle.Edges[0].StartPoint,
                superTriangle.Edges[1].StartPoint,
                superTriangle.Edges[2].StartPoint,
            };

            var trianglesToBeRemoved = new List<Triangle>();
            foreach (var triangle in _triangulation)
            {
                foreach (var edge in triangle.Edges)
                {
                    var anyEdgesEqualToSuperTriangleVertices = uniqueSuperTriangleVertices.Contains(edge.StartPoint)
                                                               || uniqueSuperTriangleVertices.Contains(edge.EndPoint);

                    if (anyEdgesEqualToSuperTriangleVertices)
                        trianglesToBeRemoved.Add(triangle);
                }
            }

            trianglesToBeRemoved.ForEach(t => _triangulation.Remove(t));
        }



        /*
        Replace an intersecters endpoint with the current point
        */

        private void RemoveIntersectingEdges(List<Triangle> trianglesToCheck, Edge subjectEdge, XYPoint currentPoint) //eww so many arguments
        {
            var guiltyTrianglesAndEdgesWithTheirReplacements = new Dictionary<Triangle, Triangle>();

            foreach (var triangle in trianglesToCheck)
            {
                for (var i = 0; i < triangle.Edges.Count; i++)
                {
                    var currEdge = triangle.Edges[i];

                    if (currEdge.CrossesThrough(subjectEdge))
                    {
                        var replacingTriangle = CreateReplacingTriangle(triangle, currentPoint);
                        guiltyTrianglesAndEdgesWithTheirReplacements[triangle] = replacingTriangle;

                        Debug.Log("Triangle no. " + i + "'s (" + triangle + ") edge " + currEdge + " intersects " + subjectEdge);
                        break; //Going to check next triangle now; 
                               //since we get a complete replacment triangle there's no need to check the other edges.
                    }
                }
            }

            //Replacing the edges
            foreach (var triangle in _triangulation)
            {
                if (guiltyTrianglesAndEdgesWithTheirReplacements.ContainsKey(triangle))
                {
                    var replacingTrianglesEdges = guiltyTrianglesAndEdgesWithTheirReplacements[triangle].Edges;
                    triangle.Edges = replacingTrianglesEdges;
                }
            }
        }

        private Triangle CreateReplacingTriangle(Triangle triangleToReplace, XYPoint currentPoint)
        {
            //Making sure supertriangle points are never replaced,
            //and any non-supertriangle points are set to be the currentPoint.
            var replacementTriangle = new Triangle() { Edges = new List<Edge>() };

            foreach (var edgeToReplace in triangleToReplace.Edges)
            {
                if (_superTriangle.Edges.Any(e => e.Equals(edgeToReplace)))
                {
                    //Both points of this edge are supertriangle edges, so we leave the edge be.
                    replacementTriangle.Edges.Add(new Edge(edgeToReplace.StartPoint, edgeToReplace.EndPoint));
                }
                else if (_superTriangle.Edges.Any(e => e.StartPoint == edgeToReplace.EndPoint
                                         || e.EndPoint == edgeToReplace.EndPoint))
                {
                    replacementTriangle.Edges.Add(new Edge(currentPoint, edgeToReplace.EndPoint));
                }
                else
                    replacementTriangle.Edges.Add(new Edge(edgeToReplace.StartPoint, currentPoint));
            }

            return replacementTriangle;
        }
    }
}