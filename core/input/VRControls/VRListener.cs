using System;
using UnityEngine;
using worldWizards.core.input.Tools;

namespace worldWizards.core.input.VRControls
{
    /**
     * Holds information specific to VR Controllers and registers listeners for the buttons.
     */
    public class VRListener : InputListener
    {
        private SteamVR_TrackedController controller; // The VR Controller that is being listened to.
        private Transform cameraRigTransform;         // Location of the player.
        private Transform headTransform;              // Location of the player's head.

        public void Init(bool canChange, Type initToolType)
        {
            canChangeTools = canChange;
            tool = gameObject.AddComponent(initToolType) as Tool;
            
            SteamVR_ControllerManager cameraRig = FindObjectOfType<SteamVR_ControllerManager>();
            cameraRigTransform = cameraRig.transform;
            headTransform = cameraRig.GetComponentInChildren<Camera>().transform;
        }

        protected void Awake()
        {
            controller = GetComponent<SteamVR_TrackedController>();
            
            // Register InputListener's listener functions to the OnEventHandlers.
            controller.TriggerClicked += OnTriggerClick;
            controller.TriggerUnclicked += OnTriggerUnclick;
            controller.Gripped += OnGrip;
            controller.Ungripped += OnUngrip;
            controller.MenuButtonClicked += OnMenuClick;
            controller.MenuButtonUnclicked += OnMenuUnclick;
            controller.PadClicked += OnPadClick;
            controller.PadUnclicked += OnPadUnclick;
            controller.PadTouched += OnPadTouch;
            controller.PadUntouched += OnPadUntouch;
        }

        protected override Vector2 GetCurrentPadPosition()
        {
            return new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
        }

        public override Vector3 GetHeadOffset()
        {
            Vector3 offset = cameraRigTransform.position - headTransform.position;
            offset.y = 0; // Without this, the player's head will be in the ground when teleporting.
            return offset;
        }

        public override Transform GetCameraRigTransform()
        {
            return cameraRigTransform;
        }

        public override Vector3 GetControllerPoint()
        {
            return controller.transform.position;
        }

        public override Vector3 GetControllerDirection()
        {
            return controller.transform.forward;
        }
    }
}