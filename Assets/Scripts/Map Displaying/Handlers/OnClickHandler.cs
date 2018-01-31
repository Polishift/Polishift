using DefaultNamespace.Map_Displaying.UI;
using Map_Displaying.Reference_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map_Displaying.Handlers
{
    public class OnClickHandler : MonoBehaviour
    {
        private GameObject UI_Canvas;
        private EventSystem _theEventSystem;

        private void Start()
        {
            UI_Canvas = GameObject.Find("UI_Canvas");
            _theEventSystem = EventSystem.current;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                //ignore if the raycast is over an UI element, or if it's not a country 
                if (Physics.Raycast(ray, out hit) 
                    && !_theEventSystem.IsPointerOverGameObject()
                    && hit.transform.gameObject.GetComponent<CountryInformationReference>() != null
                    && NoOtherInformationPanelsAlreadyOpen())
                {
                    AddInformationPanelForHitCountry(hit);
                }
            }
        }

        private void AddInformationPanelForHitCountry(RaycastHit hit)
        {
            hit.transform.gameObject.AddComponent<CountryInfoDisplayer>();
            hit.transform.gameObject.GetComponent<CountryInfoDisplayer>().Init();
        }

        private bool NoOtherInformationPanelsAlreadyOpen()
        {
            int informationPanelsCount = 0;
            
            foreach(Transform child in UI_Canvas.transform)
                if (child.name.Contains(CountryInfoDisplayer.PREFIX))
                    informationPanelsCount++;

            return informationPanelsCount == 0;
        }
    }
}