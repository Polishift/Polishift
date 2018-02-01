using System;
using System.Globalization;
using Predicting;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
{
    public class CorrectnessDisplayerUI : MonoBehaviour
    {
        private static readonly string GameObjectName = "CorrectnessPercentageText";

        private void Start()
        {
            UICreator.AddTextToCanvas(GameObjectName, "Correctness: 0.0%.", 15, new Vector3(299, 150), new Vector2(100, 100));
        }


        private void Update()
        {
            /*
            Debug.Log("PredictionRulerHandler.CorrectClassifications = " + PredictionRulerHandler.CorrectClassifications 
                      + ", PredictionRulerHandler.TotalRecordCount = " + PredictionRulerHandler.TotalRecordCount
                      + " 5/31 = " + (decimal)  PredictionRulerHandler.CorrectClassifications /  PredictionRulerHandler.TotalRecordCount);
            */

            if (PredictionRulerHandler.CorrectClassifications > 0 && PredictionRulerHandler.TotalRecordCount > 0)
            {
                var classificationCorrectnessPercentage = PredictionRulerHandler.CorrectClassifications / PredictionRulerHandler.TotalRecordCount;
                GameObject.Find(GameObjectName).GetComponent<Text>().text = "Correctness: " + classificationCorrectnessPercentage.ToString("P2", CultureInfo.CreateSpecificCulture("hr-HR"));
            }
        }
    }
}