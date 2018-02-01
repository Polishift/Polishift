using UI;
using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI
{
    public class PopUpDisplayer : AbstractDisplayer
    {
        //ugly
        public string PopupMessage = "";
        
        public static readonly string PREFIX = "POPUP DISPLAYER ";
        private static readonly string PANEL_OBJNAME = PREFIX + " Background image";
        private static readonly string TEXT_OBJNAME = PREFIX + " text";
        private static readonly string CLOSEBUTTON_OBJNAME = PREFIX + " CloseButton";

        
        public override void Init()
        {
            base.BaseInit();
            
            CreateBasicPanel();
        }

        protected override void CreateBasicPanel()
        {
            Sprite defaultSprite = Resources.Load<Sprite>("Square");

            //Set this stuff right, change the icon, and this is done :))
            UICreator.AddBackgroundPanelToCanvas(PANEL_OBJNAME, defaultSprite, new Vector3(0, 0), new Vector3(5, 1));            
            UICreator.AddTextToCanvas(TEXT_OBJNAME, PopupMessage, 20, new Vector3(-200, -59), new Vector2(100, 200));
            
            Sprite exitButtonSprite = Resources.Load<Sprite>("Sprites/ExitButton");
            UICreator.AddChildButtonToCanvas(CLOSEBUTTON_OBJNAME, exitButtonSprite, exitButtonSprite, DestroyThisPanel, new Vector3(232.2f, 31.7f), 0.3f);
        }

        protected override void DestroyThisPanel()
        {
            Destroy(GameObject.Find(PANEL_OBJNAME));
            Destroy(GameObject.Find(TEXT_OBJNAME));
            Destroy(GameObject.Find(CLOSEBUTTON_OBJNAME));
        }
    }
}