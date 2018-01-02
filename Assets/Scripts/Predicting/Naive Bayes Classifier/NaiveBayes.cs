using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace NaiveBayesClassifier
{
    public class NaiveBayesClassifier
    {
        private List<Record> _trainingSet;

        private readonly Dictionary<string, int> _classificationCounts = new Dictionary<string, int>();

        private readonly Table<string, double> _frequencyTable = new Table<string, double>();
        private readonly Table<string, double> _likelihoodTable = new Table<string, double>();

        public NaiveBayesClassifier(List<Record> trainingSet)
        {
            foreach (var r in trainingSet)
            {
                Debug.Log("Record from training set has class " + r.classification);                
            }

            SanitizeTrainingSet(trainingSet);

            CreateFrequencyTableAndCountClassifications();
            CreateLikelihoodTable();
        }

        private void SanitizeTrainingSet(List<Record> trainingSet)
        {
            var trainingSetWithoutUnknowns = trainingSet.Where(t => !t.classification.Equals("unknown")).ToList();
            
            //if all training set records are unknown, thje trainingsetWithoutUnknowns would be empty.
            //to avoid nulls, we substitute it with a one record-only list.
            if (trainingSetWithoutUnknowns.Count() <= 0)
                trainingSetWithoutUnknowns.Add(new Record("unknown", new Dictionary<string, bool>()));

            _trainingSet = trainingSetWithoutUnknowns;
        }
        

        /**
        * Classification creation 
        **/

        public string GetClassification(Record recordToBeClassified)
        {
            var likelihoodsPerClassification = new Dictionary<string, double>();
            
            foreach (var possibleClassification in _frequencyTable.rowsPerColumn.Keys)
            {
                var thisClassificationsProbability = GetProbabilityOfGivenClassification(possibleClassification, recordToBeClassified);
                likelihoodsPerClassification.Add(possibleClassification, thisClassificationsProbability);
            }

            var sortedLikelihoodsPerClassification = likelihoodsPerClassification.OrderByDescending(kv => kv.Value).ToList();
            var mostLikelyClassification = sortedLikelihoodsPerClassification.First();

            return mostLikelyClassification.Key;
        }

        private double GetProbabilityOfGivenClassification(string currentPossibleClassification, Record recordToBeClassified)
        {
            var priorClassProbability = GetClassPriorProbability(currentPossibleClassification);
            double finalProbability = priorClassProbability;

            foreach (var predictorKV in _likelihoodTable.rowsPerColumn[currentPossibleClassification])
            {
                var predictorsName = predictorKV.Key;
                var predictorLikelihood = predictorKV.Value;

                if (recordToBeClassified.predictors[predictorsName] == true)
                    finalProbability = finalProbability * predictorLikelihood;
            }
            return finalProbability;
        }


        /**
        Table creation
        **/

        private void CreateFrequencyTableAndCountClassifications()
        {
            foreach (var record in _trainingSet)
            {
                //populating count dict
                if (!_classificationCounts.ContainsKey(record.classification))
                {
                    _classificationCounts.Add(record.classification, 1);
                }
                else
                    _classificationCounts[record.classification] += 1;

                //populating frequency table
                _frequencyTable.PotentiallyAddClassification(record.classification);

                foreach (var predictor in record.predictors)
                {
                    if (!_frequencyTable.rowsPerColumn[record.classification].ContainsKey(predictor.Key))
                        _frequencyTable.rowsPerColumn[record.classification].Add(predictor.Key, 0);

                    if (predictor.Value == true)
                    {
                        _frequencyTable.rowsPerColumn[record.classification][predictor.Key] += 1;
                    }
                }
            }
        }

        private void CreateLikelihoodTable()
        {
            foreach (var record in _frequencyTable.rowsPerColumn)
            {
                var classificationOfItem = record.Key;
                _likelihoodTable.PotentiallyAddClassification(classificationOfItem);

                foreach (var predictor in record.Value)
                {
                    var likelihoodOfThisPredictor = predictor.Value / GetCountsOfClassification(_frequencyTable, classificationOfItem);

                    if (!_likelihoodTable.rowsPerColumn[classificationOfItem].ContainsKey(predictor.Key))
                        _likelihoodTable.rowsPerColumn[classificationOfItem].Add(predictor.Key, likelihoodOfThisPredictor);
                    else
                        _likelihoodTable.rowsPerColumn[classificationOfItem][predictor.Key] = likelihoodOfThisPredictor;
                }
            }
        }

        private int GetCountsOfClassification(Table<string, double> table, string classification)
        {
            return _classificationCounts[classification];
        }


        /**
        Formula computation
        **/

        private double GetClassPriorProbability(string classification)
        {
            return (double) _classificationCounts[classification] / _trainingSet.Count();
        }
    }
}