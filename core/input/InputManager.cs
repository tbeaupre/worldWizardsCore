using UnityEngine;
using worldWizards.core.input.Desktop;
using worldWizards.core.input.Tools;
using worldWizards.core.input.VRControls;

namespace worldWizards.core.input
{
    public class InputManager : MonoBehaviour
    {
        private bool vrEnabled;
        public Camera headCamera;
        public Camera desktopCamera;
        
        private void Awake()
        {
            vrEnabled = UnityEngine.VR.VRDevice.isPresent;
            if (vrEnabled)
            {
                Debug.Log("InputManager::Awake(): VR Controls Enabled");
                desktopCamera.gameObject.SetActive(false);
                headCamera.gameObject.SetActive(true);
                SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
                
                VRListener leftListener = controllerManager.left.AddComponent<VRListener>();
                leftListener.Init(false, typeof(StandardTool));
                
                VRListener rightListener = controllerManager.right.AddComponent<VRListener>();
                rightListener.Init(true, typeof(CreateObjectTool));
            }
            else
            {
                Debug.Log("InputManager::Awake(): Desktop Controls Enabled");
                headCamera.gameObject.SetActive(false);
                desktopCamera.gameObject.SetActive(true);
                
                ControlScheme leftControlScheme = new ControlScheme(KeyCode.E, KeyCode.Q, KeyCode.Alpha2,
                    KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.LeftShift);
                DesktopListener leftListener = gameObject.AddComponent<DesktopListener>();
                leftListener.Init(leftControlScheme, false, typeof(StandardTool));
                
                ControlScheme rightControlScheme = new ControlScheme(KeyCode.U, KeyCode.O, KeyCode.Alpha8,
                    KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L, KeyCode.Slash);
                DesktopListener rightListener = gameObject.AddComponent<DesktopListener>();
                rightListener.Init(rightControlScheme, true, typeof(CreateObjectTool));
            }
        }
    }
}