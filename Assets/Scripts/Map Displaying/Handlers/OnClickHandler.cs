using DefaultNamespace.Map_Displaying.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Map_Displaying.Handlers
{
    public class OnClickHandler : MonoBehaviour
    {
        private readonly string UI_TAG = "UI";
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
                if (Physics.Raycast(ray, out hit))
                {
                    if (!_theEventSystem.IsPointerOverGameObject()) //ignore if its an UI element
                    {
                        hit.transform.gameObject.AddComponent<CountryInfoDisplayer>();
                        hit.transform.gameObject.GetComponent<CountryInfoDisplayer>().Init();
                    }
                }
            }
        }
    }
}