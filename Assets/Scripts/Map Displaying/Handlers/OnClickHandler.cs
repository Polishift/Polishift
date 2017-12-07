using DefaultNamespace.Map_Displaying.UI;
using DefaultNamespace.Map_Displaying.UI.Country_Info_Popup;
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
                
                //ignore if the raycast is over an UI element
                if (Physics.Raycast(ray, out hit) && !_theEventSystem.IsPointerOverGameObject())
                {
                    RemoveAllOtherInformationPanels();
                    AddInformationPanelForHitCountry(hit);
                }
            }
        }

        private void RemoveAllOtherInformationPanels()
        {
            foreach (Transform childUIPanel in GenericPanel.UI_CANVAS.transform)
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