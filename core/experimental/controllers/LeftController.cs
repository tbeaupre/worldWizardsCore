namespace WorldWizards.core.experimental.controllers
{
    public class LeftController : ControllerListener
    {
        protected override void Awake()
        {
            tool = gameObject.AddComponent<StandardTool>();
            base.Awake();
        }
    }
}