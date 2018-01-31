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
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESCAPE");

                UICreator.DestroyUI();
                CreateMenu();
            }
        }

        
        private void CreateMenu()
        {
            //Adding background
            Sprite defaultSprite = Resources.Load<Sprite>("Square");
            UICreator.AddBackgroundPanelToCanvas(defaultSprite, new Vector3(0, 0), new Vector3(2f, 1.3f));

            //Adding text
            UICreator.AddTextToCanvas("QuitText", "Quit?", 34, new Vector3(10, 0, 0), new Vector2(110, 100));

            //Adding buttons
            Sprite confirmSprite = Resources.Load<Sprite>("Sprites/Checkmark");
            Sprite cancelSprite = Resources.Load<Sprite>("Sprites/ExitButton");

            UICreator.AddChildButtonToCanvas("QuitButton", confirmSprite, confirmSprite, () => SceneManager.LoadScene("MenuScene"), new Vector3(-40f, -22f), 0.6f);
            UICreator.AddChildButtonToCanvas("CancelButton", cancelSprite, cancelSprite, UICreator.DestroyUI, new Vector3(30, -22f), 0.6f);
        }
    }
}