namespace WorldWizards.core.manager
{
    /// <summary>
    ///     Manager Registry has references to all managers.
    /// </summary>
    public class ManagerRegistry : Singleton<ManagerRegistry>
    {
        protected ManagerRegistry()
        {
        } // guarantee this will be always a singleton only - can't use the constructor!

        public SceneGraphManager sceneGraphManager { get; private set; }
        

        // Use this for initialization
        private void Start()
        {
            sceneGraphManager = new SceneGraphManagerImpl();
        }
    }
}