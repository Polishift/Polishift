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

            var counter = 0;
            foreach (var currentPoint in _inputPoints)
            {
                //debug
                Debug.Log("Point no " + counter + ": " + currentPoint.ToString());
                counter++;
                //end debug

                var badTriangles = DetermineBadTriangles(currentPoint);
                var polygonsWithOriginalTriangles = DeterminePolygons(badTriangles);

                RemoveBadTrianglesFromTriangulation(badTriangles);
                CreateNewTriangles(currentPoint, polygonsWithOriginalTriangles);

                foreach (var item in _currentTriangles)
                    Debug.Log("BEFORE INTERSECT: Triangle " + item.ToString());


                //debug
                for (int i = 0; i < _newTrianglesForCurrentIteration.Count(); i++)
                {
                    for (int j = 0; j < _newTrianglesForCurrentIteration[i].Edges.Count(); j++)
                        RemoveIntersectingEdges(_currentTriangles, _newTrianglesForCurrentIteration[i].Edges[j], currentPoint);
                }
                //end debug

                foreach (var item in _currentTriangles)
                    Debug.Log("AFTER INTERSECT: Triangle " + item.ToString());


                _newTrianglesForCurrentIteration.Clear();


                //1, 2, Are good and their tri's are good. 3 However seems to do the circumcircle detection wrong...
                if (counter == 2)
                    break;
            }
            //RemoveSuperTriangleVertices(superTriangle);

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
                    Debug.Log(currentPoint.ToString() + " is within circumcircle of " + triangle.ToString());
                    badTriangles.Add(triangle);
                }
                else
                    Debug.Log(currentPoint.ToString() + " is NOT within circumcircle of " + triangle.ToString());

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
            var guiltyTrianglesAndEdgesWithTheirReplacements = new Dictionary<Triangle, List<Tuple<int, Edge>>>();

            foreach (var triangle in trianglesToCheck)
            {
                guiltyTrianglesAndEdgesWithTheirReplacements.Add(triangle, new List<Tuple<int, Edge>>());

                for (var i = 0; i < triangle.Edges.Count; i++)
                {
                    var currEdge = triangle.Edges[i];

                    if (currEdge.CrossesThrough(subjectEdge))
                    {
                        var replacementEdge = CreateReplacingEdge(currEdge, currentPoint); 

                        currEdge.IS_BAD = true;
                        guiltyTrianglesAndEdgesWithTheirReplacements[triangle].Add(new Tuple<int, Edge>(i, replacementEdge));
                    }
                }
            }

            //Replacing the edges
            foreach (var triangle in _triangulation)
            {
                if (guiltyTrianglesAndEdgesWithTheirReplacements.ContainsKey(triangle))
                {
                    for (int i = 0; i < guiltyTrianglesAndEdgesWithTheirReplacements[triangle].Count; i++)
                    {
                        var guiltyEdgeForThisTriangleIndex = guiltyTrianglesAndEdgesWithTheirReplacements[triangle][i].Item1;
                        var replacingEdge = guiltyTrianglesAndEdgesWithTheirReplacements[triangle][i].Item2;

                        replacingEdge.IS_BAD = false;
                        Debug.Log("Triangle " + triangle.ToString() + " is going to replace edge " + triangle.Edges[guiltyEdgeForThisTriangleIndex] + " with edge " + replacingEdge.ToString());

                        triangle.ReplaceEdge(replacingEdge, guiltyEdgeForThisTriangleIndex);
                    }
                }
            }
        }

        private Edge CreateReplacingEdge(Edge edgeToReplace, XYPoint currentPoint)
        {
            //Making sure supertriangle points are never replaced

            if(_superTriangle.Edges.Any(e => e.StartPoint == edgeToReplace.EndPoint 
                                        || e.EndPoint == edgeToReplace.EndPoint))
            {
                return new Edge(currentPoint, edgeToReplace.EndPoint);
            }
            else
                return new Edge(edgeToReplace.StartPoint, currentPoint);
            
        }
    }
}