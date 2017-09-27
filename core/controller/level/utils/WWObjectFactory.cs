using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.coordinate.utils;
using worldWizards.core.controller.level;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace worldWizards.core.controller.level.utils
{
    /// <summary>
    /// The WorldWizard Object Factory is responsible for instantiating gameobjects into the
    /// unity environment.
    /// </summary>
    public static class WWObjectFactory
    {

        public static WWObjectData MockCreate(Coordinate coordinate, string resourceTag)
        {
            return CreateNew(null, coordinate, resourceTag);
        }
			
		public static WWObjectData MockCreateProp(Coordinate coordinate, string resourceTag)
		{
			return CreateNew( new MetaData(), coordinate, resourceTag);
		}

        public static WWObjectData CreateNew(MetaData metaData, Coordinate coordinate, string resourceTag)
        {
            return Create(Guid.NewGuid(), metaData, coordinate, resourceTag);
        }

        public static WWObjectData Create(Guid id, MetaData metaData, Coordinate coordinate, string resourceTag)
        {
			return new WWObjectData(id, metaData, coordinate, null, new List<WWObjectData>(), resourceTag);
        }

        public static WWObject Instantiate(WWObjectData objectData)
        {
            Vector3 spawnPos = CoordinateHelper.convertWWCoordinateToUnityCoordinate(objectData.coordinate);

            // Load resource and check to see if it is valid.
            WWResource resource = WWResourceController.GetResource(objectData.resourceTag);
            GameObject gameObject;
            WWResourceMetaData metaData = resource.GetMetaData();
            if (resource.GetPrefab() == null)
            {
                gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                metaData = gameObject.AddComponent<WWResourceMetaData>();
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
                gameObject = UnityEngine.GameObject.Instantiate(resource.GetPrefab(), spawnPos, Quaternion.identity);
            }

            // Use ResourceMetaData to construct the object.
            WWObject wwObject = ConstructWWObject(gameObject, metaData);

            // Give the new WWObject the data used to create it.
            wwObject.Init(objectData);

            return wwObject;
        }

        /// <summary>
        /// Handles the aspects of WWObject Instantiation that rely on the resource metadata.
        /// </summary>
        /// <param name="gameObject">The base GameObject which contains only resource, location, and rotation data</param>
        /// <param name="metaData">The metadata which will be used to construct the WWObject</param>
        /// <returns></returns>
        public static WWObject ConstructWWObject(GameObject gameObject, WWResourceMetaData metaData)
        {
            // Make the GameObject into a Tile, Prop, etc.
            Type type = WWTypeHelper.ConvertToSysType(metaData.type);
            WWObject wwObject = gameObject.AddComponent(type) as WWObject;

            // Add Collision Boxes to Object.

            // Scale the object to the current tile scale.
            wwObject.transform.localScale = Vector3.one * CoordinateHelper.tileLength;

			// remove the WWResourceMetaData component for a microptimization
			#if UNITY_EDITOR
			GameObject.DestroyImmediate(wwObject.GetComponent<WWResourceMetaData>());
			#else
			GameObject.Destroy(wwObject.GetComponent<WWResourceMetaData>());
			#endif

            wwObject.gameObject.SetActive(true);

            return wwObject;
        }
    }
}
