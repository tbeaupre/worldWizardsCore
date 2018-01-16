using UnityEngine;
using worldWizards.core.input.Desktop;
using worldWizards.core.input.Tools;
using worldWizards.core.input.VRControls;

namespace worldWizards.core.input
{
    /**
     * This script is attached to a single object in the scene and handles the creation of InputListeners for either
     * VR or Desktop mode.
     */
    public class InputManager : MonoBehaviour
    {
        public Camera headCamera;    // The VR Rig's head camera.
        public Camera desktopCamera; // The camera which is used if VR is not enabled.
        private InputListener left;  // The left controller's input listener.
        private InputListener right; // The right controller's input listener.
        
        private void Awake()
        {
            // Check for VRDevice and create the necessary InputListeners for either VR or Desktop mode.
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                Debug.Log("InputManager::Awake(): VR Controls Enabled");
                // Change the current camera to the VR rig's headCamera.
                desktopCamera.gameObject.SetActive(false);
                headCamera.gameObject.SetActive(true);
                
                SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
                
                // Create and Initialize the left controller.
                VRListener leftListener = controllerManager.left.AddComponent<VRListener>();
                leftListener.Init(false, typeof(StandardTool));
                left = leftListener;
                
                // Create and Initialize the right controller.
                VRListener rightListener = controllerManager.right.AddComponent<VRListener>();
                rightListener.Init(true, typeof(CreateObjectTool));
                right = rightListener;
            }
            else
            {
                Debug.Log("InputManager::Awake(): Desktop Controls Enabled");
                // Change the current camera to the desktop Camera.
                headCamera.gameObject.SetActive(false);
                desktopCamera.gameObject.SetActive(true);
                
                // Create the ControlScheme struct for the left desktop controller.
                ControlScheme leftControlScheme = new ControlScheme(KeyCode.E, KeyCode.Q, KeyCode.Alpha2,
                    KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.LeftShift);
                // Create and Initialize the left controller.
                DesktopListener leftListener = gameObject.AddComponent<DesktopListener>();
                leftListener.Init(leftControlScheme, true, typeof(StandardTool));
                left = leftListener;
                
                // Create the ControlScheme struct for the right desktop controller.
                ControlScheme rightControlScheme = new ControlScheme(KeyCode.U, KeyCode.O, KeyCode.Alpha8,
                    KeyCode.I, KeyCode.K, KeyCode.J, KeyCode.L, KeyCode.Slash);
                // Create and Initialize the right controller.
                DesktopListener rightListener = gameObject.AddComponent<DesktopListener>();
                rightListener.Init(rightControlScheme, true, typeof(CreateObjectTool));
                right = rightListener;
            }
        }
        
        // This is just for debugging the new EditObject Tool.
        public  void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                right.ChangeTool(typeof(EditObjectTool));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                right.ChangeTool(typeof(CreateObjectTool));
            }
        }
        
        public string GetLeftToolName()
        {
            return left.GetToolName();
        }
        
        public string GetRighttToolName()
        {
            return right.GetToolName();
        }
    }
}