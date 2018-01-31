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
        private Dropdown Dropdown;
        private GameObject KSlider;
        private GameObject KText;
        private const string KTextDefaultText = "Value of K: ";
        
        private GameObject GoButton;

        private void Start()
        {
            Canvas = GameObject.Find("Canvas");
            Dropdown = Canvas.transform.Find("Dropdown").gameObject.GetComponent<Dropdown>();
            KSlider = Canvas.transform.Find("KSlider").gameObject;
            KText = Canvas.transform.Find("KText").gameObject;
            KText.GetComponent<Text>().text = KTextDefaultText;
            GoButton = Canvas.transform.Find("GoButton").gameObject;

            //Adding listeners
            Dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
            KSlider.GetComponent<Slider>().onValueChanged.AddListener(delegate { KSliderValueChanged(); });
            
            //Calling the on value listener to ensure the default value is used when the user does not click the changer at all.
            DropdownValueChanged();
            //Same for K slider
            KSliderValueChanged();
        }

        private void DropdownValueChanged()
        {
            var chosenClassifier = Dropdown.options[Dropdown.value].text;

            if (ClassifierOptions.Classifiers.Contains(chosenClassifier))
            {
                Debug.Log("Setting classifier to " + chosenClassifier);
                ClassifierOptions.Classifier = chosenClassifier;
            }

            Debug.Log("chosenClassifier = " + chosenClassifier);
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

        //Changes the text value of K in the UI
        private void KSliderValueChanged()
        {
            KText.GetComponent<Text>().text = KTextDefaultText + (int) KSlider.GetComponent<Slider>().value;
        }
    }
}