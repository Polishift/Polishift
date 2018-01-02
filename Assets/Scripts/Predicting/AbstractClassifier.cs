using System.Collections.Generic;
using System.Linq;
using NaiveBayesClassifier;

namespace Predicting
{
    public abstract class AbstractClassifier
    {
        public abstract string GetClassification(Record r);
        
        protected List<Record> SanitizeTrainingSet(List<Record> rawTrainingSet)
        {
            var sanitizedTrainingSet = rawTrainingSet.Where(t => !t.Classification.Equals("unknown")).ToList();
            
            //if all training set records are unknown, thje trainingsetWithoutUnknowns would be empty.
            //to avoid nulls, we substitute it with a one record-only list.
            if (sanitizedTrainingSet.Any() == false)
                sanitizedTrainingSet.Add(new Record("unknown", new Dictionary<string, bool>()));

            return sanitizedTrainingSet;
        }
    }
}