using System;
using System.Collections.Generic;
using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

using Newtonsoft.Json;

namespace worldWizards.core.entity.gameObject
{
    public class WWObjectData
    {
        public Guid id { get; }

		public WWType type { get; }
		public MetaData metaData { get;}

        public Coordinate coordinate { get; }

		public WWObjectData parent { get; set;}
		public List<WWObjectData> children { get; }
	

        public string resourceTag { get; }

 
		public WWObjectData(Guid id, MetaData metaData, Coordinate coordinate,
			WWObjectData parent, List<WWObjectData> children, string resourceTag)
		{
            this.id = id;
            this.metaData = metaData;
            this.coordinate = coordinate;
            this.parent = parent;
            this.children = children;
            this.resourceTag = resourceTag;
        }

		public WWObjectData(WWObjectDataMemento m)
		{
			this.id = m.id;
			this.type = m.type;
			this.metaData = m.metaData;
			this.coordinate = m.coordinate;
//			this.resource = m.resource;

			// Note parent and children relationships are re-linked in the SceneGraphController during the Load
			this.parent = null;
			this.children = new List<WWObjectData>();
		}
			

		public void AddChildren(List<WWObject> children) {
			foreach (WWObject child in children) {
				this.children.Add (child.objectData);
			}
		}


		/// <summary>
		/// Gets all descendents.
		/// </summary>
		/// <returns>The all descendents.</returns>
		public List<WWObjectData> GetAllDescendents()
		{
			List<WWObjectData> descendents = new List<WWObjectData> ();
			foreach(WWObjectData child in this.children){
				descendents.Add (child);
				List<WWObjectData> childsDescendents = child.GetAllDescendents ();
				foreach (WWObjectData childsDescendent in childsDescendents) {
					descendents.Add (childsDescendent);
				}
			}
			return descendents;
		}


//		public void Parent(WWObject parent){
//			this.parent = parent.objectData.id;
//		}


    }

	[Serializable]
	public class WWObjectDataMemento{
		public Guid id;
		public WWType type;
		public MetaData metaData;
		public Coordinate coordinate;
		public WWResource resource;
		public Guid parent;
		public List<Guid> children;

		public WWObjectDataMemento(WWObjectData state){
			this.id = state.id;
			this.type = state.type;
			this.metaData = state.metaData;
			this.coordinate = state.coordinate;
//			this.resource = state.resource;
			if (state.parent != null) { // can be null if no parent
				this.parent = state.parent.id;
			}
			this.children = new List<Guid> ();
			foreach (var child in state.children) {
				this.children.Add (child.id);
			}
		}
			
		[JsonConstructor]
		public WWObjectDataMemento(){
		}

	}


}
