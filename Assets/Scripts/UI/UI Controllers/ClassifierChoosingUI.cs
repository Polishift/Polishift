using System;
using System.Linq;
using Predicting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
 {
     public class ClassifierChoosingUI : MonoBehaviour
     {
         private GameObject Canvas;
         private GameObject Dropdown;
         private GameObject KSlider;
         private GameObject KText;
         private GameObject GoButton;
 
         private void Start()
         {
             Canvas = GameObject.Find("Canvas");
             Dropdown = Canvas.transform.Find("Dropdown").gameObject;
             KSlider = Canvas.transform.Find("KSlider").gameObject;
             KText = Canvas.transform.Find("KText").gameObject;
             GoButton = Canvas.transform.Find("Button").gameObject;

         }
 
         //If KNN, ask for K
         public void Update()
         {
             var chosenClassifier = Dropdown.GetComponent<UnityEngine.UI.Dropdown>().itemText.text;
             
             if (ClassifierOptions.Classifiers.Contains(chosenClassifier))
                 ClassifierOptions.Classifier = chosenClassifier;

             if (chosenClassifier.Equals("KNN"))
             {
                 //enable knn sliders and add that info to stat class
                 KText.SetActive(true);
                 KSlider.SetActive(true);

                 ClassifierOptions.K = (int) KSlider.GetComponent<Slider>().value;
             }
             else
             {
                 KText.SetActive(false);
                 KSlider.SetActive(false);
             }
         }
     }
 }