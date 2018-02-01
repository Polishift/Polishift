using Dataformatter.Dataprocessing.Entities;
using Game_Logic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.Map_Displaying.UI
{
    public class PopupListener : MonoBehaviour
    {
        private PopUpEntity AssociatedPopUp;

        public void Init(PopUpEntity AssociatedPopUp)
        {
            this.AssociatedPopUp = AssociatedPopUp;

            //Adding stuff
            gameObject.AddComponent<CanvasRenderer>();

            gameObject.AddComponent<Button>();
            gameObject.GetComponent<Button>().onClick.AddListener(DisplayPopUp);

            //Adding popup sprite 
            gameObject.AddComponent<Image>();
            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Alert");
        }

        private void Update()
        {
            //Very dirty, but necessary because of syncing issues
            gameObject.transform.position = GameObject.Find(AssociatedPopUp.CountryCode).gameObject.transform.position;


            if (YearCounter.GetCurrentYear() - AssociatedPopUp.Year <= 2 
                && YearCounter.GetCurrentYear() - AssociatedPopUp.Year > 0)
            {
                gameObject.GetComponent<Button>().enabled = true;
                gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<Button>().enabled = false;
                gameObject.GetComponent<Image>().enabled = false;
            }
        }

        private void DisplayPopUp()
        {
            //Inits the displayer when the popup icon is clicked
            //Creating the panel, destroying previous just in case
            gameObject.AddComponent<PopUpDisplayer>();
            gameObject.GetComponent<PopUpDisplayer>().PopupMessage = AssociatedPopUp.Message;
            gameObject.GetComponent<PopUpDisplayer>().Init();
        }
    }
}