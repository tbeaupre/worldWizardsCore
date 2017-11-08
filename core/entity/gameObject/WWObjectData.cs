using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WorldWizards.core.entity.common;
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
            //			this.type = type;
            //            this.metaData = metaData;
            this.coordinate = coordinate;
            this.parent = parent;
            this.children = children;
            this.resourceTag = resourceTag;
        }

        public WWObjectData(WWObjectJSONBlob b)
        {
            id = b.id;
            //			this.type = m.type;
            //			this.metaData = m.metaData;
            coordinate = new Coordinate(b.coordinate);
            resourceTag = b.resourceTag;

            // Note parent and children relationships are re-linked in the SceneGraphController during the Load
            parent = null;
            children = new List<WWObjectData>();
        }

        public Guid id { get; private set; }

        //		public WWType type { get; }
        //		public MetaData metaData { get;}

        public Coordinate coordinate { get; private set; }

        public WWObjectData parent { get; set; }
        public List<WWObjectData> children { get; private set; }


        public string resourceTag { get; private set; }


        public void AddChildren(List<WWObject> children)
        {
            foreach (var child in children) this.children.Add(child.objectData);
        }

	    /// <summary>
	    ///     Gets all descendents.
	    /// </summary>
	    /// <returns>The all descendents.</returns>
	    public List<WWObjectData> GetAllDescendents()
        {
            var descendents = new List<WWObjectData>();
            foreach (var child in children)
            {
                descendents.Add(child);
                var childsDescendents = child.GetAllDescendents();
                foreach (var childsDescendent in childsDescendents) descendents.Add(childsDescendent);
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
            if (children.Contains(child)) children.Remove(child);
        }
    }
}