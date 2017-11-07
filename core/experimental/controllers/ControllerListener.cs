using UnityEngine;

namespace WorldWizards.core.experimental.controllers
{
    public abstract class ControllerListener : MonoBehaviour
    {
        private SteamVR_TrackedController controller;
        protected Tool tool;

        protected virtual void Awake()
        {
            controller = GetComponent<SteamVR_TrackedController>();
            tool.Init(controller);
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

        #region Listener Functions
        private void OnTriggerClick(object sender, ClickedEventArgs e)
        {
            tool.OnTriggerClick();
        }
        private void OnTriggerUnclick(object sender, ClickedEventArgs e)
        {
            tool.OnTriggerUnclick();
        }
        private void OnGrip(object sender, ClickedEventArgs e)
        {
            tool.OnGrip();
        }
        private void OnUngrip(object sender, ClickedEventArgs e)
        {
            tool.OnUngrip();
        }
        private void OnMenuClick(object sender, ClickedEventArgs e)
        {
            tool.OnMenuClick();
        }
        private void OnMenuUnclick(object sender, ClickedEventArgs e)
        {
            tool.OnMenuUnclick();
        }
        private void OnPadClick(object sender, ClickedEventArgs e)
        {
            tool.OnPadClick(new Vector2(e.padX, e.padY));
        }
        private void OnPadUnclick(object sender, ClickedEventArgs e)
        {
            tool.OnPadUnclick();
        }
        private void OnPadTouch(object sender, ClickedEventArgs e)
        {
            tool.OnPadTouch(new Vector2(e.padX, e.padY));
        }
        private void OnPadUntouch(object sender, ClickedEventArgs e)
        {
            tool.OnPadUntouch();
        }
        #endregion
    }
}