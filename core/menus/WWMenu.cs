using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace worldWizardsCore.core.menus
{
    /**
     * WWMenu holds info for menus used in World Wizards.
     * It has references to all of its Panels and Buttons.
     * All menus inherit from WWMenu.
     */
    public abstract class WWMenu : MonoBehaviour
    {
        public bool followCamera;            // Whether or not the menu should follow the camera
        public List<GameObject> allPanels;   // Reference to all panels the menu has
        public List<Button> allButtons;      // List of all buttons the menu has
        // TODO: Add styling information?

        // For menus to add any buttons/panels it may need added at runtime 
        public abstract void Setup();

        /**
         * Get the list of all buttons this menu has
         * @return list of all buttons
         */
        public List<Button> GetAllButtons()
        {
            return allButtons;
        }
    }
}
