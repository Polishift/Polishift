using System;

namespace Predicting.NeuralNetworkClassifier
{
    public class NeuralMatrix
    {
        private static Random rand = new Random();

        public int layerNumber;
        public Dataformatter.Misc.Tuple<int, int> dimensions;

        public double[][] values; //first index is that of neurons of the rightmost layer. 
        //IE. values [1][0] is the value between the second neuron of the rightmost and the first neuron of the leftmost layer.

        public void RandomlyInitializeValues()
        {
            values = new double[dimensions.Item1][];

            double mean = 0;
            double stdDev = 1;

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = new double[dimensions.Item2];

                for (int j = 0; j < values[i].Length; j++)
                {
                    double u1 = 1.0 - rand.NextDouble();
                    double u2 = 1.0 - rand.NextDouble();
                    double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                    double randNormal = mean + stdDev * randStdNormal;

                    values[i][j] = randNormal;
                }
            }
        }
    }
}