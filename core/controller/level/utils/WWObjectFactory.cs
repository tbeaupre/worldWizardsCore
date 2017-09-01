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
    public static class WWObjectFactory
    {
        public static WWObjectData MockCreate(Coordinate coordinate)
        {
            return CreateNew(WWType.Tile, null, coordinate, new WWResource("tileTemp"));
        }

        public static WWObjectData CreateNew(WWType type, MetaData metaData, Coordinate coordinate, WWResource resource)
        {
            return Create(Guid.NewGuid(), type, metaData, coordinate, resource);
        }

        public static WWObjectData Create(Guid id, WWType type, MetaData metaData, Coordinate coordinate,
            WWResource resource)
        {

            //worldWizardsObject obj = Resources.Load<WorldWizardsObject>(resource.path);
            //obj.Init(id, type, metaData, coordinate, resource, null, null);
            return new WWObjectData(id, type, metaData, coordinate, resource, null, null);
        }

        public static WWObject Instantiate(WWObjectData objectData)
        {
            Vector3 spawnPos = CoordinateHelper.convertWWCoordinateToUnityCoordinate(objectData.coordinate);
            WWObject wwObject = UnityEngine.GameObject.Instantiate<WWObject>(
                Resources.Load<WWObject>(objectData.resource.path), spawnPos, Quaternion.identity);
            wwObject.transform.localScale = Vector3.one * CoordinateHelper.tileLength;



            wwObject.Init(objectData);

            return wwObject;
        }

        private static WWObject InstantiateTile()
        {
            return null;
        }

        private static WWObject InstantiateProp()
        {
            return null;
        }

        private static WWObject InstantiateInteractable()
        {
            return null;
        }
    }
}
