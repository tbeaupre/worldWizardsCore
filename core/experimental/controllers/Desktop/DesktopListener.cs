using System;
using System.Text;
using Newtonsoft.Json.Bson;
using UnityEngine;
using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.Desktop
{
    public class DesktopListener : InputListener
    {
        private ControlScheme controls;
        
        private readonly Vector2 upDelta = new Vector2(0, 1);
        private readonly Vector2 downDelta = new Vector2(0, -1);
        private readonly Vector2 leftDelta = new Vector2(-1, 0);
        private readonly Vector2 rightDelta = new Vector2(1, 0);

        private bool pressMod;
        private int pressNum;
        private Vector2 padPos = Vector2.zero;

        public void Init(ControlScheme controlScheme, bool canChange, Type toolType)
        {
            controls = controlScheme;
            canChangeTools = canChange;
            tool = gameObject.AddComponent(toolType) as Tool;
        }

        protected override void Update()
        {
            object sender = "Desktop";
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

        private void PadPress()
        {
            if (pressNum == 0)
            {
                object sender = "Desktop";
                if (pressMod) OnPadClick(sender);
                else OnPadTouch(sender);
            }
        }

        public override Vector3 GetHeadOffset()
        {
            return new Vector3(0, 4, 0);
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
            return Camera.main.ScreenPointToRay(Input.mousePosition).direction;
        }
        
        
        protected override Vector2 GetCurrentPadPosition()
        {
            return padPos;
        }
    }
}