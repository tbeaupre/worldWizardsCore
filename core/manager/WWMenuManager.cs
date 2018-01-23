using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace worldWizardsCore.core.manager
{
    /// <summary>
    ///     WWMenuManager holds references to all of the menus used in World Wizards.
    ///     Allows menus to be activated/deactivated from anywhere.
    ///     Allows you to get references to specific menus from anywhere.
    /// </summary>
 
    public class WWMenuManager : Manager
    {

        private UnityEngine.Object[] allMenusArray;
        private readonly Dictionary<string, GameObject> allMenus;

        private const string clone = "(Clone)";

        /// <summary>
        ///     Get references to every menu in the game and instantiate them.
        ///     Decide which menus need to be active/inactive at the start of the game.
        /// </summary>
        public WWMenuManager()
        {
            allMenus = new Dictionary<string, GameObject>();

            // Get references to all menus in Prefabs/Menus
            allMenusArray = Resources.LoadAll("Prefabs/Menus");

            foreach (Object menu in allMenusArray)
            {
                // Instantiating will call each WWMenu's Setup function
                var m = Object.Instantiate(menu) as GameObject;

                // TODO: Make separate folders for menus that are used in VR vs Desktop modes, only instantiate ones we need
                if (m == null)
                {
                    Debug.Log("Menu not created");
                }
                else
                {
                    string menuName = m.transform.name.Replace(clone, "");

                    allMenus.Add(menuName, m);
                    m.SetActive(false);
                }
            }

            // VR startup menus
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                SetMenuActive("ArmMenu", true);
            }
            // Desktop startup menus
            else
            {

            }
        }

        /// <summary>
        ///     Get reference to specific menu
        /// </summary>
        /// <param name="menuName">The menu you want a reference to</param>
        /// <returns>Menu reference</returns>
        public GameObject GetMenuReference(string menuName)
        {
            GameObject menu;
            if (allMenus.TryGetValue(menuName, out menu))
            {
                return menu;
            }

            Debug.Log("Couldn't find menu " + menuName);
            return null;
        }

        /// <summary>
        ///     Set a specific menu active/inactive
        /// </summary>
        /// <param name="menuName">The menu you want to set active/inactive</param>
        /// <param name="active">True if active, false if inactive</param>
        public void SetMenuActive(string menuName, bool active)
        {
            GameObject menu;
            if (allMenus.TryGetValue(menuName, out menu))
            {
                menu.SetActive(active);
            }
            else
            {
                Debug.Log("Couldn't find menu " + menuName);
            }
        }

        /// <summary>
        ///     Get whether a menu exists or not
        /// </summary>
        /// <param name="menuName">The menu you want to check</param>
        /// <returns>True is menu exists, false otherwise</returns>
        public bool GetMenuExists(string menuName)
        {
            GameObject m;
            return allMenus.TryGetValue(menuName, out m);
        }

    }
}
