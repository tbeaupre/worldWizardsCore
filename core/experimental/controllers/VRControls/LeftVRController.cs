using worldWizards.core.experimental.controllers.Tools;
using WorldWizards.core.experimental.controllers;

namespace worldWizards.core.experimental.controllers.VRControls
{
    public class LeftVRController : VRListener
    {
        protected override void Awake()
        {
            tool = gameObject.AddComponent<StandardTool>();
            base.Awake();
        }
    }
}