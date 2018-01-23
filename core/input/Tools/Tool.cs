using UnityEngine;
using WorldWizards.core.controller.builder;

namespace WorldWizards.core.input.Tools
{
    public abstract class Tool : MonoBehaviour
    {
        protected const float DEADZONE_SIZE = 0.7f; // The distance from the center of the touchpad which is dead.
        protected InputListener input;
        protected GridController gridController;


        protected virtual void Awake()
        {
            input = GetComponentInParent<InputListener>();
            gridController = FindObjectOfType<GridController>();
        }

        // These methods are called when the button is released
        public virtual void OnTriggerUnclick() {}
        public virtual void OnUngrip() {}
        public virtual void OnMenuUnclick() {}
        public virtual void OnPadUnclick(Vector2 lastPadPos) {}
        public virtual void OnPadUntouch(Vector2 lastPadPos) {}
        
        // These methods are called while the button is held down
        public virtual void UpdateTrigger() {}
        public virtual void UpdateGrip() {}
        public virtual void UpdateMenu() {}
        public virtual void UpdatePress(Vector2 padPos) {}
        public virtual void UpdateTouch(Vector2 padPos) {}


        public string GetToolName()
        {
            return GetType().Name;
        }
    }
}