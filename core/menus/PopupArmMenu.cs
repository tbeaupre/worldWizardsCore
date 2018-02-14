using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.manager;

namespace WorldWizards.core.menus
{
    public class PopupArmMenu : WWMenu
    {
        private GameObject armMenu;
        
        private void Start()
        {
            Debug.Log("PopupArmMenu Start");

            armMenu = ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference("ArmMenu");

            // TODO: Change this because it sucks
            if (armMenu)
            {
                Setup();
            }

            allButtons = new List<Button>(gameObject.GetComponents<Button>());
        }
    }
}