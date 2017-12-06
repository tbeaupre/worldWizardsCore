using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.resource;
using WorldWizards.core.entity.gameObject.resource.metaData;
using Object = UnityEngine.Object;

namespace WorldWizards.core.controller.level.utils
{
    /// <summary>
    ///     The WorldWizard Object Factory is responsible for instantiating gameobjects into the
    ///     unity environment.
    /// </summary>
    public static class WWObjectFactory
    {
        public static WWObjectData CreateNew(Coordinate coordinate, string resourceTag)
        {
            return Create(Guid.NewGuid(), coordinate, resourceTag);
        }

        public static WWObjectData Create(Guid id, Coordinate coordinate, string resourceTag)
        {
            return new WWObjectData(id, coordinate, null, new List<WWObjectData>(), resourceTag);
        }

        public static WWObject Instantiate(WWObjectData objectData)
        {
            Vector3 spawnPos = CoordinateHelper.convertWWCoordinateToUnityCoordinate(objectData.coordinate);

            // Load resource and check to see if it is valid.
            WWResource resource = WWResourceController.GetResource(objectData.resourceTag);
            GameObject gameObject;
            WWResourceMetaData resourceMetaData = resource.GetMetaData();
            if (resource.GetPrefab() == null)
            {
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                resourceMetaData = gameObject.AddComponent<WWResourceMetaData>();
                gameObject.transform.Translate(spawnPos);
            }
            else
            {
                if (resource.GetMetaData() == null)
                {
                    Debug.Log("There is no metadata for this resource, so it cannot be instantiated.");
                    return null;
                }
                // Create a GameObject at the correct location and rotation.
                gameObject = Object.Instantiate(resource.GetPrefab(), spawnPos,
                    Quaternion.Euler(0, objectData.coordinate.rotation, 0));
            }

            // Use ResourceMetaData to construct the object.
            WWObject wwObject = ConstructWWObject(gameObject, resourceMetaData);
            // Give the new WWObject the data used to create it.
            wwObject.Init(objectData, resourceMetaData);
            wwObject.SetPosition(objectData.coordinate);

            return wwObject;
        }

        /// <summary>
        ///     Handles the aspects of WWObject Instantiation that rely on the resource metadata.
        /// </summary>
        /// <param name="gameObject">The base GameObject which contains only resource, location, and rotation data</param>
        /// <param name="metaData">The metadata which will be used to construct the WWObject</param>
        /// <returns></returns>
        public static WWObject ConstructWWObject(GameObject gameObject, WWResourceMetaData metaData)
        {
            // Make the GameObject into a Tile, Prop, etc.
            Type type = WWTypeHelper.ConvertToSysType(metaData.wwObjectMetaData.type);
            var wwObject = gameObject.AddComponent(type) as WWObject;

            // Scale the object to the current tile scale.
            wwObject.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            // remove the WWResourceMetaData component for a microptimization
#if UNITY_EDITOR
            Object.DestroyImmediate(wwObject.GetComponent<WWResourceMetaData>());
#else
			GameObject.Destroy(wwObject.GetComponent<WWResourceMetaData>());
			#endif

            wwObject.gameObject.SetActive(true);

            return wwObject;
        }
    }
}