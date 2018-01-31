using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public static class UICreator
    {
        private static GameObject UI_Canvas;

        static UICreator()
        {
            UI_Canvas = GameObject.Find("UI_Canvas");
        }
        
        public static void DestroyUI()
        {
            foreach (Transform child in GameObject.Find("UI_Canvas").transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        
        public static void AddChildButtonToCanvas(string name, Sprite buttonSprite, Sprite pressedButtonSprite, Action buttonAction, Vector3 anchoredPosition, float scale)
        {
            GameObject newChildButton = new GameObject(name);
            newChildButton.tag = "UI";
            newChildButton.transform.SetParent(GameObject.Find("UI_Canvas").transform);

            //Setting transform
            RectTransform trans = newChildButton.AddComponent<RectTransform>();
            trans.localScale = new Vector3(scale, scale, 1);
            trans.anchoredPosition = anchoredPosition;

            newChildButton.AddComponent<Image>();
            newChildButton.AddComponent<Button>();

            newChildButton.GetComponent<Image>().sprite = buttonSprite;
            newChildButton.GetComponent<Button>().targetGraphic = newChildButton.GetComponent<Image>();
            newChildButton.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
            newChildButton.GetComponent<Button>().spriteState = new SpriteState() {pressedSprite = pressedButtonSprite};
            
            newChildButton.GetComponent<Button>().onClick.AddListener(buttonAction.Invoke);
        }


        public static void AddTextToCanvas(string name, string textVal, int fontsize, Vector3 position, Vector2 widthHeight)
        {
            GameObject textObject = new GameObject(name);
            textObject.tag = "UI";
            textObject.transform.SetParent(UI_Canvas.transform);

            //Setting transform
            RectTransform trans = textObject.AddComponent<RectTransform>();
            trans.localScale = new Vector3(1, 1, 1);
            trans.anchoredPosition = position;
            trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, widthHeight.x);
            trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, widthHeight.y);

            //Setting text options
            Text text = textObject.AddComponent<Text>();
            text.text = textVal;
            text.fontSize = fontsize;
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            text.color = Color.black;

            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
        }

        
        public static void AddBackgroundPanelToCanvas(Sprite sprite, Vector3 anchoredPosition, Vector3 scale)
        {
            GameObject backgroundGameObject = new GameObject("Background image");
            backgroundGameObject.transform.SetParent(UI_Canvas.transform);
            
            //Setting transform
            RectTransform trans = backgroundGameObject.AddComponent<RectTransform>();
            trans.anchoredPosition = anchoredPosition;
            trans.localScale = scale;

            //setting sprite
            backgroundGameObject.AddComponent<Image>().sprite = sprite;
        }
    }
}