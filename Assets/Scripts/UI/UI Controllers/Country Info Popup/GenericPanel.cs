using Dataformatter.Misc;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI.Country_Info_Popup
{
    public class GenericPanel
    {
        public static readonly GameObject UI_CANVAS = GameObject.Find("UI_Canvas");
        private GameObject _gameObjectForThisPanel;

        private string _panelsGameObjectName;
        private ImageInfo _backgroundImageInfo;
        private List<ButtonInfo> _buttons = new List<ButtonInfo>();
        private List<TextLabelInfo> _textLabels = new List<TextLabelInfo>();

        
        public void Create(Vector2 positionOnCanvas, 
                           string panelsGameObjectName, 
                           ImageInfo backgroundImage, 
                           List<TextLabelInfo> textLabels = null, 
                           List<ButtonInfo> buttons = null)
        {
            _panelsGameObjectName = panelsGameObjectName;
            _backgroundImageInfo = backgroundImage;

            if (buttons != null)
                _buttons = buttons;

            if (_textLabels != null)
                _textLabels = textLabels;
            
            
            CreatePanelGameObject(positionOnCanvas);
            
            AddBackgroundImageToPanel();
            AddButtonsToPanel();
            AddTextLabelsToPanel();
        }

        public void Destroy()
        {
            GameObject.Destroy(GameObject.Find(_panelsGameObjectName));
        }


        private void CreatePanelGameObject(Vector2 positionOnUICanvas)
        {
            //create gameObject for this panel
            _gameObjectForThisPanel = new GameObject(_panelsGameObjectName);
            _gameObjectForThisPanel.tag = "UI";
            _gameObjectForThisPanel.transform.SetParent(UI_CANVAS.transform);

            //Setting transform
            RectTransform panelTrans = _gameObjectForThisPanel.AddComponent<RectTransform>();
            panelTrans.localScale = new Vector3(1, 1, 1);
            panelTrans.anchoredPosition = positionOnUICanvas;
        }

        private void AddBackgroundImageToPanel()
        {
            GameObject backgroundGameObject = new GameObject("Background image");
            backgroundGameObject.transform.SetParent(_gameObjectForThisPanel.transform);
            
            //Setting transform
            RectTransform trans = backgroundGameObject.AddComponent<RectTransform>();
            trans.anchoredPosition = _backgroundImageInfo.positionOnCanvas;
            trans.localScale = _backgroundImageInfo.scaleOnCanvas;

            //setting sprite
            backgroundGameObject.AddComponent<Image>().sprite = _backgroundImageInfo.imageSprite;
        }

        private void AddButtonsToPanel()
        {
            int buttonCounter = 0;
            foreach (var button in _buttons)
            {
                buttonCounter++;

                //Adding gameObject
                GameObject newChildButton = new GameObject("Button " + buttonCounter);
                newChildButton.tag = "UI";
                newChildButton.transform.SetParent(_gameObjectForThisPanel.transform);

                //Setting transform
                RectTransform trans = newChildButton.AddComponent<RectTransform>();
                trans.anchoredPosition = button.positionOnCanvas;
                trans.localScale = button.scaleOnCanvas;
                   

                //Setting onclick listener(s)
                newChildButton.AddComponent<Button>().onClick = button.onClick;
                
                //Setting image/graphic of button
                newChildButton.AddComponent<Image>().sprite = button.buttonSprite;
                newChildButton.GetComponent<Image>().color = Color.red;
            }
        }

        private void AddTextLabelsToPanel()
        {
            int textCounter = 0;
            foreach (var label in _textLabels)
            {
                textCounter++;

                //Adding gameObject
                GameObject labelGameObject = new GameObject("Text label " + textCounter);
                labelGameObject.tag = "UI";
                labelGameObject.transform.SetParent(_gameObjectForThisPanel.transform);

                //Setting transform
                RectTransform trans = labelGameObject.AddComponent<RectTransform>();
                trans.localScale = new Vector3(1, 1, 1);
                trans.anchoredPosition = label.positionOnCanvas;
                trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, label.widthHeight.x);
                trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, label.widthHeight.y);
                //text rect and button shouldnt overlap
                
                //Setting text properties                
                Text text = labelGameObject.AddComponent<Text>();
                text.text = label.Text;
                text.fontSize = label.fontSize;
                text.font = label.Font;
                text.material = label.Material;
                text.color = label.Color;
            }
        }
    }
}