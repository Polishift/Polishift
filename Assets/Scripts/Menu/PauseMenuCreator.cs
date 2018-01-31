using System;
using System.Linq;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class PauseMenuCreator : MonoBehaviour
    {
        private const string PANEL_OBJNAME = "PAUSEMENU background";
        private const string QUITTEXT_OBJNAME = "PAUSEMENU QuitText";
        private const string QUITBUTTON_OBJNAME = "PAUSEMENU QuitButton";
        private const string CANCELBUTTON_OBJNAME = "PAUSEMENU CancelButton";
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESCAPE");

                DestroyMenu();
                CreateMenu();
            }
        }
        
        private void CreateMenu()
        {
            //Adding background
            Sprite defaultSprite = Resources.Load<Sprite>("Square");
            UICreator.AddBackgroundPanelToCanvas(PANEL_OBJNAME, defaultSprite, new Vector3(0, 0), new Vector3(2f, 1.3f));

            //Adding text
            UICreator.AddTextToCanvas(QUITTEXT_OBJNAME, "Quit?", 34, new Vector3(10, 0, 0), new Vector2(110, 100));

            //Adding buttons
            Sprite confirmSprite = Resources.Load<Sprite>("Sprites/Checkmark");
            Sprite cancelSprite = Resources.Load<Sprite>("Sprites/ExitButton");

            UICreator.AddChildButtonToCanvas(QUITBUTTON_OBJNAME, confirmSprite, confirmSprite, () => SceneManager.LoadScene("MenuScene"), new Vector3(-40f, -22f), 0.6f);
            UICreator.AddChildButtonToCanvas(CANCELBUTTON_OBJNAME, cancelSprite, cancelSprite, DestroyMenu, new Vector3(30, -22f), 0.6f);
        }

        private void DestroyMenu()
        {
            Destroy(GameObject.Find(PANEL_OBJNAME));
            Destroy(GameObject.Find(QUITTEXT_OBJNAME));
            Destroy(GameObject.Find(QUITBUTTON_OBJNAME));
            Destroy(GameObject.Find(CANCELBUTTON_OBJNAME));
        }
    }
}