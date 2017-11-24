using System;
using Game_Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
{
    public class YearUIController : MonoBehaviour
    {
        public Sprite PauseButtonSprite;


        private void Update()
        {
            var currentYear = YearCounter.GetCurrentYear().ToString();
            gameObject.transform.Find("UI_Canvas").transform.Find("Year Counter").GetComponent<Text>().text =
                currentYear;
        }

        //todo set canvas and buttons to topright
        private void Start()
        {
            //AddChildButtonToCanvas("Play", YearCounter.Play, this.transform.position);

            AddChildCanvas();
            AddYearCounter();
            AddChildButtonToCanvas("PauseButton", PauseButtonSprite, YearCounter.Pause, this.transform.position);
        }

        void AddChildCanvas()
        {
            GameObject canvasChild = new GameObject();
            canvasChild.name = "UI_Canvas";
            canvasChild.transform.position = transform.position;
            canvasChild.transform.parent = this.gameObject.transform;

            canvasChild.AddComponent<Canvas>();
            canvasChild.AddComponent<CanvasScaler>();

            canvasChild.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            canvasChild.GetComponent<Canvas>().worldCamera =
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            
            canvasChild.GetComponent<CanvasScaler>().uiScaleMode =
                CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasChild.GetComponent<CanvasScaler>().screenMatchMode =
                CanvasScaler.ScreenMatchMode.Expand;
        }
        
        void AddYearCounter()
        {
            GameObject yearCounter = new GameObject();
            yearCounter.name = "Year Counter";
            yearCounter.transform.parent = this.gameObject.transform.Find("UI_Canvas");

            yearCounter.AddComponent<Text>();
            yearCounter.GetComponent<Text>().font = new Font("Arial");
            
            yearCounter.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        
        void AddChildButtonToCanvas(string name, Sprite buttonSprite, Action buttonAction, Vector3 position)
        {
            GameObject newChildButton = new GameObject();
            newChildButton.name = name;
            newChildButton.transform.parent = this.gameObject.transform.Find("UI_Canvas");

            newChildButton.AddComponent<Button>();
            newChildButton.AddComponent<Image>();

            //Executes the specified action on the click
            newChildButton.GetComponent<RectTransform>().localPosition = Vector3.zero;
            newChildButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            newChildButton.GetComponent<Button>().onClick.AddListener(buttonAction.Invoke);
            newChildButton.GetComponent<Image>().sprite = buttonSprite;
            
            //todo; position in topright and make clickable
        }
    }
}