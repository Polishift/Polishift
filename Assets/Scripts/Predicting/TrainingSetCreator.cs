using System.Collections.Generic;
using Game_Logic;
using Game_Logic.Country_Coloring;
using Map_Displaying.Reference_Scripts;
using NaiveBayesClassifier;
using UnityEngine;

namespace Predicting
{
    public static class TrainingSetCreator
    {
        public static List<Record> CreateTrainingSet(GameObject country)
        {
            var trainingSetForThisCountry = new List<Record>();

            var countriesInfo = country.GetComponent<CountryInformationReference>();
            var countriesElectionHandler = country.GetComponent<CountryElectionHandler>();

            for (int currentYear = YearCounter._minimumYear; currentYear < YearCounter._maximumYear; currentYear++)
            {
                int previousYear;

                if (currentYear != YearCounter._minimumYear)
                    previousYear = currentYear - 1;
                else
                    previousYear = currentYear;    
                
                //Retrieving the currently ruling family
                countriesElectionHandler.CurrentYear = currentYear;
                var currentCountrysPoliticalFamily = countriesElectionHandler.CurrentCountryRuler.PartyClassification;
                
                //Creating the record for the predictors training set
                var recordForThisYearsCountry = new Record(currentCountrysPoliticalFamily);
                recordForThisYearsCountry.predictors = countriesInfo.CreatePredictionFactors(previousYear, currentYear);
                trainingSetForThisCountry.Add(recordForThisYearsCountry);
            }
            return trainingSetForThisCountry;
        }
        
    }
}