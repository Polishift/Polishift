using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Predicting.NeuralNetworkClassifier
{
    public class NeuralNetwork
    {
        public int[] LayerSizes;
        public int AmountOfLayers;

        public List<NeuralMatrix> biases = new List<NeuralMatrix>();
        public List<NeuralMatrix> weights = new List<NeuralMatrix>();


        public NeuralNetwork(int[] layerSizes)
        {
            AmountOfLayers = layerSizes.Length;
            LayerSizes = layerSizes;

            //Initializing the biases per layer
            for (int i = 1; i < AmountOfLayers; i++)
            {
                var biasesForThisLayer = new NeuralMatrix();
                biasesForThisLayer.layerNumber = i;

                var dimensions = new Dataformatter.Misc.Tuple<int, int>(LayerSizes[i], 1);
                biasesForThisLayer.dimensions = dimensions;
                biasesForThisLayer.RandomlyInitializeValues();

                biases.Add(biasesForThisLayer);
            }

            //Initializing the weights per layer
            var allButFirstLayerSizes = LayerSizes.Skip(1).ToArray();
            var allButLastLayerSizes = LayerSizes.Reverse().Skip(1).Reverse().ToArray();

            var sizesZip = allButFirstLayerSizes.Zip(allButLastLayerSizes, (size1, size2) => new Dataformatter.Misc.Tuple<int, int>(size1, size2)).ToArray();

            //todo not sure if randomness here is correct
            for (int j = 0; j < sizesZip.Length; j++)
            {
                var weightsForThisLayer = new NeuralMatrix();
                weightsForThisLayer.layerNumber = j;

                weightsForThisLayer.dimensions = sizesZip[j];
                weightsForThisLayer.RandomlyInitializeValues();

                weights.Add(weightsForThisLayer);
            }
        }

        public void StochasticGradientDescent(List<Tuple<double[][], string>> trainingInput, int epochAmount, int miniBatchAmount, double learningRate)
        {
            for (int i = 0; i < epochAmount; i++)
            {
                trainingInput.Shuffle();

                //Splitting the training data into minibatches
                List<MiniBatch> miniBatches = new List<MiniBatch>();
                for (int k = 0; k < trainingInput.Count; k += miniBatchAmount)
                {
                    var newMiniBatch = new MiniBatch();
                    newMiniBatch.data = trainingInput.GetRange(k, (k + miniBatchAmount));
                    miniBatches.Add(newMiniBatch); 
                }
                
                
            }
        }
        
        public override string ToString()
        {
            var returnStr = "";
            var count = 0;

            //Loop through the layers
            foreach (var weightsMatrix in weights)
            {
                count++;

                returnStr += "{ Weights Layer no. " + weightsMatrix.layerNumber + " \n";
                returnStr += "{ \n";
                foreach (var weightSet in weightsMatrix.values)
                {
                    for (int i = 0; i < weightSet.Length; i++)
                    {
                        returnStr += weightSet[0] + " | ";
                    }
                }
                returnStr += "\n }";
                returnStr += "\n }";
            }
            return returnStr;
        }

        public double[][] Sigmoid(double[][] z)
        {
            var returnVals = new double[z.Length][];

            for (int i = 0; i < z.Length; i++)
            {
                returnVals[i] = new double[z[i].Length];

                for (int j = 0; j < z[i].Length; j++)
                {
                    returnVals[i][j] = 1 / (1 + Math.Exp(-z[i][j]));
                }
            }
            return returnVals;
        }

        public double[][] SigmoidPrime(double[][] z)
        {
            return Operators.MultiplyMatrix(Sigmoid(z) * (1 - Sigmoid(z)));
        }

        
        
        
        private double[][] FeedForward(double[][] inputValues)
        {
            var biasAndWeightsMatricesZip = biases.Zip(weights, (s, w) => new Tuple<NeuralMatrix, NeuralMatrix>(s, w));

            foreach (var biasWeightsCombo in biasAndWeightsMatricesZip)
            {
                var biasesOfThisCombo = biasWeightsCombo.Item1.values;
                var weightsOfThisCombo = biasWeightsCombo.Item2.values;

                var dotProductOfWeightsAndInputs = Operators.MultiplyMatrix(weightsOfThisCombo, inputValues);

                inputValues = Sigmoid(Operators.SumMatrix(dotProductOfWeightsAndInputs, biasesOfThisCombo));
            }
            return inputValues;
        }
    }
}