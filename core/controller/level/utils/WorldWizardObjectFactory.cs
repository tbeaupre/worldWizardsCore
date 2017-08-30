using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.coordinate.utils;
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
        public static WWObjectData MockCreate(Coordinate coordinate)
        {
            return CreateNew(WorldWizardsType.Tile, null, coordinate, new WWResource("tileTemp"));
        }

        public static WWObjectData CreateNew(WorldWizardsType type, MetaData metaData, Coordinate coordinate, WWResource resource)
        {
            return Create(Guid.NewGuid(), type, metaData, coordinate, resource);
        }

        public static WWObjectData Create(Guid id, WorldWizardsType type, MetaData metaData, Coordinate coordinate,
            WWResource resource)
        {

            //worldWizardsObject obj = Resources.Load<WorldWizardsObject>(resource.path);
            //obj.Init(id, type, metaData, coordinate, resource, null, null);
            return new WWObjectData(id, type, metaData, coordinate, resource, null, null);
        }

        public static WorldWizardsObject Instantiate(WWObjectData objectData)
        {
            Vector3 spawnPos = CoordinateHelper.convertWWCoordinateToUnityCoordinate(objectData.coordinate);
            WorldWizardsObject wwObject = UnityEngine.GameObject.Instantiate<WorldWizardsObject>(
                Resources.Load<WorldWizardsObject>(objectData.resource.path), spawnPos, Quaternion.identity);
            wwObject.transform.localScale = Vector3.one * CoordinateHelper.tileLength;



            wwObject.Init(objectData);

            return wwObject;
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
