using System;
using System.Collections.Generic;
using Game_Logic;
using NaiveBayesClassifier;
using UnityEngine;

namespace Predicting
{
    public class PredictionCreator
    {
        public string ToBePredictedCountryName;
        public List<Record> TrainingSet;
        
        //create trainingset. The classification is the current political family. The factors are all the things from the repo's
        public PredictionCreator()
        {
            var countryToBePredicted = GameObject.Find(ToBePredictedCountryName);
            TrainingSet = TrainingSetCreator.CreateTrainingSet(countryToBePredicted);
        }

        public string GetClassification(Record recordToBeClassified)
        {
            var classifier = new NaiveBayesClassifier.NaiveBayesClassifier(TrainingSet);
            return classifier.GetClassification(recordToBeClassified);
        }
    }
}