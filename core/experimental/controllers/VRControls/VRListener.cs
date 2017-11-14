using UnityEngine;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.VRControls
{
    public abstract class VRListener : InputListener
    {
        private SteamVR_TrackedController controller;

        protected virtual void Awake()
        {
            controller = GetComponent<SteamVR_TrackedController>();
            if (tool.listenForTrigger)
            {
                controller.TriggerClicked += OnTriggerClick;
                controller.TriggerUnclicked += OnTriggerUnclick;
            }
            if (tool.listenForGrip)
            {
                controller.Gripped += OnGrip;
                controller.Ungripped += OnUngrip;
            }
            if (tool.listenForMenu)
            {
                controller.MenuButtonClicked += OnMenuClick;
                controller.MenuButtonUnclicked += OnMenuUnclick;
            }
            if (tool.listenForPress)
            {
                controller.PadClicked += OnPadClick;
                controller.PadUnclicked += OnPadUnclick;
            }
            if (tool.listenForTouch)
            {
                controller.PadTouched += OnPadTouch;
                controller.PadUntouched += OnPadUntouch;
            }
        }

        protected override Transform GetCurrentTransform()
        {
            return controller.transform;
        }

        protected override Vector2 GetCurrentPadPosition()
        {
            return new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
        }

        #region Listener Functions
        private void OnTriggerClick(object sender, ClickedEventArgs e)
        {
            OnTriggerClick(sender);
        }
        private void OnTriggerUnclick(object sender, ClickedEventArgs e)
        {
            OnTriggerUnclick(sender);
        }
        private void OnGrip(object sender, ClickedEventArgs e)
        {
            OnGrip(sender);
        }
        private void OnUngrip(object sender, ClickedEventArgs e)
        {
            OnUngrip(sender);
        }
        private void OnMenuClick(object sender, ClickedEventArgs e)
        {
            OnMenuClick(sender);
        }
        private void OnMenuUnclick(object sender, ClickedEventArgs e)
        {
            OnMenuUnclick(sender);
        }
        private void OnPadClick(object sender, ClickedEventArgs e)
        {
            OnPadClick(sender);
        }
        private void OnPadUnclick(object sender, ClickedEventArgs e)
        {
            OnPadUnclick(sender);
        }
        private void OnPadTouch(object sender, ClickedEventArgs e)
        {
            OnPadTouch(sender);
        }
        private void OnPadUntouch(object sender, ClickedEventArgs e)
        {
            OnPadUntouch(sender);
        }
        #endregion
    }
}