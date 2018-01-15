using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.manager;

namespace worldWizardsCore.core.manager
{
    /// <summary>
    ///     This MonoBehavior needs to execute before everything else in World Wizards.
    ///     It is responsible for setting up the Manager Registry.
    /// </summary>
    public class WWMain : MonoBehaviour
    {
        private void Awake()
        {
            // setup the manager registry
            ManagerRegistry setupManagerRegistry = ManagerRegistry.Instance;
            ResourceLoader.LoadResources();
        }
    }
}