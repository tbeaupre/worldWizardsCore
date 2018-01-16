using System;
using UnityEngine;
using worldWizards.core.input.Tools;

namespace worldWizards.core.input
{
    public abstract class InputListener : MonoBehaviour
    {
        protected Tool tool; // The tool which is currently attached to this controller.
        protected bool canChangeTools = true;
        
        protected bool Trigger { get; private set; } // True while the trigger is held down.
        protected bool Grip { get; private set; }    // True while the grip is held down.
        protected bool Menu { get; private set; }    // True while the menu button is held down.
        protected bool Press { get; private set; }   // True while the touchpad is pressed.
        protected bool Touch { get; private set; }   // True while the touchpad is touched.
        
        private Vector2 lastPadPos; // Tracks the last place the pad was pressed/touched for use with unclick function
        
        // Checks the state of the different buttons and updates the tool if necessary.
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
        
        // Gets the position of the user's finger on the touchpad (or keys on desktop).
        protected abstract Vector2 GetCurrentPadPosition();

        public void ChangeTool(Type newToolType)
        {
            if (canChangeTools)
            {
                Destroy(tool);
                tool = gameObject.AddComponent(newToolType) as Tool;
            }
        }
        
        // For retrieving data which is different in VR and desktop modes.
        public abstract Vector3 GetHeadOffset();           // Location of the player's head relative to their body.
        public abstract Transform GetCameraRigTransform(); // Location of the player's body in space.
        public abstract Vector3 GetControllerPoint();      // Location of the controller in space.
        public abstract Vector3 GetControllerDirection();  // Direction the controller is pointing to.
        
        #region Listener Functions
        // For processing inputs. Handles flags for updates and calls unclick functions.
        // MUST follow the format: void Foo(object sender, ClickedEventArgs e)
        protected void OnTriggerClick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Trigger = true;
        }
        protected void OnTriggerUnclick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Trigger = false;
            tool.OnTriggerUnclick();
        }
        protected void OnGrip(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Grip = true;
        }
        protected void OnUngrip(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Grip = false;
            tool.OnUngrip();
        }
        protected void OnMenuClick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Menu = true;
        }
        protected void OnMenuUnclick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Menu = false;
            tool.OnMenuUnclick();
        }
        protected void OnPadClick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Press = true;
        }
        protected void OnPadUnclick(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Press = false;
            tool.OnPadUnclick(lastPadPos);
        }
        protected void OnPadTouch(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Touch = true;
        }
        protected void OnPadUntouch(object sender, ClickedEventArgs e = new ClickedEventArgs())
        {
            Touch = false;
            tool.OnPadUntouch(lastPadPos);
        }
        #endregion

        public string GetToolName()
        {
            return tool.GetToolName();
        }
    }
}