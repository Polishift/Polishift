using System;
using System.Linq;
using System.Collections.Generic;

using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Datamodels;

public class Triangle
{
    private double CircumCircleRadius;

    public List<Edge> Edges = new List<Edge>();

    public bool IsWithinCircumCircle(XYPoint point)
    {
        var aX = Edges[0].startPoint.X - point.X;
        var aY = Edges[0].startPoint.Y - point.Y;

        var bX = Edges[0].endPoint.X - point.X;
        var bY = Edges[0].endPoint.Y - point.Y;

        var cX = Edges[1].endPoint.X - point.X;
        var cY = Edges[1].endPoint.Y - point.Y;

        var det = (
                    (aX * aX + aY * aY) * (bX * cY - cX * bY) -
                    (bX * bX + bY * bY) * (aX * cY - cX * aY) +
                    (cX * cX + cY * cY) * (aX * bY - bX * aY)
                  );

        //God knows why this works            
        return det < 0;
    }

    public bool HasEdge(Edge anotherEdge)
    {
        for (int i = 0; i < this.Edges.Count; i++)
        {
            var currentEdge = this.Edges[i];

            if (currentEdge.Equals(anotherEdge))
                return true;
        }
        return false;
    }

    public override string ToString()
    {
        string result = " | ";
        foreach (var edge in Edges)
            result += edge.ToString() + " ";

        result += " | ";
        return result;
    } 
    /*
    private double ComputeCircumCircleRadius()
    {
        var a = edges[0].EuclideanDistance(edges[1]);
        var b = edges[1].EuclideanDistance(edges[2]);
        var c = edges[2].EuclideanDistance(edges[0]);

        var s = (a + b + c) / 2;

        var radiusDividend = (a * b) * c;
        var radiusDivisor = 4 * Math.Sqrt((s * (s - a)) * (s - b) * (s - c));

        return radiusDividend / radiusDivisor;
    }
    */
}
