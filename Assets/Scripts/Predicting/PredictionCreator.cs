using System;
using System.Collections.Generic;
using Game_Logic;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using NaiveBayesClassifier;
using UnityEngine;

namespace Predicting
{
    [RequireComponent(typeof(CountryInformationReference), typeof(CountryElectionHandler), typeof(CountryColorer))]
    public class PredictionCreator : MonoBehaviour
    {
        public List<Record> TrainingSet;
        
        //create trainingset. The classification is the current political family. The factors are all the things from the repo's
        public void Predict()
        {
            var thisCountriesInfo = gameObject.GetComponent<CountryInformationReference>();

            var classificationRecordForThisCountry = new Record("Unknown", thisCountriesInfo.GetPredictorFactors(YearCounter.MaximumYear - 1, YearCounter.MaximumYear));
            TrainingSet = thisCountriesInfo.CreateTrainingSetForThisCountry();
            var predictedClassification = GetPredictedClassification(classificationRecordForThisCountry);
            
            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(predictedClassification);
        }

        private string GetPredictedClassification(Record recordToBeClassified)
        {
            var classifier = new NaiveBayesClassifier.NaiveBayesClassifier(TrainingSet);
            return classifier.GetClassification(recordToBeClassified);
        }
    }
}