using UnityEngine;
using worldWizards.core.entity.common;

namespace worldWizards.core.controller.common
{
    /// <summary>
    /// A Singleton class that persists for the duration of the
    /// entire World Wizards application, and manages overall application settings.
    /// </summary>
    public class WWMasterSingleton : MonoBehaviour
    {
        private WWMode mode;
        public static WWMasterSingleton instance;

        /// <summary>
        /// Handle the Singleton pattern in Unity where the Singleton
        /// is a Unity GameObject that should only exist as a single instance.
        /// </summary>
        public void Awake() {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else {
                Destroy(this);
            }
        }
    }
}
