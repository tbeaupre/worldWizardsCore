using UnityEngine;

namespace WorldWizards.core.experimental.controllers
{
    public abstract class ControllerListener : MonoBehaviour
    {
        private SteamVR_TrackedController controller;
        protected Tool tool;
        private bool debug = false;
        
        private bool trigger = false;
        private bool grip = false;
        private bool menu = false;
        private bool press = false;
        private bool touch = false;
        private Vector2 lastPadPos; // May be possible to remove.

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
        
        private void Update()
        {
            tool.UpdateTransform(controller.transform);
            
            if (trigger)
            {
                tool.UpdateTrigger();
            }
            if (grip)
            {
                tool.UpdateGrip();
            }
            if (menu)
            {
                tool.UpdateMenu();
            }
            if (press)
            {
                lastPadPos = new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
                tool.UpdatePress(lastPadPos);
            }
            else if (touch)
            {
                lastPadPos = new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
                tool.UpdateTouch(lastPadPos);
            }
        }

        #region Listener Functions
        private void OnTriggerClick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnTriggerClick", sender);
            trigger = true;
        }
        private void OnTriggerUnclick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnTriggerUnclick", sender);
            trigger = false;
            tool.OnTriggerUnclick();
        }
        private void OnGrip(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnGrip", sender);
            grip = true;
        }
        private void OnUngrip(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnUngrip", sender);
            grip = false;
            tool.OnUngrip();
        }
        private void OnMenuClick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnMenuClick", sender);
            menu = true;
        }
        private void OnMenuUnclick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnMenuUnclick", sender);
            menu = false;
            tool.OnMenuUnclick();
        }
        private void OnPadClick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadClick", sender);
            press = true;
        }
        private void OnPadUnclick(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadUnclick", sender);
            press = false;
            tool.OnPadUnclick(new Vector2(e.padX, e.padY));
        }
        private void OnPadTouch(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadTouch", sender);
            touch = true;
        }
        private void OnPadUntouch(object sender, ClickedEventArgs e)
        {
            DebugMessage("OnPadUntouch", sender);
            touch = false;
            tool.OnPadUntouch(new Vector2(e.padX, e.padY));
        }
        #endregion

        private void DebugMessage(string functionName, object sender)
        {
            if (debug)
            {
                Debug.Log("ControllerListener::" + functionName + ": " + sender);
            }
        }
    }
}