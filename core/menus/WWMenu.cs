using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace worldWizardsCore.core.menus
{
    /// <summary>
    ///     WWMenu holds info for menus used in World Wizards.
    ///     It has references to all of its Panels and Buttons.
    ///     All menus inherit from WWMenu.
    /// </summary>
   
    public abstract class WWMenu : MonoBehaviour
    {
        // TODO: Implement follow camera script
        
        public bool followCamera;            // Whether or not the menu should follow the camera
        public List<GameObject> allPanels;   // Reference to all panels the menu has
        public List<Button> allButtons;      // List of all buttons the menu has
        
        // TODO: Add styling information?
        
        /// <summary>
        ///      Set up variables (get any buttons & panels the menu already has)
        ///      For menus to add buttons/panels at runtime
        /// </summary>
        protected virtual void Setup()
        {
            allButtons = new List<Button>();
            allPanels = new List<GameObject>();

            /* Get all panels on this menu
            foreach (GameObject o in GetComponents<GameObject>())
            {
                if (o.CompareTag("UIPanel"))
                {
                    allPanels.Add(o);
                }
            }*/
        }
        
        /// <summary>
        ///     Get all of the panels this menu has
        /// </summary>
        /// <returns>List of all panel references</returns>
        public List<GameObject> GetAllPanels()
        {
            return allPanels;
        }

        /// <summary>
        ///     Get all of the buttons this menu has
        /// </summary>
        /// <returns>List of all button references</returns>
        public List<Button> GetAllButtons()
        {
            return allButtons;
        }
        
        /// <summary>
        ///     Add a button to the list of all buttons this menu has
        /// </summary>
        /// <param name="button">Button reference to add to list</param>
        public virtual void AddButton(Button button)
        {
            allButtons.Add(button);
        }
    }
}
