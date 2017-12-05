using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.level.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    internal enum State
    {
        Normal,
        DoorAttach,
        PerimeterWalls
    }

    public class CreateObjectGun : MonoBehaviour
    {
        public Text coordDebugText;

        private WWObject curObject;
        private int curRotation;
        private int curTile;
        public Collider gridCollider;

        public Plane groundPlane;
        public Transform markerObject;
        private bool placeDoorState = false;

        private bool placeState = true;
        private List<string> possibleTiles;
        private State state = State.Normal;

        private void Awake()
        {
            // Need to make sure Manager registry is initialized first
            groundPlane = new Plane(Vector3.up, Vector3.up);
//            ResourceLoader.LoadResources();
//            foreach (string s in ResourceLoader.FindAssetBundlePaths()) Debug.Log(s);

            possibleTiles = WWResourceController.GetResourceKeysByAssetBundle("ww_basic_assets");
            Debug.Log(possibleTiles.Count);
        }


        private void BuildWallsInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    var wwObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (wwObject != null)
                    {
                        if (!wwObject.Equals(curObject))
                        {
                            BuilderAlgorithms.BuildPerimeterWalls(GetResourceTag(), wwObject);
                        }
                    }
                }
            }
        }


        private void TryPlaceDoor(Vector3 hitPoint)
        {
            Debug.Log("TryPlaceDoor called.");
            Coordinate coord = CoordinateHelper.ConvertUnityCoordinateToWWCoordinate(hitPoint);
            List<WWObject> objects = ManagerRegistry.Instance.sceneGraphManager.GetObjectsInCoordinateIndex(coord);
            Debug.Log("objects count " + objects.Count);

            foreach (WWObject obj in objects)
            {
                Debug.Log(" object type " + obj.resourceMetaData.wwObjectMetaData.type);
                if (obj.resourceMetaData.wwObjectMetaData.type == WWType.Tile)
                {
                    Debug.Log("A tile was in the coordinate");
                    if (curObject.resourceMetaData.wwObjectMetaData.type == WWType.Door)
                    {
                        Debug.Log("The current Object is a door");
                        if (ManagerRegistry.Instance.sceneGraphManager.AddDoor((Door) curObject, (Tile) obj, hitPoint))
                        {
                            curObject = null;
                        }
                    }
                }
            }
        }

        private void CheckForStateChange()
        {
            EnterNormalState();
            EnterDoorAttachState();
            EnterPerimeterState();
        }

        private void EnterNormalState()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                state = State.Normal;
            }
        }

        private void EnterDoorAttachState()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                state = State.DoorAttach;
            }
        }

        private void EnterPerimeterState()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                state = State.PerimeterWalls;
            }
        }

        private void Update()
        {
            CheckForStateChange();
            RotateObjects();


            if (state == State.PerimeterWalls)
            {
                BuildWallsInput();
                return;
            }
            DeleteHitObject();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (gridCollider.Raycast(ray, out raycastHit, 1000f))
            {
                Vector3 position = raycastHit.point;
                // because the tile center is in the middle need to offset
                position.x += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                position.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                position.z += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                CycleObjectsScrollWheel(position);
                coordDebugText.text = string.Format("x : {0}, z : {1}", position.x, position.z);
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
                if (Input.GetMouseButtonUp(0))
                {
                    if (curObject != null)
                    {
                        Destroy(curObject.gameObject);
                        curObject = PlaceObject(position);

                        if (state == State.DoorAttach)
                        {
                            TryPlaceDoor(position);
                        }
                        else if (state == State.Normal)
                        {
                            if (ManagerRegistry.Instance.sceneGraphManager.Add(curObject))
                            {
                                curObject = null;
                            }
                        }
                    }
                }
            }
        }

        private void RotateObjects()
        {
            if (curObject == null)
            {
                return;
            }

            Vector3 curPos = curObject.transform.position;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("Rotate left");
                curRotation += 90;
                curRotation = curRotation % 360 + (curRotation < 0 ? 360 : 0);
                Destroy(curObject.gameObject);
                curObject = ForceRotateAndPlaceObject(curPos);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Rotate right");
                curRotation -= 90;
                curRotation = curRotation % 360 + (curRotation < 0 ? 360 : 0);
                Destroy(curObject.gameObject);
                curObject = ForceRotateAndPlaceObject(curPos);
            }
        }

        private void CycleObjectsScrollWheel(Vector3 position)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                curTile++;
                if (curObject != null)
                {
                    Destroy(curObject.gameObject);
                }
                curObject = PlaceObject(position);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                curTile--;
                if (curObject != null)
                {
                    Destroy(curObject.gameObject);
                }
                curObject = PlaceObject(position);
            }
        }

        private string GetResourceTag()
        {
            int tileIndex = Mathf.Abs(curTile) % possibleTiles.Count;
            return possibleTiles[tileIndex];
        }

        private WWObject ForceRotateAndPlaceObject(Vector3 position)
        {
            int theRot = curRotation;
            Coordinate coordRotated = CoordinateHelper.ConvertUnityCoordinateToWWCoordinate(position, theRot);
            WWObjectData objData = WWObjectFactory.CreateNew(coordRotated, GetResourceTag());
            WWObject go = WWObjectFactory.Instantiate(objData);
            return go;
        }

        private WWObject PlaceObject(Vector3 position)
        {
            List<int> possibleConfigurations =
                BuilderAlgorithms.GetPossibleRotations(position, GetResourceTag());

            if (possibleConfigurations.Count == 0)
            {
                return null;
            }
            int theRot = possibleConfigurations[0];
            if (possibleConfigurations.Contains(curRotation))
            {
                theRot = curRotation;
            }
            Coordinate coordRotated = CoordinateHelper.ConvertUnityCoordinateToWWCoordinate(position, theRot);
            WWObjectData objData = WWObjectFactory.CreateNew(coordRotated, GetResourceTag());
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