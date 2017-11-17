using System;
using UnityEngine;
using worldWizards.core.input.Tools;

namespace worldWizards.core.input
{
    public abstract class InputListener : MonoBehaviour
    {
        private bool debug = false;
        
        protected Tool tool;
        protected bool canChangeTools = true;
        
        protected bool Trigger { get; private set; }
        protected bool Grip { get; private set; }
        protected bool Menu { get; private set; }
        protected bool Press { get; private set; }
        protected bool Touch { get; private set; }
        private Vector2 lastPadPos; // May be possible to remove.
        
        protected virtual void Update()
        {
            if (Trigger)
            {
                tool.UpdateTrigger();
            }
            if (Grip)
            {
                tool.UpdateGrip();
            }
            if (Menu)
            {
                tool.UpdateMenu();
            }
            if (Press)
            {
                lastPadPos = GetCurrentPadPosition();
                tool.UpdatePress(lastPadPos);
            }
            else if (Touch)
            {
                lastPadPos = GetCurrentPadPosition();
                tool.UpdateTouch(lastPadPos);
            }
        }

        public void ChangeTool(Type newToolType)
        {
            if (canChangeTools)
            {
                Destroy(GetComponent<Tool>());
                tool = gameObject.AddComponent(newToolType) as Tool;
            }
        }
        
        public abstract Vector3 GetHeadOffset();
        public abstract Transform GetCameraRigTransform();
        public abstract Vector3 GetControllerPoint();
        public abstract Vector3 GetControllerDirection();
        protected abstract Vector2 GetCurrentPadPosition();
        
        #region Listener Functions
        protected void OnTriggerClick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnTriggerClick", sender);
            Trigger = true;
        }
        protected void OnTriggerUnclick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnTriggerUnclick", sender);
            Trigger = false;
            tool.OnTriggerUnclick();
        }
        protected void OnGrip(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnGrip", sender);
            Grip = true;
        }
        protected void OnUngrip(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnUngrip", sender);
            Grip = false;
            tool.OnUngrip();
        }
        protected void OnMenuClick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnMenuClick", sender);
            Menu = true;
        }
        protected void OnMenuUnclick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnMenuUnclick", sender);
            Menu = false;
            tool.OnMenuUnclick();
        }
        protected void OnPadClick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnPadClick", sender);
            Press = true;
        }
        protected void OnPadUnclick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnPadUnclick", sender);
            Press = false;
            tool.OnPadUnclick(lastPadPos);
        }
        protected void OnPadTouch(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnPadTouch", sender);
            Touch = true;
        }
        protected void OnPadUntouch(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            DebugMessage("OnPadUntouch", sender);
            Touch = false;
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