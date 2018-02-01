using System.Linq;
using Dataformatter.Dataprocessing.Entities;
using Game_Logic;
using Repository;
using UnityEngine;

namespace DefaultNamespace.Map_Displaying.UI
{
    public static class PopupCreator
    {
        private static PopUpEntity[] AllPopups;

        public static void Init()
        {
            AllPopups = RepositoryHub.PopUpRepository.GetAll();

            int count = 0;
            foreach (var popUp in AllPopups)
            {
                count++;

                GameObject popupObj = new GameObject();
                popupObj.transform.parent = GameObject.Find("PopupCanvas").transform;
                popupObj.name = "Popup " + count;
                popupObj.AddComponent<PopupListener>().Init(popUp);
            }
        }
    }
}