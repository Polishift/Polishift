using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI.Country_Info_Popup
{
    public class ButtonInfo
    {
        public Vector3 positionOnCanvas;
        public Vector3 scaleOnCanvas = new Vector3(1, 1, 1);
        public Sprite buttonSprite;
        public readonly Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();

        
        public ButtonInfo(Vector3 positionOnCanvas, Vector3 scaleOnCanvas, 
                          Sprite buttonSprite, Action onClickAction)
        {
            this.positionOnCanvas = positionOnCanvas;

            if (scaleOnCanvas != null)
                this.scaleOnCanvas = scaleOnCanvas;

            this.buttonSprite = buttonSprite;
            this.onClick.AddListener(onClickAction.Invoke);
        }
    }
}