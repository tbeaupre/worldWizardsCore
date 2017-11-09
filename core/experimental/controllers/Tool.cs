using UnityEngine;
using WorldWizards.core.viveControllers;

namespace WorldWizards.core.experimental.controllers
{
    public abstract class Tool : MonoBehaviour
    {
        protected const ulong GRIP_ID = SteamVR_Controller.ButtonMask.Grip;
        protected const ulong APP_MENU_ID = SteamVR_Controller.ButtonMask.ApplicationMenu;
        protected const ulong TOUCHPAD_ID = SteamVR_Controller.ButtonMask.Touchpad;
        protected const float DEADZONE_SIZE = 0.7f; // The distance from the center of the touchpad which is dead.

        protected SteamVR_TrackedController controller;

        public bool listenForTrigger = false;
        public bool listenForGrip = false;
        public bool listenForMenu = false;
        public bool listenForPress = false;
        public bool listenForTouch = false;
        
        private bool trigger = false;
        private bool grip = false;
        private bool menu = false;
        private bool press = false;
        private bool touch = false;
        protected Vector2 lastPadPos;

        public virtual void Init(SteamVR_TrackedController newController)
        {
            controller = newController;
        }

        public virtual void Update()
        {
            if (trigger)
            {
                UpdateTrigger();
            }
            if (grip)
            {
                UpdateGrip();
            }
            if (menu)
            {
                UpdateMenu();
            }
            if (press)
            {
                lastPadPos = new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
                UpdatePress(lastPadPos);
            }
            else if (touch)
            {
                lastPadPos = new Vector2(controller.controllerState.rAxis0.x, controller.controllerState.rAxis0.y);
                UpdateTouch(lastPadPos);
            }
        }
        
        // These methods are called when the button is pressed or released
        public void OnTriggerClick() { trigger = true; }
        public virtual void OnTriggerUnclick() { trigger = false; }
        
        public void OnGrip() { grip = true; }
        public virtual void OnUngrip() { grip = false; }
        
        public void OnMenuClick() { menu = true; }
        public virtual void OnMenuUnclick() { menu = false; }
        
        public void OnPadClick(Vector2 pos) { press = true; }
        public virtual void OnPadUnclick() { press = false; }
        
        public void OnPadTouch(Vector2 pos) { touch = true; }
        public virtual void OnPadUntouch() { touch = false; }
        
        // These methods are called if the button is held down
        protected virtual void UpdateTrigger() {}
        protected virtual void UpdateGrip() {}
        protected virtual void UpdateMenu() {}
        protected virtual void UpdatePress(Vector2 padPos) {}
        protected virtual void UpdateTouch(Vector2 padPos) {}
    }
}