using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Map_Displaying.Reference_Scripts;
using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI
{
    [RequireComponent(typeof(CountryInformationReference))]
    public class CountryInfoDisplayer : MonoBehaviour
    {
        Rect _mWindowRect;
        string _mTitle;
        string _mMsg;
        //Action _mAction;


        public void Init()
        {
            Debug.Log("Created new COuntryInfoDisplayer, gameObject name = " + gameObject.name);
            
            var countryInformation = gameObject.GetComponent<CountryInformationReference>();
            
            //todo set alternative face to be = new
            _mTitle = countryInformation.Iso3166Country.Name;
            _mMsg = countryInformation.Iso3166Country.Name;
            //_mAction = action;
        }

        void OnGUI()
        {
            const int maxWidth = 640;
            const int maxHeight = 480;

            int width = Mathf.Min(maxWidth, Screen.width - 20);
            int height = Mathf.Min(maxHeight, Screen.height - 20);
            _mWindowRect = new Rect(
                x: (Screen.width - width) / 2,
                y: (Screen.height - height) / 2,
                width: width,
                height: height);

            _mWindowRect = GUI.Window(0, _mWindowRect, WindowPropertyFunc, _mTitle);
        }

        void WindowPropertyFunc(int windowId)
        {
            const int border = 10;
            const int width = 50;
            const int height = 25;
            const int spacing = 10;

            Rect l = new Rect(
                border,
                border + spacing,
                _mWindowRect.width - border * 2,
                _mWindowRect.height - border * 2 - height - spacing);
            GUI.Label(l, _mMsg);

            Rect b = new Rect(
                _mWindowRect.width - width - border,
                _mWindowRect.height - height - border,
                width,
                height);

            if (GUI.Button(b, "ok"))
            {
                Destroy(this);
                //_mAction();
            }
        }
    }
}