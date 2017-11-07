using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WorldWizards.core.experimental.controllers
{
    public abstract class Tool : MonoBehaviour
    {
        protected const ulong GRIP_ID = SteamVR_Controller.ButtonMask.Grip;
        protected const ulong APP_MENU_ID = SteamVR_Controller.ButtonMask.ApplicationMenu;
        protected const ulong TOUCHPAD_ID = SteamVR_Controller.ButtonMask.Touchpad;
        protected const float DEADZONE_SIZE = 0.7f; // The distance from the center of the touchpad which is dead.
        
        protected SteamVR_TrackedObject trackedObj;
        protected SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int)trackedObj.index); }
        }

        protected bool trigger = false;
        protected bool grip = false;
        protected bool appMenu = false;
        protected bool press = false;
        protected bool touch = false;

        protected virtual void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        protected virtual void Update()
        {
            if (trigger)
            {
                if (Controller.GetHairTrigger())
                {
                    UpdateTrigger();
                }
                else if (Controller.GetHairTriggerUp())
                {
                    OnTrigger();
                }
            }
            if (grip)
            {
                if (Controller.GetPress(GRIP_ID))
                {
                    UpdateGrip();
                }
                else if (Controller.GetPressUp(GRIP_ID))
                {
                    OnGrip();
                }
            }
            if (appMenu)
            {
                if (Controller.GetPress(APP_MENU_ID))
                {
                    UpdateAppMenu();
                }
                if (Controller.GetPressUp(APP_MENU_ID))
                {
                    OnAppMenu();
                }
            }
            if (press)
            {
                if (Controller.GetPress(TOUCHPAD_ID))
                {
                    UpdateTouchpadPress(Controller.GetAxis()); 
                }
                if (Controller.GetPressUp(TOUCHPAD_ID))
                {
                    OnTouchpadPress(Controller.GetAxis());
                }
            }
            if (touch)
            {
                if (Controller.GetTouch(TOUCHPAD_ID))
                {
                    UpdateTouchpadTouch(Controller.GetAxis());
                }
                if (Controller.GetTouchUp(TOUCHPAD_ID))
                {
                    OnTouchpadTouch(Controller.GetAxis());
                }
            }
        }

        // These methods are called when the button is released
        protected virtual void OnTrigger() {}
        protected virtual void OnGrip() {}
        protected virtual void OnAppMenu() {}
        protected virtual void OnTouchpadPress(Vector2 pos) {}
        protected virtual void OnTouchpadTouch(Vector2 pos) {}
        
        // These methods are called if the button is held down
        protected virtual void UpdateTrigger() {}
        protected virtual void UpdateGrip() {}
        protected virtual void UpdateAppMenu() {}
        protected virtual void UpdateTouchpadPress(Vector2 pos) {}
        protected virtual void UpdateTouchpadTouch(Vector2 pos) {}
    }
}