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
        if(Intersects(anotherEdge))
            return LineGoesThroughEdge(anotherEdge);
        else
            return false;
    }

    public XYPoint GetMidPoint()
    {
        var firstPointDivision = (startPoint.X + startPoint.Y) / 2;
        var secondPointDivision = (endPoint.X + endPoint.Y) / 2; 
         
        return new XYPoint() { X = firstPointDivision, Y = secondPointDivision };
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

    private bool LineGoesThroughEdge(Edge anotherEdge)
    {
        var interSectionPoint = GetIntersectionPoint(anotherEdge);
        var distanceToIntersectionPoint = this.startPoint.EuclideanDistance(interSectionPoint);

        return distanceToIntersectionPoint > 0
               && distanceToIntersectionPoint < this.Length;
    }

    public XYPoint GetIntersectionPoint(Edge anotherEdge)
    {
        var xOne = this.startPoint.X;
        var yOne = this.startPoint.Y;
        var xTwo = this.endPoint.X;
        var yTwo = this.endPoint.Y;
        
        var xThree = anotherEdge.startPoint.X;
        var yThree = anotherEdge.startPoint.Y;
        var xFour = anotherEdge.endPoint.X;
        var yFour = anotherEdge.endPoint.Y;

        var xDividend = (((xTwo*yOne) - (xOne*yTwo)) * (xFour - xThree)) - 
                        (((xFour*yThree) - (xThree*yFour)) * (xTwo - xOne));

        var xDivisor = ((xTwo - xOne) * (yFour - yThree)) - 
                       ((xFour - xThree) * (yTwo - yOne));

        var yDividend = (((xTwo*yOne) - (xOne*yTwo)) * (yFour - yThree)) - 
                        (((xFour*yThree) - (xThree*yFour)) * (yTwo - yOne));

        var yDivisor = ((xTwo - xOne) * (yFour - yThree)) - 
                       ((xFour - xThree) * (yTwo - yOne));
        

        return new XYPoint { X = xDividend/xDivisor, Y = yDividend/yDivisor};
    }
}
