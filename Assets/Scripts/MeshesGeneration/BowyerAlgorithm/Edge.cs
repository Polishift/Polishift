using System;
using UnityEngine;
using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Datamodels;


public class Edge
{
    private double Length { get; set; }
    public XYPoint startPoint { get; set; }
    public XYPoint endPoint { get; set; }

    public Edge(XYPoint startPoint, XYPoint endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.Length = startPoint.EuclideanDistance(endPoint);
    }

    public bool CrossesThrough(Edge anotherEdge)
    {
        return Intersects(anotherEdge) && LineGoesThroughEdge();
    }

    public override bool Equals(object obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Edge edge = obj as Edge;
        if ((System.Object)edge == null)
        {
            return false;
        }

        return edge.startPoint.Equals(this.startPoint) && edge.endPoint.Equals(this.endPoint);
    }

    public override string ToString()
    {
        return startPoint.ToString() + " to " + endPoint.ToString();
    }

    /*
    private bool Intersects(Edge anotherEdge)
    {
        var a = this.startPoint;
        var b = this.endPoint;
        var c = anotherEdge.startPoint;
        var d = anotherEdge.endPoint;

        var aSide = (d.X - c.X) * (a.Y - c.Y) - (d.Y - c.Y) * (a.X - c.X) > 0;
        var bSide = (d.X - c.X) * (b.Y - c.Y) - (d.Y - c.Y) * (b.X - c.X) > 0;
        var cSide = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X) > 0;
        var dSide = (b.X - a.X) * (d.Y - a.Y) - (b.Y - a.Y) * (d.X - a.X) > 0;

        return aSide != bSide && cSide != dSide;
    }
    */

    private XYPoint GetIntersectionPoint(Edge anotherEdge)
    {
        
    }

    private bool LineGoesThroughEdge()
    {
        //if length of originating line > length from starting point to intersection point
    }
}
