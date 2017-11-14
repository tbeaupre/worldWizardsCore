using UnityEngine;

namespace WorldWizards.core.experimental.controllers
{
    public abstract class ControllerListener : MonoBehaviour
    {
        private SteamVR_TrackedController controller;
        protected Tool tool;
        private bool debug = false;

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
            DebugMessage("OnTriggerClick", sender);
            tool.OnTriggerClick();
        }
        private void OnTriggerUnclick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnTriggerUnclick", sender);
            tool.OnTriggerUnclick();
        }
        private void OnGrip(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnGrip", sender);
            tool.OnGrip();
        }
        private void OnUngrip(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnUngrip", sender);
            tool.OnUngrip();
        }
        private void OnMenuClick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnMenuClick", sender);
            tool.OnMenuClick();
        }
        private void OnMenuUnclick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnMenuUnclick", sender);
            tool.OnMenuUnclick();
        }
        private void OnPadClick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadClick", sender);
            tool.OnPadClick(new Vector2(e.padX, e.padY));
        }
        private void OnPadUnclick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadUnclick", sender);
            tool.OnPadUnclick();
        }
        private void OnPadTouch(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadTouch", sender);
            tool.OnPadTouch(new Vector2(e.padX, e.padY));
        }
        private void OnPadUntouch(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadUntouch", sender);
            tool.OnPadUntouch();
        }
        #endregion

        private void DebugMessage(string functionName, object sender)
        {
            if (debug)
            {
                Debug.Log("ControllerListener::" + functionName + ": " + sender.ToString());
            }
        }
    }
}