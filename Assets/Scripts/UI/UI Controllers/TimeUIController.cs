using System;
using Game_Logic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
{
    public class TimeUIController : MonoBehaviour
    {
        public Sprite PauseButtonSprite;
        public Sprite PlayButtonSprite;
        public Sprite FastbackwardButtonSprite;
        public Sprite FastforwardButtonSprite;


        private void Update()
        {
            var currentYear = YearCounter.GetCurrentYear().ToString();
            gameObject.transform.Find("TimeControl_Canvas").transform.Find("Year Counter").GetComponent<Text>().text = currentYear;
        }

        private void Start()
        {
            this.transform.position = Vector3.zero;

            CreateTimeControlsCanvas();

            AddYearCounter();
            AddChildButtonToCanvas("Pause Button", PauseButtonSprite, YearCounter.Pause, new Vector3(380, 140, 3300), 0.55f);
            AddChildButtonToCanvas("Play Button", PlayButtonSprite, YearCounter.Play, new Vector3(450, 140, 3300), 0.55f);
            AddChildButtonToCanvas("FFW Button", FastforwardButtonSprite, YearCounter.FastForward, new Vector3(450, 80, 3300), 0.45f);
            AddChildButtonToCanvas("FBW Button", FastbackwardButtonSprite, YearCounter.FastBackward, new Vector3(380, 80, 3300), 0.45f);
        }


        void CreateTimeControlsCanvas()
        {
            GameObject canvasChild = new GameObject();
            canvasChild.name = "TimeControl_Canvas";
            canvasChild.transform.parent = this.gameObject.transform;

            canvasChild.AddComponent<Canvas>();
            canvasChild.AddComponent<CanvasScaler>();
            canvasChild.AddComponent<GraphicRaycaster>();

            canvasChild.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasChild.GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            canvasChild.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasChild.GetComponent<CanvasScaler>().referenceResolution = new Vector2(800, 600);
            canvasChild.GetComponent<CanvasScaler>().referencePixelsPerUnit = 100;            
            canvasChild.GetComponent<CanvasScaler>().screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }

        void AddYearCounter()
        {
            GameObject yearCounter = new GameObject("Year Counter");
            yearCounter.transform.SetParent(GameObject.Find("TimeControl_Canvas").transform);

            //Setting transform
            RectTransform trans = yearCounter.AddComponent<RectTransform>();
            trans.localScale = new Vector3(1, 1, 1);
            trans.anchoredPosition = new Vector3(400, 200, 3300);

            //Setting text options
            Text text = yearCounter.AddComponent<Text>();
            text.text = "1945";
            text.fontSize = 60;
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.material = new Material(Shader.Find("UI/Default_Overlay"));
            text.color = Color.black;

            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }


        void AddChildButtonToCanvas(string name, Sprite buttonSprite, Action buttonAction, Vector3 anchoredPosition, float scale)
        {
            GameObject newChildButton = new GameObject(name);
            newChildButton.transform.SetParent(GameObject.Find("TimeControl_Canvas").transform);

            //Setting transform
            RectTransform trans = newChildButton.AddComponent<RectTransform>();
            trans.localScale = new Vector3(scale, scale, 1);
            trans.anchoredPosition = anchoredPosition;

            newChildButton.AddComponent<Image>();
            newChildButton.AddComponent<Button>();

            newChildButton.GetComponent<Image>().sprite = buttonSprite;
            newChildButton.GetComponent<Button>().targetGraphic = newChildButton.GetComponent<Image>();
            newChildButton.GetComponent<Button>().onClick.AddListener(buttonAction.Invoke);
        }
    }
}