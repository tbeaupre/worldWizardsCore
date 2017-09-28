using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using worldWizards.core.entity.coordinate.utils;
using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.common;
using UnityEngine.UI;

namespace worldWizards.core.experimental{
	public class CreateObjectGun : MonoBehaviour {
		
		public Plane groundPlane;
		public Collider gridCollider;
		public Transform markerObject;
		SceneGraphController sceneGraphController;
		List<string> possibleTiles;
		private int curTile = 0;
		private int curRotation = 0;
		public Text coordDebugText;

		private WWObject curObject;

		void Awake (){

			Debug.Log ((int) (-1.1f/10f));
			groundPlane = new Plane(Vector3.up, Vector3.up);

			sceneGraphController = FindObjectOfType<SceneGraphController>();
            ResourceLoader.LoadResources();

			possibleTiles = new List<string>(WWResourceController.bundles.Keys); 
			Debug.Log (possibleTiles.Count);
		}


		void Update() {
			DeleteHitObject ();
			// move the builder grid
			Vector3 gridPosition = gridCollider.transform.position;
			if (Input.GetKeyDown(KeyCode.UpArrow)){
				gridPosition.y += (CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow)){
				gridPosition.y -= (CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
			}
			gridCollider.transform.position = gridPosition;


			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit raycastHit;

			if (gridCollider.Raycast (ray, out raycastHit, 1000f )) {
				Vector3 position = raycastHit.point;
				// because the tile center is in the middle need to offset
				position.x += .5f * CoordinateHelper.baseTileLength;
				position.y += CoordinateHelper.baseTileLength;
				position.z += .5f * CoordinateHelper.baseTileLength;
				CycleObjectsScrollWheel (position);
				RotateObjects (position);
				coordDebugText.text = string.Format ("x : {0}, z : {1}", position.x, position.z);
				Debug.DrawRay(raycastHit.point, Camera.main.transform.position, Color.red, 0, false);
				if (curObject == null) {
					curObject = PlaceObject (position);
				} else { // move it
					curObject.transform.position = new Vector3 (raycastHit.point.x,
						raycastHit.point.y + 0.5f * CoordinateHelper.baseTileLength,
						raycastHit.point.z);
				}
				if (Input.GetMouseButtonUp (0)) {
					if (curObject != null) {
						Destroy (curObject.gameObject);
						curObject = PlaceObject (position);
						sceneGraphController.Add (curObject);
						curObject = null;
					}
				}
			}
		}
			
		void RotateObjects(Vector3 position){
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				curRotation += 90;
				if (curObject != null) {
					Destroy (curObject.gameObject);
				}
				curObject = PlaceObject (position);
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)){
				curRotation -= 90;
				if (curObject != null) {
					Destroy (curObject.gameObject);
				}
				curObject = PlaceObject (position);
			}   
		}

		void CycleObjectsScrollWheel(Vector3 position){
			if(Input.GetAxis("Mouse ScrollWheel") > 0){
				curTile++;
				if (curObject != null) {
					Destroy (curObject.gameObject);
				}
				curObject = PlaceObject (position);
			}
			else if(Input.GetAxis("Mouse ScrollWheel") < 0){
				curTile--;
				if (curObject != null) {
					Destroy (curObject.gameObject);
				}
				curObject = PlaceObject (position);
			}   
		}
			
		WWObject PlaceObject(Vector3 position){



			int tileIndex = Mathf.Abs(curTile) % possibleTiles.Count;
		


			Coordinate coordinate = CoordinateHelper.convertUnityCoordinateToWWCoordinate(position, curRotation);
			WWObjectData objData = WWObjectFactory.MockCreate(coordinate, possibleTiles[tileIndex]);
			WWObject go = WWObjectFactory.Instantiate(objData);




//			Debug.Log (curObject.objectData.metaData.GetType ());
//			Debug.Log (curObject.objectData.GetType());
		
			return go;
		}


		void DeleteHitObject(){
			if (Input.GetKeyDown(KeyCode.Mouse1)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit)) {
					WWObject wwObject = hit.transform.gameObject.GetComponent<WWObject> ();
					if (!wwObject.Equals(curObject)) {
						sceneGraphController.Delete (wwObject.GetId());
					}
				}
			}
		}
			
	}

}