using UnityEngine;

namespace worldWizards.core.input.Tools
{
    public abstract class Tool : MonoBehaviour
    {
        protected const float DEADZONE_SIZE = 0.7f; // The distance from the center of the touchpad which is dead.
        protected InputListener controller;

        protected virtual void Awake()
        {
            controller = GetComponentInParent<InputListener>();
        }

        // These methods are called when the button is released
        public virtual void OnTriggerUnclick() {}
        public virtual void OnUngrip() {}
        public virtual void OnMenuUnclick() {}
        public virtual void OnPadUnclick(Vector2 lastPadPos) {}
        public virtual void OnPadUntouch(Vector2 lastPadPos) {}
        
        // These methods are called if the button is held down
        public virtual void UpdateTrigger() {}
        public virtual void UpdateGrip() {}
        public virtual void UpdateMenu() {}
        public virtual void UpdatePress(Vector2 padPos) {}
        public virtual void UpdateTouch(Vector2 padPos) {}
    }
}