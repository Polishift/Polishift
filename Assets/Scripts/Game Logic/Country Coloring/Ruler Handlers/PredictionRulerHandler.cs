using System.Collections.Generic;
using System.Linq;
using Classifiers;
using Dataformatter.Dataprocessing.Entities;
using Game_Logic;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using NaiveBayesClassifier;
using Predicting.Nearest_Neighbours_Classifier;
using UnityEngine;

namespace Predicting
{
    public class PredictionRulerHandler : AbstractRulerHandler
    {
        //This static var is used to display the global accuracy of the current classifier in the UI.
        public static decimal CorrectClassifications;
        public static decimal TotalRecordCount;
        
        
        public int CurrentYear;

        public override void Init()
        {
            ThisCountriesInfo = gameObject.GetComponent<CountryInformationReference>();
            CurrentRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);

            IsInitialized = true;
        }

        //This should only be called once when dealing with the prediction handler!
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
                
                //Making sure the classification doesnt go on forever
                IsDone = true;
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
            
            //Updating static (!) correctness percentage here, by comparing  predictedClassification to actual classification
            UpdateCorrectnessPercentage(predictedClassification, ThisCountriesInfo.FutureRulerClassification);

            //And finally finally, setting the color to be that of the predicted classification.
            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(CurrentRuler.PartyClassification);
        }

        private static void UpdateCorrectnessPercentage(string predicted, string actual)
        {
            var predictedLower = predicted.ToLower();
            var actualLower = actual.ToLower();

            //Bit hacky, but used to balance out the dictatorships which the classifier cant reasonably predict.
            if (!actualLower.Equals("unknown"))
            {
                TotalRecordCount++;

                if (predictedLower.Equals(actualLower))
                    CorrectClassifications += 1;
            }
        }
        
        
        
        /*
            UI METHODS 
        */
        
        public override string RulerToText()
        {
            var lowerClassification = CurrentRuler.PartyClassification.ToLower();
            var prettifiedPredictedClassification = PrettifiedPartyClassifications.prettifiedPartyClassifications[lowerClassification];
            var prettifiedActualClassification = PrettifiedPartyClassifications.prettifiedPartyClassifications[ThisCountriesInfo.FutureRulerClassification];
            
            return "We predict that a party of type \n\n'" + prettifiedPredictedClassification + "'\n\n will rule after " + YearCounter.MaximumYear + ".\n"
                    + GetClassificationCorrectnessText(prettifiedPredictedClassification, prettifiedActualClassification);
        }

        private string GetClassificationCorrectnessText(string predicted, string actual)
        {
            if (predicted.Equals(actual))
                return "This prediction is correct.";
            return "This prediction is incorrect,\n the actual ruling party is " + actual;
        }
        
        
    }
}