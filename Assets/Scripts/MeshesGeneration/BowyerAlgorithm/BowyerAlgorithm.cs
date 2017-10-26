using System.Collections.Generic;
using System.Linq;
using Dataformatter;
using Dataformatter.Datamodels;
using Dataformatter.Misc;
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

            var counter = 0;
            foreach (var currentPoint in _inputPoints)
            {
                //debug
                Debug.Log("Current point: " + currentPoint.ToString());
                counter++;
                //end debug

                var badTriangles = DetermineBadTriangles(currentPoint);
                var polygonsWithOriginalTriangles = DeterminePolygons(badTriangles);

                RemoveBadTrianglesFromTriangulation(badTriangles);
                CreateNewTriangles(currentPoint, polygonsWithOriginalTriangles);


                //debug
                for (var i = 0; i < _newTrianglesForCurrentIteration.Count(); i++)
                {
                    for (var j = 0; j < _newTrianglesForCurrentIteration[i].Edges.Count(); j++)
                        RemoveIntersectingEdges(_currentTriangles, _newTrianglesForCurrentIteration[i].Edges[j]);
                }
                //end debug

                _newTrianglesForCurrentIteration.Clear();

                if(counter == 4)
                    break;
            }
            RemoveSuperTriangleVertices(superTriangle);

            return _triangulation;
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
        Replace an intersecter with the Edge that it intersects (?)
        */


        private void RemoveIntersectingEdges(List<Triangle> trianglesToCheck, Edge subjectEdge)
        {
            var guiltyTrianglesAndEdgesWithTheirReplacements = new Dictionary<Triangle, List<Tuple<int, Edge>>>();

            foreach (var triangle in trianglesToCheck)
            {
                guiltyTrianglesAndEdgesWithTheirReplacements.Add(triangle, new List<Tuple<int, Edge>>());

                for (var i = 0; i < triangle.Edges.Count; i++)
                {
                    var currEdge = triangle.Edges[i];

                    if (currEdge.CrossesThrough(subjectEdge))
                    {
                        Debug.Log("We'll replace " + currEdge + " with " + subjectEdge + " since it intersects with " + subjectEdge);
                        currEdge.IS_BAD = true;
                        guiltyTrianglesAndEdgesWithTheirReplacements[triangle].Add(new Tuple<int,Edge>(i, subjectEdge));
                    }
                }
            }

            //Replacing the edges
            foreach (var triangle in _triangulation)
            {
                if (guiltyTrianglesAndEdgesWithTheirReplacements.ContainsKey(triangle))
                {
                    for (var i = 0; i < guiltyTrianglesAndEdgesWithTheirReplacements[triangle].Count; i++)
                    {
                        var guiltyEdgeForThisTriangle = guiltyTrianglesAndEdgesWithTheirReplacements[triangle][i].Item1; 
                        var replacingEdge = guiltyTrianglesAndEdgesWithTheirReplacements[triangle][i].Item2; 

                        replacingEdge.IS_BAD = false;
                        triangle.Edges.RemoveAt(guiltyEdgeForThisTriangle);
                        triangle.Edges.Add(replacingEdge);          
                    }
                }
            }
        }
    }
}