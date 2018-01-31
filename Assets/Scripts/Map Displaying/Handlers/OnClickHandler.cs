using DefaultNamespace.Map_Displaying.UI;
using Map_Displaying.Reference_Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map_Displaying.Handlers
{
    public class OnClickHandler : MonoBehaviour
    {
        private readonly string UI_CANVAS = "UI";
        private EventSystem _theEventSystem;

        private void Start()
        {
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
                    && hit.transform.gameObject.GetComponent<CountryInformationReference>() != null)
                {
                    RemoveAllOtherInformationPanels();
                    AddInformationPanelForHitCountry(hit);
                }
            }
        }

        private void RemoveAllOtherInformationPanels()
        {
            foreach (Transform childUIPanel in GameObject.Find("UI_Canvas").transform)
            {
                Destroy(childUIPanel.gameObject);
            }
        }

        private void AddInformationPanelForHitCountry(RaycastHit hit)
        {
            hit.transform.gameObject.AddComponent<CountryInfoDisplayer>();
            hit.transform.gameObject.GetComponent<CountryInfoDisplayer>().Init();
        }
    }
}