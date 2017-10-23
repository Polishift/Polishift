using System.Collections.Generic;
using System.Linq;

namespace Dataformatter.Datamodels
{
    public class Polygon<T> where T: IPoint
    {
        public List<T> Points = new List<T>();

        public Polygon()
        {
        }

        public Polygon(List<T> points)
        {
            Points = points;
        }

        public override string ToString()
        {
            var listString = Points.Aggregate("", (current, point) => current + "\n" + point.ToString());

            return listString;
        }
    }
}