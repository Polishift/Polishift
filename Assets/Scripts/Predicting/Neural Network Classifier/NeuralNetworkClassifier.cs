using System;
using System.Collections.Generic;
using System.Linq;
using NaiveBayesClassifier;
using UnityEngine;

namespace Predicting.NeuralNetworkClassifier
{
    public class NeuralNetworkClassifier : AbstractClassifier
    {
        private List<Record> _trainingSet;
        
        public NeuralNetworkClassifier(List<Record> trainingSet)
        {
            _trainingSet = base.SanitizeTrainingSet(trainingSet);    
        }

        public override string GetClassification(Record r)
        {
            return "unknown";
        }
    }
}