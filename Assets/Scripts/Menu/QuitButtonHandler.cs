using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class QuitButtonHandler : MonoBehaviour
    {
        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(TaskOnClick);
        }

        void TaskOnClick()
        {
            Application.Quit();
        }
    }
}