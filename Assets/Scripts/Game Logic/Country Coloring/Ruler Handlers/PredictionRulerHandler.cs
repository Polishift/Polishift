using System;
using System.Collections.Generic;
using System.Linq;
using Dataformatter.Dataprocessing.Entities;
using Map_Displaying.Reference_Scripts;
using NaiveBayesClassifier;
using UnityEngine;

namespace Game_Logic.Country_Coloring
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
                if (currentElections.Any())
                {
                    CurrentRuler = currentElections.OrderByDescending(e => e.TotalVotePercentage).First();
                }

                //Retrieving the currently ruling party's political family
                CurrentYear = currentYear;
                var currentCountrysPoliticalFamily = CurrentRuler.PartyClassification;
                
                //Adding the combination of current ruling family + predictor values to the training set
                trainingSet.Add(new Record(currentCountrysPoliticalFamily, 
                                ThisCountriesInfo.GetPredictorFactors(previousYear, currentYear)));
            }
            
            //Creating the classifier, plus the record for the current (to be predicted) state of this country
            var naiveBayesClassifier = new NaiveBayesClassifier.NaiveBayesClassifier(trainingSet);
            var classificationRecordForThisCountry = new Record("Unknown", ThisCountriesInfo.GetPredictorFactors(YearCounter.MaximumYear - 1, YearCounter.MaximumYear));
            
            //Finally, getting the most likely future classification and setting the ruler to be of that family.
            var predictedClassification = naiveBayesClassifier.GetClassification(classificationRecordForThisCountry);
            CurrentRuler = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
            CurrentRuler.PartyClassification = predictedClassification;

            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(CurrentRuler.PartyClassification);
        }
        
        public override string RulerToText()
        {
            return "We predict that a party of type " + CurrentRuler.PartyClassification + " will rule next.";
        }

        public bool IsReady()
        {
            return IsInitialized;
        }
    }
}