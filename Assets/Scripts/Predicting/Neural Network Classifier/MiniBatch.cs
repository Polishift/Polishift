using System.Collections.Generic;
using Dataformatter.Misc;

namespace Predicting.NeuralNetworkClassifier
{
    public class MiniBatch
    {
        public List<Tuple<double[][], string>> data = new List<Tuple<double[][], string>>();

        public void UpdateMiniBatch(double learningRate)
        {
        }
    }
}