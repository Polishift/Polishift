using System;
using System.Collections.Generic;
using System.Linq;

namespace NaiveBayesClassifier
{
    public class Record
    {
        public readonly string Classification;
        public readonly Dictionary<string, int> Attributes = new Dictionary<string, int>();

        
        public Record(string classification)
        {
            this.Classification = classification;
        }

        public Record(string classification, Dictionary<string, int> attributes)
        {
            this.Classification = classification;
            this.Attributes = attributes;
        }

        public double GetEuclideanDistance(Record anotherRecord)
        {
            if(!HasTheSamePredictors(anotherRecord))
                return double.PositiveInfinity;
            
            var euclidDistance = 0.0;

            foreach (var predictorKv in this.Attributes)
            {
                var thisPredictorsValue = Convert.ToInt32(predictorKv.Value);
                var otherRecordsPredictorValue = Convert.ToInt32(anotherRecord.Attributes[predictorKv.Key]);

                euclidDistance += Math.Pow((thisPredictorsValue - otherRecordsPredictorValue), 2);
            }
            return Math.Sqrt(euclidDistance);
        }

        private bool HasTheSamePredictors(Record anotherRecord)
        {
            foreach (var predictorKey in this.Attributes.Keys)
            {
                if (anotherRecord.Attributes.ContainsKey(predictorKey) == false)
                    return false;
            }
            return true;
        }

        
        
        public override string ToString()
        {
            var predictorValueString = "";
            Attributes.ToList().ForEach(p => predictorValueString += (p.Key + " = " + p.Value + "\n"));

            return Classification + ": { " + predictorValueString + " } ";
        }
    }
}