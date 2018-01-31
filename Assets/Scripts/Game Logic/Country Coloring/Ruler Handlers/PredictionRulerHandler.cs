using System;
using System.Collections.Generic;
using System.Linq;
using Classifiers;
using Dataformatter.Dataprocessing.Entities;
using Game_Logic;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using NaiveBayesClassifier;
using Predicting.Nearest_Neighbours_Classifier;
using NaiveBayesClassifier;
using UnityEngine;

namespace Predicting
{
    public class PredictionRulerHandler : AbstractRulerHandler
    {
        public int CurrentYear;

        public override void Init()
        {
            ThisCountriesInfo = gameObject.GetComponent<CountryInformationReference>();
            CurrentRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);

            IsInitialized = true;
        }

        public override void HandleRuler()
        {
            //If we are either not ready yet, or done predicting, we dont do anything.            
            if (!IsInitialized || CurrentRuler.PartyClassification != "unknown")
                return;
            
            var trainingSet = new List<Record>();

            var startingYear = YearCounter.MinimumYear + 1;
            var endingYear = YearCounter.MaximumYear - 1;
            for (int currentYear = startingYear; currentYear < endingYear; currentYear++)
            {
                //Calculating the curr and previous year, taking boundary cases into consideration
                int previousYear;

                if (currentYear != startingYear)
                    previousYear = currentYear - 1;
                else
                    previousYear = startingYear - 1;
                
                //(temporarily) Setting the CurrentRulingParty to the winner of the current elections
                var currentElections = ThisCountriesInfo.AllElectionsEverForThisCountry.Where(e => e.Year == CurrentYear);
                var currentDictators = ThisCountriesInfo.AllDictatorshipsEverForThisCountry.Where(e => e.From >= CurrentYear
                                                                                                       && e.To <= CurrentYear);
                if (currentElections.Any())
                {
                    CurrentRuler = currentElections.OrderByDescending(e => e.TotalVotePercentage).First();
                }
                else if (currentDictators.Any())
                {
                    CurrentRuler = currentDictators.First();
                }

                //Retrieving the currently ruling party's political family
                CurrentYear = currentYear;
                var currentCountrysPoliticalFamily = CurrentRuler.PartyClassification;
                
                //Adding the combination of current ruling family + predictor values to the training set
                trainingSet.Add(new Record(currentCountrysPoliticalFamily, 
                                ThisCountriesInfo.GetPredictorFactors(previousYear, currentYear)));
            }
            
            
            //Creating the classifier, plus the record for the current (to be predicted) state of this country
            //var naiveBayesClassifier = new NaiveBayesClassifier.NaiveBayesClassifier(trainingSet);
            var classificationRecordForThisCountry = new Record("Unknown", ThisCountriesInfo.GetPredictorFactors(YearCounter.MaximumYear - 1, YearCounter.MaximumYear));

            AbstractClassifier classifier;
            
            //Todo: Make this cleaner and generic
            if(ClassifierOptions.Classifier == "KNN")
            {
                classifier = new KNN(trainingSet, ClassifierOptions.K);
            }
            else if(ClassifierOptions.Classifier == "NaiveBayes")
            {
                classifier = new NaiveBayesClassifier.NaiveBayesClassifier(trainingSet);
            }
            else
            {
                classifier = new ID3(trainingSet);         
            }
            
            
            //Finally, getting the most likely future classification and setting the ruler to be of that family.
            var predictedClassification = classifier.GetClassification(classificationRecordForThisCountry);
            CurrentRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
            CurrentRuler.PartyClassification = predictedClassification;

            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(CurrentRuler.PartyClassification);
        }
        
        public override string RulerToText()
        {
            return "We predict that a party of type " + CurrentRuler.PartyClassification + "\n will rule after " + YearCounter.MaximumYear + ".";
        }

        public bool IsReady()
        {
            return IsInitialized;
        }
    }
}