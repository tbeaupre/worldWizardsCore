using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.controller.level.utils
{
    /// <summary>
    /// The WorldWizard Object Factory is responsible for instantiating gameobjects into the 
    /// unity environment.
    /// </summary>
    public static class WorldWizardObjectFactory
    {
        public static WorldWizardsObject Instantiate(WorldWizardsType type, string id, Coordinate coordinate)
        {
            return null;
        }

        private static WorldWizardsObject InstantiateTile()
        {
            return null;
        }

        private static WorldWizardsObject InstantiateProp()
        {
            return null;
        }

        private static WorldWizardsObject InstantiateInteractable()
        {
            return null;
        }
    }
}
