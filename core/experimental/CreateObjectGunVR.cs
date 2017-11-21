using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    public class CreateObjectGunVR : MonoBehaviour
    {
        public Text coordDebugText;

        private WWObject curObject;
        private int curRotation;
        private int curTile;
        public Collider gridCollider;

        public Plane groundPlane;
        public Transform markerObject;
        private List<string> possibleTiles;
        public LayerMask teleportMask;

        private SteamVR_TrackedObject trackedObj;

        public Transform VRCameraRig;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) trackedObj.index); }
        }

        private void Awake()
        {
            groundPlane = new Plane(Vector3.up, Vector3.up);

            ResourceLoader.LoadResources();

            possibleTiles = new List<string>(WWResourceController.bundles.Keys);
            Debug.Log(possibleTiles.Count);

            trackedObj = GetComponent<SteamVR_TrackedObject>();

            gridCollider.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            VRCameraRig = FindObjectOfType<SteamVR_ControllerManager>().transform;
        }

        private void Update()
        {
            DeleteHitObject();
            // move the builder grid
            Vector3 gridPosition = gridCollider.transform.position;
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                gridPosition.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                Vector3 oldCamPosition = VRCameraRig.position;
                oldCamPosition.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                VRCameraRig.position = oldCamPosition;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                gridPosition.y -= CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                Vector3 oldCamPosition = VRCameraRig.position;
                oldCamPosition.y -= CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                VRCameraRig.position = oldCamPosition;
            }
            gridCollider.transform.position = gridPosition;


            //Ray ray = Camera.main.ScreenPointToRay (trackedObj.transform.position);
            //Ray ray = new Ray(trackedObj.transform.position, trackedObj.transform.forward);

            //laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
            RaycastHit raycastHit;

            //    if (gridCollider.Raycast (ray, out raycastHit, 1000f )) {
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out raycastHit, 100, teleportMask))
            {
                Vector3 position = raycastHit.point;
                // because the tile center is in the middle need to offset
                position.x += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                position.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                position.z += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                coordDebugText.text = string.Format("x : {0}, z : {1}", position.x, position.z);
                
                CycleObjectsSwipe (position);
                RotateObjects(position);
                
                Debug.DrawRay(raycastHit.point, Camera.main.transform.position, Color.red, 0, false);
                if (curObject == null)
                {
                    curObject = PlaceObject(position);
                }
                else
                {
                    curObject.transform.position = new Vector3(raycastHit.point.x,
                        raycastHit.point.y + 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                        raycastHit.point.z);
                }
                if (Controller.GetHairTriggerDown())
                {
                    if (curObject != null)
                    {
                        Destroy(curObject.gameObject);
                        curObject = PlaceObject(position);
                        ManagerRegistry.Instance.sceneGraphManager.Add(curObject);
                        curObject = null;
                    }
                }
            }
        }

        private void RotateObjects(Vector3 position)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                curRotation += 90;
                if (curObject != null)
                {
                    Destroy(curObject.gameObject);
                }
                curObject = PlaceObject(position);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                curRotation -= 90;
                if (curObject != null)
                {
                    Destroy(curObject.gameObject);
                }
                curObject = PlaceObject(position);
            }
        }

        private void CycleObjectsSwipe(Vector3 position)
        {
            var offset = (int)(possibleTiles.Count * 0.3f);
            if (offset != 0)
            {
                curTile = (curTile + offset) % possibleTiles.Count;
                if (curObject != null) Destroy(curObject.gameObject);
                curObject = PlaceObject(position);
            }
        }

        private WWObject PlaceObject(Vector3 position)
        {
            int tileIndex = Mathf.Abs(curTile) % possibleTiles.Count;
            Coordinate coordinate = CoordinateHelper.convertUnityCoordinateToWWCoordinate(position, curRotation);
            WWObjectData objData = WWObjectFactory.CreateNew(coordinate, possibleTiles[tileIndex]);
            WWObject go = WWObjectFactory.Instantiate(objData);
            return go;
        }

        private void DeleteHitObject()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    var wwObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (!wwObject.Equals(curObject))
                    {
                        ManagerRegistry.Instance.sceneGraphManager.Delete(wwObject.GetId());
                    }
                }
            }
        }
    }
}