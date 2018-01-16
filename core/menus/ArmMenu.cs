using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.manager;

namespace worldWizardsCore.core.menus
{
    /**
     * ArmMenu is a VR-only WWMenu that sits on the player's arm and allows them to quickly change controller tools.
     * Sets up positioning based on left controller position & rotation
     */
    public class ArmMenu : WWMenu
    { 
        private GameObject popupArmMenu;
        private GameObject controller;

        // Use this for initialization
        private void Start ()
        {
            Debug.Log("ArmMenu Start");

            /*if (ManagerRegistry.Instance.menuManager.GetMenuExists("ArmMenu"))
            {
                armMenu = ManagerRegistry.Instance.menuManager.GetMenuReference("ArmMenu");
            }

            if (ManagerRegistry.Instance.menuManager.GetMenuExists("PopupArmMenu"))
            {
                popupArmMenu = ManagerRegistry.Instance.menuManager.GetMenuReference("PopupArmMenu");
            }*/

            if (UnityEngine.VR.VRDevice.isPresent)
            {
                controller = FindObjectOfType<SteamVR_ControllerManager>().left;
                Setup();
            }
        }

        public override void Setup()
        {
            // Get list of all buttons on this menu.
            allButtons = new List<Button>(gameObject.GetComponents<Button>());

            
            // Put arm menu in the right place and parent it to the controller
            transform.rotation = controller.transform.rotation;
            transform.position = controller.transform.position;
            transform.Rotate(90, 0, 0);
            transform.position = new Vector3(transform.position.x, 
                                             transform.position.y + 0.05f,
                                             transform.position.z - 0.35f);
            transform.parent = controller.transform;

            // TODO: Move this logic to PopupMenus own class
            /* Put the popup menu in the right place and parent it to the arm menu
            popupArmMenu.transform.rotation = transform.rotation;
            popupArmMenu.transform.position = transform.position;
            popupArmMenu.transform.Rotate(0, 0, 90);
            popupArmMenu.transform.position = new Vector3(popupArmMenu.transform.position.x - 0.2f, popupArmMenu.transform.position.y, popupArmMenu.transform.position.z);
            popupArmMenu.transform.parent = transform;
            popupArmMenu.SetActive(false);
            */
        }
    }
}
