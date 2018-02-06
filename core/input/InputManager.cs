using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.input.Desktop;
using WorldWizards.core.input.Tools;
using WorldWizards.core.input.VRControls;
using WorldWizards.SteamVR.Scripts;

namespace WorldWizards.core.input
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
        
        private List<Type> availableToolTypes;
        private int toolIndex;
        
        private void Awake()
        {
            BuildAvailableToolTypes();
            // Check for VRDevice and create the necessary InputListeners for either VR or Desktop mode.
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                Debug.Log("InputManager::Awake(): VR Controls Enabled");
                // Change the current camera to the VR rig's headCamera.
                desktopCamera.gameObject.SetActive(false);
                headCamera.gameObject.SetActive(true);
                ConfigureHighlightsFX(headCamera.gameObject.AddComponent<HighlightsFX>());
                
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
                ConfigureHighlightsFX(desktopCamera.gameObject.AddComponent<HighlightsFX>());
                
                
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


        private void ConfigureHighlightsFX(HighlightsFX fx)
        {
            fx.m_highlightColor = Color.yellow;
            fx.m_fillType = HighlightsFX.FillType.Outline;
            fx.m_selectionType = HighlightsFX.HighlightType.Glow;
            fx.m_sortingType = HighlightsFX.SortingType.Overlay;
        }

        // This is just for debugging the new EditObject Tool.
        public  void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ToolIndexDown();
                right.ChangeTool(GetCurrentTool());
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ToolIndexUp();
                right.ChangeTool(GetCurrentTool());
            }
        }

        private void BuildAvailableToolTypes()
        {
            availableToolTypes = new List<Type>();
            availableToolTypes.Add(typeof(EditObjectTool));
            availableToolTypes.Add(typeof(CreateObjectTool));
            availableToolTypes.Add(typeof(SelectionTool));
        }

        private Type GetCurrentTool()
        {
            return availableToolTypes[toolIndex];
        }

        private void ToolIndexUp()
        {
            toolIndex++;
            if (toolIndex >= availableToolTypes.Count)
            {
                toolIndex = 0;
            }
        } 
        
        private void ToolIndexDown()
        {
            toolIndex--;
            if (toolIndex < 0)
            {
                toolIndex = availableToolTypes.Count - 1;
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