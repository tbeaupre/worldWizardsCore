using System;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using WorldWizards.core.controller.builder;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject.resource.metaData;
using WorldWizards.core.entity.gameObject.utils;

namespace WorldWizards.core.entity.gameObject
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// The WWObject is an abstract base class for all World Wizards objects.
    /// WWObject extends MonoBehavior so it has to be attached to a GameObject.
    /// </summary>
    public abstract class WWObject : MonoBehaviour
    {
        public WWResourceMetadata ResourceMetadata { get; private set; }
        public WWObjectData objectData { get; private set; }

        /// <summary>
        /// Component that can fade the materials of the WWObject
        /// </summary>
        public MaterialSwitcher MaterialSwitcher { get; private set; }

        /// <summary>
        /// Visually show that the WWObject is selected.
        /// </summary>
        public void Select()
        {
            MaterialSwitcher.SwitchOn();
        }

        public List<Renderer> GetAllRenderers()
        {
            return MaterialSwitcher.GetAllRenderers();
        }

        /// <summary>
        /// Visually show that the WWObject is deselected.
        /// </summary>
        public void Deselect()
        {
            MaterialSwitcher.SwitchOff();
        }

        /// <summary>
        /// Initialize the WWObject
        /// </summary>
        /// <param name="objectData">The instance data for the WWObject.</param>
        /// <param name="resourceMetadata">The resource meta data that describes this WWObject.</param>
        public void Init(WWObjectData objectData, WWResourceMetadata resourceMetadata)
        {
            this.objectData = objectData;
            this.ResourceMetadata = resourceMetadata;
            var switchMaterial = Resources.Load("Materials/TileFadeMat") as Material;
            MaterialSwitcher = new MaterialSwitcher(gameObject, switchMaterial);
        }

        /// <summary>
        /// Get the id of this WWObject instance.
        /// </summary>
        /// <returns>The guid for this WWObject.</returns>
        public Guid GetId()
        {
            return objectData.id;
        }

        /// <summary>
        /// Gets the coordinate of this WWObject.
        /// </summary>
        /// <returns>the coordinate.</returns>
        public Coordinate GetCoordinate()
        {
            if (ResourceMetadata.wwObjectMetadata.type == WWType.Tile)
            {
                // we only want the index without the offset for Tiles
                return new Coordinate(objectData.wwTransform.coordinate.Index);
            }
            return objectData.wwTransform.coordinate;
        }

        /// <summary>
        /// Gets the y rotation of this WWObject
        /// </summary>
        /// <returns>the y rotation</returns>
        public int GetRotation()
        {
            return objectData.wwTransform.rotation;
        }

        /// <summary>
        /// Returns the Wall collisions for this WWObject, taking into consideration the object's current rotation.
        /// </summary>
        /// <returns>The Wall collisions after applying rotation</returns>
        public WWWalls GetWallsWRotationApplied()
        {
            return WWWallsHelper.GetRotatedWWWalls(ResourceMetadata, GetRotation());
        }

        public WWObject GetOldestParent()
        {
            // not yet implemented
            return null;
        }

        /// <summary>
        ///  Promote this World Wizard Object to not have a parent.
        /// </summary>
        public void Unparent()
        {
            objectData.Unparent();
        }

        /// <summary>
        /// Set this WWObject to be the child of the given parent WWObject. 
        /// </summary>
        /// <param name="parent">The parent to become a child of.</param>
        public void SetParent(WWObject parent)
        {
            objectData.SetParent(parent.objectData);
        }

        /// <summary>
        /// Removes the given WWObject from this object's list of children if it is a child.
        /// </summary>
        /// <param name="child">The child WWObject to remove.</param>
        public void RemoveChild(WWObject child)
        {
            var childrenToRemove = new List<WWObject>();
            childrenToRemove.Add(child);
            RemoveChildren(childrenToRemove);
        }

        /// <summary>
        /// Removes the given list of WWObjects from this object's list of children if it is a child.
        /// </summary>
        /// <param name="children">The list of children to attempt to remove.</param>
        public void RemoveChildren(List<WWObject> children)
        {
            foreach (WWObject child in children)
                if (objectData.children.Contains(child.objectData))
                {
                    objectData.RemoveChild(child.objectData);
                }
        }

        /// <summary>
        /// Adds the list of children to this WWObject's list of children.
        /// </summary>
        /// <param name="children">The list of children WWObjects to add</param>
        public void AddChildren(List<WWObject> children)
        {
            objectData.AddChildren(children);
            foreach (WWObject child in children) child.SetParent(this);
        }

        /// <summary>
        /// Get the children WWObjectDatas of this WWObject.
        /// </summary>
        /// <returns>This WWObject's children WWObjectDatas.</returns>
        public List<WWObjectData> GetChildren()
        {
            return objectData.children;
        }

        /// <summary>
        /// Get the WWObjectData of this WWObject's parent
        /// </summary>
        /// <returns>The WWObjectData of this WWObject's parent, or null if there is no parent.</returns>
        public WWObjectData GetParent()
        {
            return objectData.parent; // parent;
        }

        /// <summary>
        /// Returns all the descendent WWObjectDatas of this WWObject.
        /// </summary>
        /// <returns>All the descendents of this WWObject.</returns>
        public List<WWObjectData> GetAllDescendents()
        {
            return objectData.GetAllDescendents();
        }

        /// <summary>
        /// Set the rotation of this WwObject.
        /// </summary>
        /// <param name="yRotation">The rotation to set.</param>
        public void SetRotation(int yRotation)
        {
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            objectData.wwTransform.rotation = yRotation;
        }

        /// <summary>
        /// Set the position of this WWObject and update the coordinate.
        /// </summary>
        /// <param name="position">The position to set in Unity's coordinate space.</param>
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
            objectData.wwTransform.coordinate = CoordinateHelper.UnityCoordToWWCoord(position);
        }

        /// <summary>
        /// Set the position of this WWObject and update the coordinate.
        /// </summary>
        /// <param name="coordinate">The coordinate to set</param>
        public void SetPosition(Coordinate coordinate)
        {
            if (ResourceMetadata.wwObjectMetadata.type == WWType.Tile)
            {
                coordinate.SnapToGrid();
            }
            SetPosition(CoordinateHelper.WWCoordToUnityCoord(coordinate));
        }
        
        /// <summary>
        /// Set the WWTransform of this WWObject and the gameObject transform
        /// </summary>
        /// <param name="wwTransform">The wwTransform to set</param>
        public void SetTransform(WWTransform wwTransform)
        {
            SetPosition(wwTransform.coordinate);
            SetRotation(wwTransform.rotation);
        }
        
    }
}