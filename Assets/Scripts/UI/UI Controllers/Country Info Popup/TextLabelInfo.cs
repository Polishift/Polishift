using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI.Country_Info_Popup
{
    public class TextLabelInfo
    {
        public Vector3 positionOnCanvas;
        public Vector2 widthHeight;
        public string Text;
        public Font Font;
        public int fontSize;
        
        public Material Material;
        public Color Color;

        public TextLabelInfo(string text, Font font, int fontSize, Material material, Color color, Vector3 positionOnCanvas, Vector2 widthHeight)
        {
            this.Text = text;
            this.Font = font;
            this.fontSize = fontSize;
            this.Material = material;
            this.Color = color;
            this.positionOnCanvas = positionOnCanvas;
            this.widthHeight = widthHeight;
        }
    }
}