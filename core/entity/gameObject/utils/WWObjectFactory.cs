using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.resources;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject.resource;
using WorldWizards.core.entity.gameObject.resource.metaData;
using Object = UnityEngine.Object;

namespace WorldWizards.core.entity.gameObject.utils
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    ///  The World Wizard Object Factory is responsible for instantiating gameobjects into the
    ///  Unity environment.
    /// </summary>
    public static class WWObjectFactory
    {
        public static WWObjectData CreateNew(WWTransform wwTransform, string resourceTag)
        {
            return Create(Guid.NewGuid(), wwTransform, resourceTag);
        }

        public static WWObjectData Create(Guid id, WWTransform wwTransform, string resourceTag)
        {
            return new WWObjectData(id, wwTransform, null, new List<WWObjectData>(), resourceTag);
        }

        public static WWObject Instantiate(WWObjectData objectData)
        {
            Vector3 spawnPos = CoordinateHelper.WWCoordToUnityCoord(objectData.wwTransform.coordinate);

            // Load resource and check to see if it is valid.
            WWResource resource = WWResourceController.GetResource(objectData.resourceTag);
            GameObject gameObject;
            WWResourceMetadata resourceMetadata = resource.GetMetaData();
            if (resource.GetPrefab() == null)
            {
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                resourceMetadata = gameObject.AddComponent<WWResourceMetadata>();
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
                    Quaternion.Euler(0, objectData.wwTransform.rotation, 0));
            }

            // Use ResourceMetaData to construct the object.
            WWObject wwObject = ConstructWWObject(gameObject, resourceMetadata);
            // Give the new WWObject the data used to create it.
            wwObject.Init(objectData, resourceMetadata);
            wwObject.SetTransform(objectData.wwTransform);
            return wwObject;
        }

        /// <summary>
        ///     Handles the aspects of WWObject Instantiation that rely on the resource metadata.
        /// </summary>
        /// <param name="gameObject">The base GameObject which contains only resource, location, and rotation data</param>
        /// <param name="metadata">The metadata which will be used to construct the WWObject</param>
        /// <returns></returns>
        public static WWObject ConstructWWObject(GameObject gameObject, WWResourceMetadata metadata)
        {
            // Make the GameObject into a Tile, Prop, etc.
            Type type = WWTypeHelper.ConvertToSysType(metadata.wwObjectMetadata.type);
            var wwObject = gameObject.AddComponent(type) as WWObject;

            // Scale the object to the current tile scale.
            wwObject.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            // remove the WWResourceMetadata component for a microptimization
#if UNITY_EDITOR
            Object.DestroyImmediate(wwObject.GetComponent<WWResourceMetadata>());
#else
			GameObject.Destroy(wwObject.GetComponent<WWResourceMetadata>());
#endif

            wwObject.gameObject.SetActive(true);

            return wwObject;
        }
    }
}