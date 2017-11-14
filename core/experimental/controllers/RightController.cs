using System;

namespace WorldWizards.core.experimental.controllers
{
    public class RightController : ControllerListener
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