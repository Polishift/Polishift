using System;
using System.Collections.Generic;
using System.Linq;

namespace NaiveBayesClassifier
{
    public class Record
    {
        public readonly string Classification;
        public readonly Dictionary<string, bool> Predictors = new Dictionary<string, bool>();

        
        public Record(string classification)
        {
            this.Classification = classification;
        }

        public Record(string classification, Dictionary<string, bool> predictors)
        {
            this.Classification = classification;
            this.Predictors = predictors;
        }

        public double GetEuclideanDistance(Record anotherRecord)
        {
            if(!HasTheSamePredictors(anotherRecord))
                return double.PositiveInfinity;
            
            var euclidDistance = 0.0;

            foreach (var predictorKv in this.Predictors)
            {
                var thisPredictorsValue = Convert.ToInt32(predictorKv.Value);
                var otherRecordsPredictorValue = Convert.ToInt32(anotherRecord.Predictors[predictorKv.Key]);

                euclidDistance += Math.Pow((thisPredictorsValue - otherRecordsPredictorValue), 2);
            }
            return Math.Sqrt(euclidDistance);
        }

        private bool HasTheSamePredictors(Record anotherRecord)
        {
            foreach (var predictorKey in this.Predictors.Keys)
            {
                if (anotherRecord.Predictors.ContainsKey(predictorKey) == false)
                    return false;
            }
            return true;
        }

        
        
        public override string ToString()
        {
            var predictorValueString = "";
            Predictors.ToList().ForEach(p => predictorValueString += (p.Key + " = " + p.Value + "\n"));

            return Classification + ": { " + predictorValueString + " } ";
        }
    }
}