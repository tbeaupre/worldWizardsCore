using UnityEngine;
using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.Desktop
{
    public class DesktopListener : InputListener
    {
        private KeyCode trigger;
        private KeyCode grip;
        private KeyCode menu;
        private KeyCode up;
        private KeyCode down;
        private KeyCode left;
        private KeyCode right;
        private KeyCode pressMod;
        
        private bool press = false;
        private bool padRelease = true;
        private Vector2 padPos = Vector2.zero;

        public void Init(KeyCode trigger, KeyCode grip, KeyCode menu,
            KeyCode up, KeyCode down, KeyCode left, KeyCode right, KeyCode pressMod,
            Tool tool)
        {
            this.trigger = trigger;
            this.grip = grip;
            this.menu = menu;
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
            this.pressMod = pressMod;
            this.tool = tool;
        }

        public void ChangeTool(Tool newTool)
        {
            tool = newTool;
        }
        
        private void OnGUI()
        {
            object sender = "Desktop";
            
            Event e = Event.current;
            if (e.isKey)
            {
                if (e.keyCode == trigger)
                {
                    if(e.type == EventType.KeyDown) OnTriggerClick(sender);
                    else OnTriggerUnclick(sender);
                    return;
                }
                if (e.keyCode == grip)
                {
                    if(e.type == EventType.KeyDown) OnGrip(sender);
                    else OnUngrip(sender);
                    return;
                }
                if (e.keyCode == menu)
                {
                    if(e.type == EventType.KeyDown) OnMenuClick(sender);
                    else OnMenuUnclick(sender);
                    return;
                }
                if (e.keyCode == pressMod)
                {
                    press = e.type == EventType.KeyDown;
                    return;
                }

                padRelease = false; // Changed in ChangePad() based on EventType
                if (e.keyCode == up)
                {
                    ChangePad(e, new Vector2(0, 1));
                }
                if (e.keyCode == down)
                {
                    ChangePad(e, new Vector2(0, -1));
                }
                if (e.keyCode == left)
                {
                    ChangePad(e, new Vector2(-1, 0));
                }
                if (e.keyCode == right)
                {
                    ChangePad(e, new Vector2(1, 0));
                }

                if (padRelease)
                {
                    if (press) OnPadUnclick(padPos);
                    else OnPadUntouch(padPos);
                }
                else if (padPos != Vector2.zero) // Digital pad has been pressed.
                {
                    if (press) OnPadClick(padPos);
                    else OnPadTouch(padPos);
                }
            }
        }

        private void ChangePad(Event e, Vector2 offset)
        {
            if (e.type == EventType.KeyDown)
            {
                padPos += offset;
            }
            else
            {
                padRelease = true;
                padPos -= offset;
            }
        }
        
        protected override Transform GetCurrentTransform()
        {
            return Camera.main.transform;
        }

        protected override Vector2 GetCurrentPadPosition()
        {
            return padPos;
        }
    }
}