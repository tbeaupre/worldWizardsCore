using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldWizards.core.menus
{
    /// <summary>
    ///     WWMenuBuilder allows buttons to be added at runtime to any menu
    /// </summary>

    public static class WWMenuBuilder
    {
        
        /// <summary>
        ///     Adds a button to the menu for every string in the list
        /// </summary>
        /// <param name="buttonPrefab">The button prefab to be instantiated</param>
        /// <param name="buttonStrings">A string for every button we need</param>
        /// <param name="menu">The menu we are adding the buttons to</param>
        public static void BuildMenu(Button buttonPrefab, List<string> buttonStrings, GameObject panel, WWMenu menu)
        {   
            foreach (string buttonString in buttonStrings)
            {
                AddButton(buttonString, buttonPrefab, panel, menu);
            }
        }
        
        // TODO: Make sure buttons stay in panel/move to next row when necessary
        /// <summary>
        ///     Instantiates a button and adds it to the list of all buttons for this menu
        /// </summary>
        /// <param name="bundleTag">The string representing the asset bundle to be put on the button</param>
        /// <param name="buttonPrefab">The button prefab to be instantiated</param>
        /// <param name="menu">The menu the button is being added to</param>
        private static void AddButton(string bundleTag, Button buttonPrefab, GameObject panel, WWMenu menu)
        {
            Button button = (Button)MonoBehaviour.Instantiate(buttonPrefab);
            Text text = button.GetComponentInChildren<Text>();
            text.text = bundleTag;

            // TODO: Get panel reference instead of main canvas reference
            button.transform.SetParent(panel.transform, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            
            button.GetComponent<WWButton>().SetMetadata(bundleTag);
            
            menu.AddButton(button);
        }
        
        
    }
}
