using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.manager;

namespace worldWizardsCore.core.menus
{
    /// <summary>
    ///     ArmMenu is a VR-only WWMenu that sits on the player's arm and allows them to quickly change controller tools.
    ///     Sets up positioning based on left controller position & rotation
    /// </summary>
    
    public class ArmMenu : WWMenu
    { 
        private GameObject popupArmMenu;
        private GameObject controller;

        private void Start ()
        {
            Debug.Log("ArmMenu Start");

            if (UnityEngine.XR.XRDevice.isPresent)
            {
                controller = FindObjectOfType<SteamVR_ControllerManager>().left;
                Setup();
            }
        }

        protected override void Setup()
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
        }
    }
}
