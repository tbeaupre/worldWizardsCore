using System;
using System.Collections.Generic;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.gameObject
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// The WWObjectData class holds all the properties that describes the current state of a WWObject.
    /// Currently these properties are: id, transform, parent, list of children, and the associated resource tag.
    /// </summary>
    public class WWObjectData
    {
        /// <summary>
        /// The unique identifier for this WWObjectData
        /// </summary>
        public Guid id { get; private set; }

        /// <summary>
        /// The transform for this WWObjectData
        /// </summary>
        public WWTransform wwTransform { get; set; }

        /// <summary>
        /// The parent of this WWObjectData. Null if there is no parent.
        /// </summary>
        public WWObjectData parent { get; set; }
        
        /// <summary>
        /// The list of children WWObjecDatas
        /// </summary>
        public List<WWObjectData> children { get; private set; }

        /// <summary>
        /// The resourceTag associated with this WWObject. Neccassary for getting the resource from an Asset Bundle.
        /// </summary>
        public string resourceTag { get; private set; }
        
        /// <summary>
        /// Constructs a new WWObjectData instance with the provided data.
        /// </summary>
        /// <param name="id">The unique id.</param>
        /// <param name="wwTransform">The WWTransform.</param>
        /// <param name="parent">The parent WWObjectData. Can be null</param>
        /// <param name="children">The list of children this WWObjectData is a parent of.</param>
        /// <param name="resourceTag">The resource tag</param>
        public WWObjectData(Guid id, WWTransform wwTransform,
            WWObjectData parent, List<WWObjectData> children, string resourceTag)
        {
            this.id = id;
            this.wwTransform = wwTransform;
            this.parent = parent;
            this.children = children;
            this.resourceTag = resourceTag;
        }

        /// <summary>
        /// Constructs a new WWObject data from a JSON blobl, presumably from a file.
        /// </summary>
        /// <param name="b"> the JSON blob</param>
        public WWObjectData(WWObjectJSONBlob b)
        {
            id = b.id;
            wwTransform = new WWTransform(b.wwTransform);
            resourceTag = b.resourceTag;
            // Note parent and children relationships are re-linked in the SceneGraphController
            // during the Load Operation
            parent = null;
            children = new List<WWObjectData>();
        }

        /// <summary>
        /// Add this list of children to this object's chidlren.
        /// </summary>
        /// <param name="children">The list of children to add</param>
        public void AddChildren(List<WWObject> children)
        {
            foreach (WWObject child in children) this.children.Add(child.objectData);
        }

        /// <summary>
        /// Get all of the descendents of this object.
        /// </summary>
        /// <returns>This object's descendents.</returns>
        public List<WWObjectData> GetAllDescendents()
        {
            var descendents = new List<WWObjectData>();
            foreach (var child in children)
            {
                descendents.Add(child);
                List<WWObjectData> childsDescendents = child.GetAllDescendents();
                foreach (var childsDescendent in childsDescendents)
                {
                    descendents.Add(childsDescendent);
                }
            }
            return descendents;
        }

        /// <summary>
        /// Unparents this object from its parent object if it has a parent. Also removes
        /// this object from the parent's list of children.
        /// </summary>
        public void Unparent()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
                parent = null;
            }
        }

        /// <summary>
        /// Set the parent object for this object. 
        /// </summary>
        /// <param name="parent">The parent object for this object to become a child of.</param>
        public void SetParent(WWObjectData parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Removes the given child if object is in fact a child of this object.
        /// </summary>
        /// <param name="child">The child to emancipate.</param>
        public void RemoveChild(WWObjectData child)
        {
            if (children.Contains(child))
            {
                children.Remove(child);
            }
        }
    }
}