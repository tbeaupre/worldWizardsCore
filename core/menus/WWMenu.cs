using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace worldWizardsCore.core.menus
{
    public abstract class WWMenu : MonoBehaviour
    {
        public bool followCamera;
        public List<GameObject> allPanels;
        public List<Button> allButtons;
        // TODO: Add styling information?

        // For menus to add any buttons/panels it may need added at runtime 
        public abstract void Setup();

        public List<Button> GetAllButtons()
        {
            return allButtons;
        }
    }
}
