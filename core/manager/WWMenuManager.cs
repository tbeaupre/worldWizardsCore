using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class WWMenuManager
{

    private UnityEngine.Object[] allMenusArray;
    private Dictionary<string, GameObject> allMenus;

    private const string clone = "(Clone)";

    
    /**
     * WWMenuManager holds references to all of the menus used in World Wizards.
     * Allows menus to be activated/deactivated from anywhere.
     * Allows you to get references to specific menus from anywhere.
     */
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
        if (UnityEngine.VR.VRDevice.isPresent)
        {
            SetMenuActive("ArmMenu", true);
        }
        // Desktop startup menus
        else
        {
            
        }
    }

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

    public bool GetMenuExists(string menuName)
    {
        GameObject m;
        return allMenus.TryGetValue(menuName, out m);
    }
    
}
