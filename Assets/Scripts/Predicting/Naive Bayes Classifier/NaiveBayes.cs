using System;
using System.Linq;
using System.Collections.Generic;
using Predicting;
using UnityEngine;

namespace NaiveBayesClassifier
{
    public class NaiveBayesClassifier : AbstractClassifier
    {
        private List<Record> _trainingSet;

        private readonly Dictionary<string, int> _classificationCounts = new Dictionary<string, int>();

        private readonly Table<string, double> _frequencyTable = new Table<string, double>();
        private readonly Table<string, double> _likelihoodTable = new Table<string, double>();

        public NaiveBayesClassifier(List<Record> trainingSet)
        {
            foreach (var r in trainingSet)
                Debug.Log("Record from training set has class " + r.Classification);

            _trainingSet = base.SanitizeTrainingSet(trainingSet);

            CreateFrequencyTableAndCountClassifications();
            CreateLikelihoodTable();
        }
        

        /**
        * Classification creation 
        **/

        public override string GetClassification(Record recordToBeClassified)
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

                if (recordToBeClassified.Attributes[predictorsName] == 1)
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
                if (!_classificationCounts.ContainsKey(record.Classification))
                {
                    _classificationCounts.Add(record.Classification, 1);
                }
                else
                    _classificationCounts[record.Classification] += 1;

                //populating frequency table
                _frequencyTable.PotentiallyAddClassification(record.Classification);

                foreach (var predictor in record.Attributes)
                {
                    if (!_frequencyTable.rowsPerColumn[record.Classification].ContainsKey(predictor.Key))
                        _frequencyTable.rowsPerColumn[record.Classification].Add(predictor.Key, 0);

                    if (predictor.Value == 1)
                    {
                        _frequencyTable.rowsPerColumn[record.Classification][predictor.Key] += 1;
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