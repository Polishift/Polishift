using UI;
using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI
{
    public class CountryDetailsDisplayer : AbstractDisplayer
    {
        public static readonly string PREFIX = "DETAILS DISPLAYER";
        private static readonly string PANEL_OBJNAME = PREFIX + " Background image";
        private static readonly string RULERINFO_OBJNAME = PREFIX + " RulerInfo";
        private static readonly string TEXT_OBJNAME = PREFIX + " text";
        private static readonly string CLOSEBUTTON_OBJNAME = PREFIX + " CloseButton";

        
        public override void Init()
        {
            base.BaseInit();
            
            //Creating the panel, destroying previous just in case
            CreateBasicPanel();
        }

        protected override void CreateBasicPanel()
        {
            Sprite defaultSprite = Resources.Load<Sprite>("Square");

            UICreator.AddBackgroundPanelToCanvas(PANEL_OBJNAME, defaultSprite, new Vector3(0, 0), new Vector3(5.5f, 4.2f));
            
            UICreator.AddTextToCanvas(RULERINFO_OBJNAME, base.GetCurrentCountryText() + ":\nDetails", 35, new Vector3(-200, 60), new Vector2(100, 200));
            UICreator.AddTextToCanvas(TEXT_OBJNAME, base.CountryInformation.GetPrettifiedDetails(), 20, new Vector3(-200, -40), new Vector2(100, 200));
            
            Sprite exitButtonSprite = Resources.Load<Sprite>("Sprites/ExitButton");
            UICreator.AddChildButtonToCanvas(CLOSEBUTTON_OBJNAME, exitButtonSprite, exitButtonSprite, DestroyThisPanel, new Vector3(220, 150), 0.3f);
        }

        protected override void DestroyThisPanel()
        {
            Destroy(GameObject.Find(PANEL_OBJNAME));
            Destroy(GameObject.Find(RULERINFO_OBJNAME));
            Destroy(GameObject.Find(TEXT_OBJNAME));
            Destroy(GameObject.Find(CLOSEBUTTON_OBJNAME));
        }
    }
}