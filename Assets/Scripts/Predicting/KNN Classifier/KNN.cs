using System.Collections.Generic;
using System.Linq;
using NaiveBayesClassifier;

namespace Predicting.Nearest_Neighbours_Classifier
{
    public class KNN : AbstractClassifier
    {
        private List<Record> _trainingSet;
        private readonly int K;
        
        public KNN(List<Record> trainingSet)
        {
            K = 4;
            _trainingSet = base.SanitizeTrainingSet(trainingSet);    
        }
            
        public override string GetClassification(Record newRecord)
        {
            //Getting all the distances
            Dictionary<Record, double> distancesToNeighbours = new Dictionary<Record, double>();
            foreach (var alreadyClassifiedRecord in _trainingSet)
            {
                var distanceToThisClassifiedRecord = newRecord.GetEuclideanDistance(alreadyClassifiedRecord);
                
                distancesToNeighbours.Add(alreadyClassifiedRecord, distanceToThisClassifiedRecord);
            }

            //Getting the K amount of closest neighbours
            var KNeighbours = distancesToNeighbours.OrderByDescending(n => n.Value).Take(K);

            //Counting the classifications of the neighbours
            var classificationsCounts = new Dictionary<string, int>();
            foreach (var neighbour in KNeighbours)
            {
                var neighboursClassification = neighbour.Key.Classification;

                if (classificationsCounts.ContainsKey(neighboursClassification) == false)
                    classificationsCounts.Add(neighboursClassification, 1);
                else
                    classificationsCounts[neighboursClassification] += 1;
            }
            
            //Returning the classification which happens most among the neighbours
            return classificationsCounts.ToList().OrderByDescending(c => c.Value).First().Key;
        }
    }
}