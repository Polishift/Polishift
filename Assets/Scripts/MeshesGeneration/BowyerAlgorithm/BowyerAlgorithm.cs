using System.Collections.Generic;
using System.Linq;
using Dataformatter.Datamodels;
using UnityEngine;

namespace MeshesGeneration.BowyerAlgorithm
{
    public class BowyerAlgorithm
    {
        private Triangle _superTriangle;
        private List<XYPoint> _inputPoints;
        private readonly HashSet<Triangle> _triangulation = new HashSet<Triangle>();
        private readonly List<Triangle> _currentTriangles = new List<Triangle>();


        public BowyerAlgorithm(List<XYPoint> inputPoints)
        {
            _inputPoints = inputPoints;
        }

        /*
        Todo:

        Keep a hashset of new triangles per iteration.
        Exempt these from edge intersection checks, so only check bad triangles.
        */


        public HashSet<Triangle> ComputeFinalTriangulation()
        {
            CreateSuperTriangle();
            var superTriangle = _currentTriangles[0];

            _inputPoints = _inputPoints.OrderBy(p => p.X).ToList();

            var counter = 0;
            foreach (var currentPoint in _inputPoints)
            {
                //debug
                Debug.Log("Current point: " + counter + ": " + currentPoint);
                Debug.Log("we have triangles: ");
                _currentTriangles.ForEach(t => Debug.Log(t.ToString()));

                counter++;
                //end debug

                var badTriangles = DetermineBadTriangles(currentPoint);
                var polygonsWithOriginalTriangles = DeterminePolygons(badTriangles);

                RemoveBadTrianglesFromTriangulation(badTriangles);
                CreateNewTriangles(currentPoint, polygonsWithOriginalTriangles);

                //if(counter >= 3)
                //break;
            }
            RemoveSuperTriangleVertices(superTriangle);

            return _triangulation;
        }

        //TODO: Change these points to be float min max's
        private void CreateSuperTriangle()
        {
            var lowerLeftCorner = new XYPoint {X = -100, Y = -20};
            var lowerRightCorner = new XYPoint {X = 300, Y = -20};
            var pyramidTop = new XYPoint
            {
                X = 100,
                Y = 200
            };

            var a = new Edge(lowerLeftCorner, pyramidTop);
            var b = new Edge(pyramidTop, lowerRightCorner);
            var c = new Edge(lowerRightCorner, lowerLeftCorner);
            var superTriangle = new Triangle {Edges = new List<Edge> {a, b, c}};

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

                    var newTriangle = new Triangle {Edges = new List<Edge> {a, b, c}};

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

        private void RemoveIntersectingEdges(Edge e)
        {
            var guiltyTrianglesAndEdges = new Dictionary<Triangle, List<int>>();

            foreach (var triangle in _currentTriangles)
            {
                guiltyTrianglesAndEdges.Add(triangle, new List<int>());

                for (var i = 0; i < triangle.Edges.Count; i++)
                {
                    var currEdge = triangle.Edges[i];

                    if (currEdge.CrossesThrough(e))
                    {
                        Debug.Log("We'll remove " + currEdge + " since it intersects with " + e);
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
}