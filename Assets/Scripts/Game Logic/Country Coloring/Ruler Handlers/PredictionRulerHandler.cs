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
        public ElectionEntity CurrentRulingParty;

        public override void Init()
        {
            ThisCountriesInfo = gameObject.GetComponent<CountryInformationReference>();
            CurrentRulingParty = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);

            IsInitialized = true;
        }

        public override void HandleRuler()
        {
            //If we are either not ready yet, or done predicting, we dont do anything.            
            if (!IsInitialized || CurrentRulingParty.PartyClassification != "unknown")
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
                    CurrentRulingParty = currentElections.OrderByDescending(e => e.TotalVotePercentage).First();
                }

                //Retrieving the currently ruling party's political family
                CurrentYear = currentYear;
                var currentCountrysPoliticalFamily = CurrentRulingParty.PartyClassification;
                
                //Adding the combination of current ruling family + predictor values to the training set
                trainingSet.Add(new Record(currentCountrysPoliticalFamily, 
                                ThisCountriesInfo.GetPredictorFactors(previousYear, currentYear)));
            }
            
            //Creating the classifier, plus the record for the current (to be predicted) state of this country
            var naiveBayesClassifier = new NaiveBayesClassifier.NaiveBayesClassifier(trainingSet);
            var classificationRecordForThisCountry = new Record("Unknown", ThisCountriesInfo.GetPredictorFactors(YearCounter.MaximumYear - 1, YearCounter.MaximumYear));
            
            //Finally, getting the most likely future classification and setting the ruler to be of that family.
            var predictedClassification = naiveBayesClassifier.GetClassification(classificationRecordForThisCountry);
            CurrentRulingParty = ElectionEntity.GetEmptyElectionEntity(ThisCountriesInfo.Iso3166Country);
            CurrentRulingParty.Year = YearCounter.MaximumYear;
            CurrentRulingParty.PartyClassification = predictedClassification;

            gameObject.GetComponent<CountryColorer>().UpdateCountryColorForNewRuler(CurrentRulingParty.PartyClassification);
        }

        public bool IsReady()
        {
            return IsInitialized;
        }
    }
}