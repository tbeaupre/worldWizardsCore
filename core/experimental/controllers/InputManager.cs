using UnityEngine;
using worldWizards.core.experimental.controllers.Desktop;
using worldWizards.core.experimental.controllers.VRControls;

namespace WorldWizards.core.experimental.controllers
{
    public class InputManager : MonoBehaviour
    {
        private void Awake()
        {
            SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
            if (UnityEngine.VR.VRDevice.isPresent)
            {
                Debug.Log("InputManager::Awake(): VR Controls Enabled");
                controllerManager.left.AddComponent<LeftVRController>();
                controllerManager.right.AddComponent<RightVRController>();
            }
            else
            {
                Debug.Log("InputManager::Awake(): Desktop Controls Enabled");
                gameObject.AddComponent<DesktopController>();
            }
        }
    }
}