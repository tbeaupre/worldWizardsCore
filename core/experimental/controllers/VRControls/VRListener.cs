using System;
using UnityEngine;
using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.VRControls
{
    public class VRListener : InputListener
    {
        private SteamVR_TrackedController controller;

        public void Init(bool canChange, Type initToolType)
        {
            canChangeTools = canChange;
            tool = gameObject.AddComponent(initToolType) as Tool;
        }

        protected virtual void Awake()
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
            // TODO Actually put this stuff in here.
            return Vector3.forward;
        }

        public override Transform GetHeadTransform()
        {
            throw new NotImplementedException();
        }

        public override Transform GetControllerTransform()
        {
            return controller.transform;
        }

        protected override Vector2 GetCurrentPadPosition()
        {
            return new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
        }
    }
}