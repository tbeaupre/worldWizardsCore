﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using worldWizards.core.entity.coordinate.utils;
using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.gameObject;
using worldWizards.core.entity.coordinate;
using UnityEngine.UI;

namespace worldWizards.core.experimental{
	public class CreateObjectGunVR : MonoBehaviour {

        public Transform VRCameraRig;

		public Plane groundPlane;
		public Collider gridCollider;
		public Transform markerObject;
		SceneGraphController sceneGraphController;
		List<string> possibleTiles;
		private int curTile = 0;
		private int curRotation = 0;
		public Text coordDebugText;

        private SteamVR_TrackedObject trackedObj;
        public LayerMask teleportMask;

        private WWObject curObject;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int)trackedObj.index); }
        }

        void Awake (){

			Debug.Log ((int) (-1.1f/10f));
			groundPlane = new Plane(Vector3.up, Vector3.up);

			sceneGraphController = FindObjectOfType<SceneGraphController>();
            ResourceLoader.LoadResources();

			possibleTiles = new List<string>(WWResourceController.bundles.Keys); 
			Debug.Log (possibleTiles.Count);

            trackedObj = GetComponent<SteamVR_TrackedObject>();

            gridCollider.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            VRCameraRig = FindObjectOfType<SteamVR_ControllerManager>().transform;
        }


        void Start() {
            StartCoroutine(cycleShit());

                
        }

		void Update() {
			DeleteHitObject ();
			// move the builder grid
			Vector3 gridPosition = gridCollider.transform.position;
			if (Input.GetKeyDown(KeyCode.UpArrow)){
				gridPosition.y += (CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
                Vector3 oldCamPosition = VRCameraRig.position;
                oldCamPosition.y += (CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
                VRCameraRig.position = oldCamPosition;

            }
			else if (Input.GetKeyDown(KeyCode.DownArrow)){
				gridPosition.y -= (CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
                Vector3 oldCamPosition = VRCameraRig.position;
                oldCamPosition.y -= (CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
                VRCameraRig.position = oldCamPosition;
            }
			gridCollider.transform.position = gridPosition;


			//Ray ray = Camera.main.ScreenPointToRay (trackedObj.transform.position);
            //Ray ray = new Ray(trackedObj.transform.position, trackedObj.transform.forward);

            //laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
            RaycastHit raycastHit;

            //    if (gridCollider.Raycast (ray, out raycastHit, 1000f )) {
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out raycastHit, 100, teleportMask)){ 

                Vector3 position = raycastHit.point;
				// because the tile center is in the middle need to offset
				position.x += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
				position.y += CoordinateHelper.baseTileLength *  CoordinateHelper.tileLengthScale;
				position.z += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
				//CycleObjectsScrollWheel (position);
				RotateObjects (position);
				coordDebugText.text = string.Format ("x : {0}, z : {1}", position.x, position.z);
				Debug.DrawRay(raycastHit.point, Camera.main.transform.position, Color.red, 0, false);
				if (curObject == null) {
					curObject = PlaceObject (position);
				} else { // move it
					curObject.transform.position = new Vector3 (raycastHit.point.x,
						raycastHit.point.y + 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
						raycastHit.point.z);
				}
				if (Controller.GetHairTriggerDown()) {
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


        private IEnumerator cycleShit() {
            float delay = 0.15f;
            //if(Input.GetAxis("Mouse ScrollWheel") > 0){;
            Vector3 position = Vector3.zero;
            yield return new WaitForSeconds(delay);
            if (Input.GetAxis("Horizontal") > 0)
            {
                curTile++;
                if (curObject != null)
                {
                    position = curObject.transform.position;
                    Destroy(curObject.gameObject);
                }
                curObject = PlaceObject(position);
            }
            //else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            //{
            else if (Input.GetAxis("Horizontal") < 0)
            {
                curTile--;
                if (curObject != null)
                {
                    position = curObject.transform.position;
                    Destroy(curObject.gameObject);
                }
                curObject = PlaceObject(position);
            }

            StartCoroutine(cycleShit());
        }



		void CycleObjectsScrollWheel(Vector3 position){
            Debug.Log(Input.GetAxis("Horizontal"));
			//if(Input.GetAxis("Mouse ScrollWheel") > 0){
            if (Input.GetAxis("Horizontal") > 0.3f)
            {
                    curTile++;
				if (curObject != null) {
					Destroy (curObject.gameObject);
				}
				curObject = PlaceObject (position);
			}
			//else if(Input.GetAxis("Mouse ScrollWheel") < 0)
            //{
            else if (Input.GetAxis("Horizontal") < -0.3f)
            {
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
			WWObjectData objData = WWObjectFactory.CreateNew(coordinate, possibleTiles[tileIndex]);
			WWObject go = WWObjectFactory.Instantiate(objData);
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