using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI.Country_Info_Popup
{
    public class ImageInfo
    {
        public Vector3 positionOnCanvas;
        public Vector3 scaleOnCanvas;
        public Sprite imageSprite;


        public ImageInfo(Vector3 positionOnCanvas, Vector3 scaleOnCanvas, Sprite imageSprite)
        {
            this.positionOnCanvas = positionOnCanvas;
            this.scaleOnCanvas = scaleOnCanvas;
            this.imageSprite = imageSprite;
        }
    }
}