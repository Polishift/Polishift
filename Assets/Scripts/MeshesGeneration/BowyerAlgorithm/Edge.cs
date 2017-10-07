using System;

using Dataformatter.Data_accessing.Factories.ModelFactories;
using Dataformatter.Data_accessing.Repositories;
using Dataformatter.Datamodels;


public class Edge
{
    public XYPoint startPoint { get; set; }
    public XYPoint endPoint { get; set; }

    public Edge(XYPoint startPoint, XYPoint endPoint)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }

    public override bool Equals(object obj) //maybe diz?
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
}
