using UnityEngine;

namespace WorldWizards.core.manager
{
    /// <summary>
    /// This MonoBehavior needs to execute before everything else in World Wizards.
    /// It is responsible for setting up the Manager Registry.
    /// </summary>
    public class WWMain : MonoBehaviour
    {
        void Awake()
        {
            // setup the manager registry
            var setupManagerRegistry = ManagerRegistry.Instance;
            
        }
    }
}