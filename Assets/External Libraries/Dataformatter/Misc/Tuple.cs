using System;

namespace Dataformatter
{
    public class Tuple<T,J> 
    {
        public T Item1 { get; set; }
        public J Item2 { get; set; }
        
        public Tuple(T Item1, J Item2)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;            
        }
    }
}