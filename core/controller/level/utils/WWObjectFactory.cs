using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.coordinate.utils;
using worldWizards.core.controller.level;
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
        public static WWObjectData MockCreate(Coordinate coordinate, WWResourceNEW resource)
        {
            return CreateNew(WWType.Tile, null, coordinate, resource);
        }

        public static WWObjectData CreateNew(WWType type, MetaData metaData, Coordinate coordinate, WWResourceNEW resource)
        {
            return Create(Guid.NewGuid(), type, metaData, coordinate, resource);
        }

        public static WWObjectData Create(Guid id, WWType type, MetaData metaData, Coordinate coordinate,
            WWResourceNEW resource)
        {
            return new WWObjectData(id, type, metaData, coordinate, null, null, resource);
        }

        public static WWObject Instantiate(WWObjectData objectData)
        {
            Vector3 spawnPos = CoordinateHelper.convertWWCoordinateToUnityCoordinate(objectData.coordinate);
            Type type = objectData.GetWWType();
            GameObject gameObject = UnityEngine.GameObject.Instantiate(objectData.resMetaData.GetObject(), spawnPos, Quaternion.identity) as GameObject;

            WWObject wwObject = gameObject.AddComponent(type) as WWObject;
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
