using System.Collections.Generic;
using System.Linq;
using System;
using NaiveBayesClassifier;
using Predicting;

namespace Classifiers
{
    public class ID3 : AbstractClassifier
    {
        private List<Record> _trainingSet;
        private List<string> attributes;

        public Node ResultingTree;


        public ID3(List<Record> trainingSet)
        {
            this._trainingSet = base.SanitizeTrainingSet(trainingSet);

            //Setting the attributes
            attributes = _trainingSet[0].Attributes.Keys.ToList();

            ResultingTree = CreateTree(trainingSet, attributes);
        }

        public override string GetClassification(Record r)
        {
            var classifyingNode = TraverseTree(r, ResultingTree);
            return classifyingNode.Label;
        }

        private Node TraverseTree(Record r, Node n)
        {
            if(n.IsLeaf || n.Label != "")
                return n;
            else
            {
                var nodeAttribute = n.DecisionAttribute;
                var recordsValueForNodeAttribute = r.Attributes[nodeAttribute];

                try
                {
                    return TraverseTree(r, n.Children[recordsValueForNodeAttribute]);
                    
                }
                catch
                {
                    Console.WriteLine("Node " + n + " // doesn't have attribute value " + recordsValueForNodeAttribute);
                    return null;
                }
            }
        }

        private Node CreateTree(List<Record> set, List<string> attributes)
        {
            Node node = new Node();

            
            if (AreAllClassificationsAreTheSame(set))
            {
                //The record whose classification we are gonna use doesnt really matter since theyre all the same
                var homogenousClassification = set[0].Classification;
                    
                node.IsLeaf = true;
                node.Label = homogenousClassification;
                return node;
            }
            if (attributes.Any() == false)
            {
                node.IsLeaf = true;                
                node.Label = GetMostCommonClassification(set);
                return node;
            }
            else
            {
                //Computing the attribute with the most information gain.
                Dictionary<string, double> informationGainPerAttribute = GetAttributeWithMostInformationGain(set, attributes);
                var bestAttribute = informationGainPerAttribute.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;


                //Setting T's decision attr to be that attribute.
                node.DecisionAttribute = bestAttribute;


                foreach (var possibleValue in GetPossibleValuesForAttribute(bestAttribute, _trainingSet))
                {
                    //Using the subset to either recurse, or add a leaf
                    var subset = set.Where(r => r.Attributes[bestAttribute] == possibleValue).ToList();
                    if (subset.Any())
                    {
                        //Add a new tree branch below Root, corresponding to the test attribute = possibleValue                        
                        node.Children.Add(possibleValue, CreateTree(subset, attributes.Where(a => a != bestAttribute).ToList()));
                    }
                    else
                    {
                        var leafNode = new Node();
                        leafNode.IsLeaf = true;
                        leafNode.Label = GetMostCommonClassification(set);

                        node.Children.Add(possibleValue, leafNode);
                        return leafNode;
                    }
                }
            }
            return node;
        }



        /* MOST COMMON CLASSIFICATION / POSSIBLE VALUES PER ATTRIBUTE */
        private bool AreAllClassificationsAreTheSame(List<Record> set)
        {
            return set.Select(r => r.Classification).Distinct().Count() > 1;
        }
        
        private string GetMostCommonClassification(List<Record> set)
        {
            return set.GroupBy(i => i)
                      .OrderByDescending(g => g.Count())
                      .Take(1)
                      .Select(g => g.Key.Classification)
                      .First();
        }

        private List<int> GetPossibleValuesForAttribute(string attribute, List<Record> set)
        {
            var possibleValues = new HashSet<int>();

            foreach (var record in set)
            {
                var thisRecordsValueOfAttribute = record.Attributes[attribute];

                if (!possibleValues.Contains(thisRecordsValueOfAttribute))
                    possibleValues.Add(thisRecordsValueOfAttribute);
            }
            return possibleValues.ToList();
        }


        /* COMPUTING INFORMATION GAIN */

        private Dictionary<string, double> GetAttributeWithMostInformationGain(List<Record> set, List<string> allowedAttributes)
        {
            var returnDict = new Dictionary<string, double>();

            var setEntropy = ComputeEntropy(set);
            foreach (var attributeKV in set[0].Attributes.Where(a => allowedAttributes.Contains(a.Key)))
            {
                returnDict.Add(attributeKV.Key, ComputeInformationGain(attributeKV.Key, setEntropy));
            }
            return returnDict;
        }

        private double ComputeInformationGain(string currentAttribute, double entropyOfSet)
        {
            var informationGain = entropyOfSet;

            //Getting all the values of this attribute in the trainingset
            var allValuesOfThisAttribute = new List<int>();
            _trainingSet.ForEach(r => allValuesOfThisAttribute.Add(r.Attributes[currentAttribute]));

            foreach (var possibleValue in allValuesOfThisAttribute)
            {
                var recordsWithThisValueForCurrentAttribute = _trainingSet.Where(r => r.Attributes[currentAttribute] == possibleValue).ToList();

                informationGain -= (recordsWithThisValueForCurrentAttribute.Count / _trainingSet.Count) * ComputeEntropy(recordsWithThisValueForCurrentAttribute);
            }
            return informationGain;
        }


        /*
        Calculates a measure of how much the classifications of the dataset 'lean' to a certain classification label.
        Since we only use win and nowin, this is pretty easy.
        */
        private double ComputeEntropy(List<Record> set)
        {
            var total = set.Count;

            //Too literal, generalize this.
            var amountOfPositiveClassifications = set.Where(r => r.Classification == "won").Count();
            var amountOfNegativeClassifications = set.Where(r => r.Classification == "nowin").Count();

            double ratioPositive = (double)amountOfPositiveClassifications / total;
            double ratioNegative = (double)amountOfNegativeClassifications / total;

            if (ratioPositive != 0)
                ratioPositive = -(ratioPositive) * DoubleLog(ratioPositive);
            if (ratioNegative != 0)
                ratioNegative = -(ratioNegative) * DoubleLog(ratioNegative);

            return ratioPositive + ratioNegative;
        }

        private double DoubleLog(double x)
        {
            return Math.Log(x) / Math.Log(2);
        }
    }
}