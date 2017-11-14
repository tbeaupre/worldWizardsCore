using System;
using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.VRControls
{
    public class RightVRController : VRListener
    {
        protected override void Awake()
        {
            tool = gameObject.AddComponent<CreateObjectTool>();
            base.Awake();
        }

        public void ChangeTool(Type type)
        {
            Destroy(GetComponent<Tool>());
            tool = gameObject.AddComponent(type) as Tool;
        }
    }
}