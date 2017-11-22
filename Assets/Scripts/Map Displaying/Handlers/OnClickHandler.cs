using DefaultNamespace.Map_Displaying.UI;
using UnityEngine;

namespace Map_Displaying.Handlers
{
    public class OnClickHandler : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("gameObjectThatWasHit  = " + hit.transform.name);
                    
                    hit.transform.gameObject.AddComponent<CountryInfoDisplayer>();
                    hit.transform.gameObject.GetComponent<CountryInfoDisplayer>().Init();
                }
            }
        }
    }
}