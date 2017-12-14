using System;
using System.Collections.Generic;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.file.entity;

namespace WorldWizards.core.entity.gameObject
{
    public class WWObjectData
    {
        public WWObjectData(Guid id, Coordinate coordinate,
            WWObjectData parent, List<WWObjectData> children, string resourceTag)
        {
            this.id = id;
            this.coordinate = coordinate;
            this.parent = parent;
            this.children = children;
            this.resourceTag = resourceTag;
        }

        public WWObjectData(WWObjectJSONBlob b)
        {
            id = b.id;
            coordinate = new Coordinate(b.coordinate);
            resourceTag = b.resourceTag;

            // Note parent and children relationships are re-linked in the SceneGraphController during the Load
            parent = null;
            children = new List<WWObjectData>();
        }

        public Guid id { get; private set; }

        public Coordinate coordinate { get; set; }

        public WWObjectData parent { get; set; }
        public List<WWObjectData> children { get; private set; }

        public string resourceTag { get; private set; }


        public void AddChildren(List<WWObject> children)
        {
            foreach (WWObject child in children) this.children.Add(child.objectData);
        }

        /// <summary>
        ///     Gets all descendents.
        /// </summary>
        /// <returns>The all descendents.</returns>
        public List<WWObjectData> GetAllDescendents()
        {
            var descendents = new List<WWObjectData>();
            foreach (WWObjectData child in children)
            {
                descendents.Add(child);
                List<WWObjectData> childsDescendents = child.GetAllDescendents();
                foreach (WWObjectData childsDescendent in childsDescendents) descendents.Add(childsDescendent);
            }
            return descendents;
        }

        public void Unparent()
        {
            if (parent != null)
            {
                parent.RemoveChild(this);
                parent = null;
            }
        }

        public void Parent(WWObjectData parent)
        {
            this.parent = parent;
        }


        public void RemoveChild(WWObjectData child)
        {
            if (children.Contains(child))
            {
                children.Remove(child);
            }
        }
    }
}