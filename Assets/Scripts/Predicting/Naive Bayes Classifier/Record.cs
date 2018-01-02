using System;
using System.Collections.Generic;
using System.Linq;

namespace NaiveBayesClassifier
{
    public class Record
    {
        public string classification;

        public Dictionary<string, bool> predictors = new Dictionary<string, bool>();

        
        public Record(string classification)
        {
            this.classification = classification;
        }

        public Record(string classification, Dictionary<string, bool> predictors)
        {
            this.classification = classification;
            this.predictors = predictors;
        }

        public override string ToString()
        {
            var predictorValueString = "";
            predictors.ToList().ForEach(p => predictorValueString += (p.Key + " = " + p.Value + "\n"));

            return classification + ": { " + predictorValueString + " } ";
        }
    }
}