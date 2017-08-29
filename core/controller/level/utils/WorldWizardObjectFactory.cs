using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;
using System;
using UnityEngine;

namespace worldWizards.core.controller.level.utils
{
    /// <summary>
    /// The WorldWizard Object Factory is responsible for instantiating gameobjects into the 
    /// unity environment.
    /// </summary>
    public static class WorldWizardObjectFactory
    {
        public static WorldWizardsObject Instantiate(WorldWizardsObject obj, WorldWizardsType type, Coordinate coordinate, string prefabPath)
        {
            // TODO: Set properties and return
            return UnityEngine.GameObject.Instantiate<WorldWizardsObject>(obj);
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
