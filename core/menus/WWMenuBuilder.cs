using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.controller.level;

namespace worldWizardsCore.core.menus
{
    /**
     * WWMenuBuilder allows buttons to be added at runtime to any menu
     */

    public static class WWMenuBuilder
    {
        
        public static void BuildMenu(Button buttonPrefab, List<string> buttonStrings, WWMenu menu)
        {   
            foreach (string buttonString in buttonStrings)
            {
                AddButton(buttonString, buttonPrefab, menu);
            }
        }

        /**
         * Instantiates a button and adds it to the list of all buttons for this menu
         * 
         * @param bundleTag the string representing the asset bundle tag to be put on the button
         */
        // TODO: Make sure buttons stay in panel/move to next row when necessary
        private static void AddButton(string bundleTag, Button buttonPrefab, WWMenu menu)
        {
            Button button = (Button)MonoBehaviour.Instantiate(buttonPrefab);
            Text text = button.GetComponentInChildren<Text>();
            text.text = bundleTag;

            // TODO: Get panel reference instead of main canvas reference
            button.transform.SetParent(menu.transform, false);
            button.transform.localScale = new Vector3(1, 1, 1);
            
            button.GetComponent<WWButton>().SetMetadata(bundleTag);
            
            menu.AddButton(button);
        }
        
        
    }
}
