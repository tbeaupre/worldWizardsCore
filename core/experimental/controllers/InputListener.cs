using UnityEngine;

namespace WorldWizards.core.experimental.controllers
{
    public abstract class InputListener : MonoBehaviour
    {
        protected Tool tool;
        private bool debug = false;
        
        private bool trigger = false;
        private bool grip = false;
        private bool menu = false;
        private bool press = false;
        private bool touch = false;
        private Vector2 lastPadPos; // May be possible to remove.
        
        private void Update()
        {
            tool.UpdateTransform(GetCurrentTransform());
            
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
                lastPadPos = GetCurrentPadPosition();
                tool.UpdatePress(lastPadPos);
            }
            else if (touch)
            {
                lastPadPos = GetCurrentPadPosition();
                tool.UpdateTouch(lastPadPos);
            }
        }

        protected abstract Transform GetCurrentTransform();

        protected abstract Vector2 GetCurrentPadPosition();
        
        #region Listener Functions
        protected void OnTriggerClick(object sender)
        {
            DebugMessage("OnTriggerClick", sender);
            trigger = true;
        }
        protected void OnTriggerUnclick(object sender)
        {
            DebugMessage("OnTriggerUnclick", sender);
            trigger = false;
            tool.OnTriggerUnclick();
        }
        protected void OnGrip(object sender)
        {
            DebugMessage("OnGrip", sender);
            grip = true;
        }
        protected void OnUngrip(object sender)
        {
            DebugMessage("OnUngrip", sender);
            grip = false;
            tool.OnUngrip();
        }
        protected void OnMenuClick(object sender)
        {
            DebugMessage("OnMenuClick", sender);
            menu = true;
        }
        protected void OnMenuUnclick(object sender)
        {
            DebugMessage("OnMenuUnclick", sender);
            menu = false;
            tool.OnMenuUnclick();
        }
        protected void OnPadClick(object sender)
        {
            DebugMessage("OnPadClick", sender);
            press = true;
        }
        protected void OnPadUnclick(object sender)
        {
            DebugMessage("OnPadUnclick", sender);
            press = false;
            tool.OnPadUnclick(lastPadPos);
        }
        protected void OnPadTouch(object sender)
        {
            DebugMessage("OnPadTouch", sender);
            touch = true;
        }
        protected void OnPadUntouch(object sender)
        {
            DebugMessage("OnPadUntouch", sender);
            touch = false;
            tool.OnPadUntouch(lastPadPos);
        }
        #endregion

        private void DebugMessage(string functionName, object sender)
        {
            if (debug)
            {
                Debug.Log("ControllerListener::" + functionName + "(): " + sender);
            }
        }
    }
}