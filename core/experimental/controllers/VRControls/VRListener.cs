using System;
using UnityEngine;
using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.VRControls
{
    public class VRListener : InputListener
    {
        private SteamVR_TrackedController controller;
        private SteamVR_ControllerManager cameraRig;
        private Transform headTransform;

        public void Init(bool canChange, Type initToolType)
        {
            canChangeTools = canChange;
            tool = gameObject.AddComponent(initToolType) as Tool;
            
            cameraRig = FindObjectOfType<SteamVR_ControllerManager>();
            headTransform = cameraRig.GetComponentInChildren<Camera>().transform;
        }

        protected void Awake()
        {
            controller = GetComponent<SteamVR_TrackedController>();
            
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

        public override Vector3 GetHeadOffset()
        {
            return cameraRig.transform.position - headTransform.position;
        }

        public override Transform GetCameraRigTransform()
        {
            return cameraRig.transform;
        }

        public override Vector3 GetControllerPoint()
        {
            return controller.transform.position;
        }

        public override Vector3 GetControllerDirection()
        {
            return controller.transform.forward;
        }

        protected override Vector2 GetCurrentPadPosition()
        {
            return new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
        }
    }
}