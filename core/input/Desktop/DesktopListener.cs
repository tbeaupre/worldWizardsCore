using System;
using UnityEngine;
using WorldWizards.core.input.Tools;

namespace WorldWizards.core.input.Desktop
{
    /**
     * Holds information specific to Desktop Controllers and handles key presses.
     */
    public class DesktopListener : InputListener
    {
        private readonly Vector2 headDelta = new Vector3(0, 4, 0); // Arbitrary offset since there is no head in desktop mode.
        
        // How the key presses affect the position of the press/touch on the touchpad.
        private readonly Vector2 upDelta = new Vector2(0, 1);
        private readonly Vector2 downDelta = new Vector2(0, -1);
        private readonly Vector2 leftDelta = new Vector2(-1, 0);
        private readonly Vector2 rightDelta = new Vector2(1, 0);
        
        private ControlScheme controls;

        private bool pressMod;                 // The state of the touchpad touch/press modifier.
        private int pressNum;                  // Keeps track of the number of touchpad keys currently pressed.
        private Vector2 padPos = Vector2.zero; // The sum of the touchpad keys currently pressed.

        public void Init(ControlScheme controlScheme, bool canChange, Type toolType)
        {
            controls = controlScheme;
            canChangeTools = canChange;
            tool = gameObject.AddComponent(toolType) as Tool;
        }

        protected override void Update()
        {
            object sender = "Desktop"; // Because the InputListener's listeners require a sender.
            if (Trigger)
            {
                if (Input.GetKeyUp(controls.triggerKey)) OnTriggerUnclick(sender);
            }
            else
            {
                if (Input.GetKeyDown(controls.triggerKey)) OnTriggerClick(sender);
            }

            if (Grip)
            {
                if (Input.GetKeyUp(controls.gripKey)) OnUngrip(sender);
            }
            else
            {
                if (Input.GetKeyDown(controls.gripKey)) OnGrip(sender);
            }

            if (Menu)
            {
                if (Input.GetKeyUp(controls.menuKey)) OnMenuUnclick(sender);
            }
            else
            {
                if (Input.GetKeyDown(controls.menuKey)) OnMenuClick(sender);
            }

            pressMod = Input.GetKey(controls.pressModKey);

            if (Input.GetKeyDown(controls.upKey))
            {
                padPos += upDelta;
                PadPress();
                pressNum++;
            }
            if (Input.GetKeyDown(controls.downKey))
            {
                padPos += downDelta;
                PadPress();
                pressNum++;
            }
            if (Input.GetKeyDown(controls.leftKey))
            {
                padPos += leftDelta;
                PadPress();
                pressNum++;
            }
            if (Input.GetKeyDown(controls.rightKey))
            {
                padPos += rightDelta;
                PadPress();
                pressNum++;
            }
            if (Press || Touch)
            {
                if (Input.GetKeyUp(controls.upKey))
                {
                    padPos -= upDelta;
                    pressNum--;
                }
                if (Input.GetKeyUp(controls.downKey))
                {
                    padPos -= downDelta;
                    pressNum--;
                }
                if (Input.GetKeyUp(controls.leftKey))
                {
                    padPos -= leftDelta;
                    pressNum--;
                }
                if (Input.GetKeyUp(controls.rightKey))
                {
                    padPos -= rightDelta;
                    pressNum--;
                }
                
                if (pressNum == 0)
                {
                    if (pressMod) OnPadUnclick(sender);
                    else OnPadUntouch(sender);
                }
            }
            base.Update();
        }

        // Triggers the OnPadClick/Touch functions if this is the first touchpad key pressed.
        private void PadPress()
        {
            if (pressNum == 0)
            {
                object sender = "Desktop";
                if (pressMod) OnPadClick(sender);
                else OnPadTouch(sender);
            }
        }
        
        protected override Vector2 GetCurrentPadPosition()
        {
            return padPos;
        }

        public override Vector3 GetHeadOffset()
        {
            return headDelta;
        }

        public override Transform GetCameraRigTransform()
        {
            return Camera.main.transform;
        }

        public override Vector3 GetControllerPoint()
        {
            return Camera.main.transform.position;
        }

        public override Vector3 GetControllerDirection()
        {
            // Controller points towards the mouse location.
            return Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        }
    }
}