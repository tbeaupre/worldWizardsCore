using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.controller.common
{
    /// <summary>
    ///     A Singleton class that persists for the duration of the
    ///     entire World Wizards application, and manages overall application settings.
    /// </summary>
    public class WWMasterSingleton : MonoBehaviour
    {
        public static WWMasterSingleton instance;
        private WWMode mode;

        /// <summary>
        ///     Handle the Singleton pattern in Unity where the Singleton
        ///     is a Unity GameObject that should only exist as a single instance.
        /// </summary>
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}