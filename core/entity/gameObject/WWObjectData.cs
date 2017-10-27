using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;

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

        public WWObjectData(WWObjectDataMemento m)
        {
            id = m.id;
            //			this.type = m.type;
            //			this.metaData = m.metaData;
            coordinate = m.coordinate;
            resourceTag = m.resourceTag;

            // Note parent and children relationships are re-linked in the SceneGraphController during the Load
            parent = null;
            children = new List<WWObjectData>();
        }

        public Guid id { get; }

        //		public WWType type { get; }
        //		public MetaData metaData { get;}

        public Coordinate coordinate { get; }

        public WWObjectData parent { get; set; }
        public List<WWObjectData> children { get; }


        public string resourceTag { get; }


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

    [Serializable]
    public class WWObjectDataMemento
    {
        public List<Guid> children;

        //		public MetaData metaData;
        public Coordinate coordinate;

        public Guid id;
        public Guid parent;
        public string resourceTag;
        public WWType type;

        public WWObjectDataMemento(WWObjectData state)
        {
            id = state.id;
            //			this.type = state.type;
            //			this.metaData = state.metaData;
            coordinate = state.coordinate;
            resourceTag = state.resourceTag;
            if (state.parent != null) parent = state.parent.id;
            children = new List<Guid>();
            foreach (var child in state.children) children.Add(child.id);
        }

        [JsonConstructor]
        public WWObjectDataMemento()
        {
        }
    }
}