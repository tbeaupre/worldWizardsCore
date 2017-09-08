using UnityEngine;
using System;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    /// <summary>
    /// The WorldWizardsObject is base class for all World Wizards objects.
    /// WorldWizardsObject extends MonoBehavior so it has to be attached to a GameObject.
    /// </summary>
    public abstract class WWObject : MonoBehaviour
    {
		public WWObjectData objectData { get; private set;}

        public virtual void Init (Guid id, MetaData metaData, Coordinate coordinate,
            WWObjectData parent, List<WWObjectData> children, string resourceTag)
        {
            this.objectData = new WWObjectData(id, metaData, coordinate, parent, children, resourceTag);
        }

        public virtual void Init(WWObjectData objectData)
        {
            this.objectData = objectData;
        }

        public Guid GetId() {
            return objectData.id;
        }

        public Coordinate GetCoordinate() {
            return objectData.coordinate;
        }

        public void SetCoordinate(Coordinate coordinate) {
        }

        public WWObject GetOldestParent() {
            return null;
        }

        /// <summary>
        /// Promote this World Wizard Object to not have a parent.
        /// </summary>
        public void Unparent() {
        }


		public void Parent(WWObject parent){
		
		}

        public void RemoveChildren(List<WWObject> children) {
        }

        public void AddChildren(List<WWObject> children) {
			this.objectData.AddChildren (children);
//			foreach (var child in children){
//				child.Parent (this);
//			}
        }

        public List<WWObject> GetChildren() {
            return null;// children;
        }

        public WWObject GetParent() {
            return null;// parent;
        }

        public List<WWObjectData> GetAllDescendents()
        {
			return objectData.GetAllDescendents ();
        }

    }
}
