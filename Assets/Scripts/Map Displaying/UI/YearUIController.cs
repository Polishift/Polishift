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
        
        //todo set canvas and buttons to topright

        //Add Child foreach button
        private void Start()
        {
            //AddChildButtonToCanvas("Play", YearCounter.Play, this.transform.position);

            AddChildCanvas();
            AddChildButtonToCanvas("Pause", PauseButtonSprite, YearCounter.Pause, this.transform.position);
        }

        void AddChildCanvas()
        {
            GameObject canvasChild = new GameObject();
            canvasChild.name = "UI_Canvas";
            canvasChild.transform.position = transform.position;
            canvasChild.transform.parent = this.gameObject.transform;

            canvasChild.AddComponent<Canvas>();
        }
        
        void AddChildButtonToCanvas(string text, Sprite buttonSprite, Action buttonAction, Vector3 position)
        {
            GameObject newChild = new GameObject();
            newChild.name = text + "_button";
            newChild.transform.position = position;
            newChild.transform.parent = this.gameObject.transform.Find("UI_Canvas");

            newChild.AddComponent<Button>();
            newChild.AddComponent<Image>();
            newChild.AddComponent<Text>();

            //Executes the specified action on the click
            newChild.GetComponent<Button>().onClick.AddListener(buttonAction.Invoke);
            newChild.GetComponent<Image>().sprite = buttonSprite;
            newChild.GetComponent<Text>().text = text;
        }

        void OnGUI()
        {
            //todo update legacy, use the child canvas instead
            //GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), YearCounter.GetCurrentYear().ToString());
        }
    }
}