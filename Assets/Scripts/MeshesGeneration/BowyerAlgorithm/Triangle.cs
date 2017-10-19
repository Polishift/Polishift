using System.Collections.Generic;
using Dataformatter.Datamodels;

namespace MeshesGeneration.BowyerAlgorithm
{
    public class Triangle
    {
        private double _circumCircleRadius;
        private XYPoint _circumCenter;

        public List<Edge> Edges = new List<Edge>();


        public bool IsWithinCircumCircle(XYPoint point)
        {
            var aX = Edges[0].StartPoint.X - point.X;
            var aY = Edges[0].StartPoint.Y - point.Y;

            var bX = Edges[0].EndPoint.X - point.X;
            var bY = Edges[0].EndPoint.Y - point.Y;

            var cX = Edges[1].EndPoint.X - point.X;
            var cY = Edges[1].EndPoint.Y - point.Y;

            var det =
                    (aX * aX + aY * aY) * (bX * cY - cX * bY) -
                    (bX * bX + bY * bY) * (aX * cY - cX * aY) +
                    (cX * cX + cY * cY) * (aX * bY - bX * aY)
                ;

            //God knows why this works            
            return det < 0;
        }

        public bool HasEdge(Edge anotherEdge)
        {
            for (var i = 0; i < Edges.Count; i++)
            {
                var currentEdge = Edges[i];

                if (currentEdge.Equals(anotherEdge))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            var result = " | ";
            foreach (var edge in Edges)
                result += edge + " ";

            result += " | ";
            return result;
        }
    }
}