using System;
using System.Collections.Generic;
using UnityEngine;

namespace Predicting
{
    
    //Static so its values dont change between scenes
    public static class ClassifierOptions
    {
        public static readonly string[] Classifiers = new string[3] {"KNN", "NaiveBayes", "ID3"};
        
        
        public static string Classifier = Classifiers[0]; //KNN is default
        
        //KNN only
        public static int K = 7;
    }
}