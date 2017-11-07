using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.experimental
{
    public class CreateObjectGun : MonoBehaviour
    {
        public Text coordDebugText;

        private WWObject curObject;
        private int curRotation;
        private int curTile;
        public Collider gridCollider;

        public Plane groundPlane;
        public Transform markerObject;
        private List<string> possibleTiles;
        private SceneGraphController sceneGraphController;

        private bool placeState = true;

        private void Awake()
        {
            groundPlane = new Plane(Vector3.up, Vector3.up);

            sceneGraphController = FindObjectOfType<SceneGraphController>();
            ResourceLoader.LoadResources();

            foreach (var s in ResourceLoader.FindAssetBundlePaths()) Debug.Log(s);

            possibleTiles = WWResourceController.GetResourceKeysByAssetBundle("ww_basic_assets");
            Debug.Log(possibleTiles.Count);
            
            
            gridCollider.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

        }

        private int GetWallIndex()
        {
            for (int i = 0; i < possibleTiles.Count; i++)
            {
                if (possibleTiles[i].Equals("ww_basic_assets_tile_wallbrick")){
//                if (possibleTiles[i].Equals("ww_basic_assets_tile_arch")){
                    return i;
                }
            }
            return 0;
        }

        
        private void PlaceWallObject(IntVector3 coordIndex, int rotation)
        {
            var tileIndex = GetWallIndex();
            var coordinate = new Coordinate(coordIndex, rotation);
            var objData = WWObjectFactory.CreateNew(coordinate, possibleTiles[tileIndex]);
            var go = WWObjectFactory.Instantiate(objData);
            if (!sceneGraphController.Add(go))
            {
                Debug.Log("Could not place wall because of collision, deleting temp");
                Destroy(go.gameObject);
            }
        }

        private void BuildWallsInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Input.GetKey(KeyCode.LeftShift))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    var wwObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (wwObject != null)
                    {
                        if (!wwObject.Equals(curObject)) BuildWalls(wwObject);
                    }
                }
            }
        }
        

        private void BuildWalls(WWObject wwObject)
        {
            Debug.Log("Execute Build Walls");
            var walls = sceneGraphController.SelectPerimeter(wwObject);
            Debug.Log("Found " + walls.Count + " to build.");
            foreach (var wall in walls)
            {
                Debug.Log(string.Format(" build a wall at inded : {0} and diretions {1}", wall.Key, wall.Value));
                
                if (Convert.ToBoolean(WWWalls.North & wall.Value))
                {
                    Debug.Log("North Wall detected");
                    PlaceWallObject(wall.Key, 0);
                }
                if (Convert.ToBoolean(WWWalls.East & wall.Value))
                {
                    Debug.Log("East Wall detected");    
                    PlaceWallObject(wall.Key, 90);
                }
                if (Convert.ToBoolean(WWWalls.South & wall.Value))
                {
                    Debug.Log("South Wall detected"); 
                    PlaceWallObject(wall.Key, 180);
                }
                if (Convert.ToBoolean(WWWalls.West & wall.Value))
                {
                    Debug.Log("West Wall detected");  
                    PlaceWallObject(wall.Key, 270);
                }



            }
        }

        private void TogglePlaceState()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                placeState = !placeState;
            }
        }

        private void Update()
        {
            TogglePlaceState();
            if (!placeState)
            {
                BuildWallsInput();
                return;
            }
            
            
            DeleteHitObject();
            // move the builder grid
            var gridPosition = gridCollider.transform.position;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                gridPosition.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                gridPosition.y -= CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            gridCollider.transform.position = gridPosition;


            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            if (gridCollider.Raycast(ray, out raycastHit, 1000f))
            {
                var position = raycastHit.point;
                // because the tile center is in the middle need to offset
                position.x += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                position.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                position.z += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                CycleObjectsScrollWheel(position);
                RotateObjects(position);
                coordDebugText.text = string.Format("x : {0}, z : {1}", position.x, position.z);
                Debug.DrawRay(raycastHit.point, Camera.main.transform.position, Color.red, 0, false);
                if (curObject == null) curObject = PlaceObject(position);
                else
                    curObject.transform.position = new Vector3(raycastHit.point.x,
                        raycastHit.point.y + 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                        raycastHit.point.z);
                if (Input.GetMouseButtonUp(0))
                    if (curObject != null)
                    {
                        Destroy(curObject.gameObject);
                        curObject = PlaceObject(position);
                        if (sceneGraphController.Add(curObject))
                        {
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
                if (curObject != null) Destroy(curObject.gameObject);
                curObject = PlaceObject(position);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                curRotation -= 90;
                if (curObject != null) Destroy(curObject.gameObject);
                curObject = PlaceObject(position);
            }
        }

        private void CycleObjectsScrollWheel(Vector3 position)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                curTile++;
                if (curObject != null) Destroy(curObject.gameObject);
                curObject = PlaceObject(position);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                curTile--;
                if (curObject != null) Destroy(curObject.gameObject);
                curObject = PlaceObject(position);
            }
        }

        private WWObject PlaceObject(Vector3 position)
        {
            var tileIndex = Mathf.Abs(curTile) % possibleTiles.Count;


            var coordinate = CoordinateHelper.convertUnityCoordinateToWWCoordinate(position, curRotation);
            var objData = WWObjectFactory.CreateNew(coordinate, possibleTiles[tileIndex]);
            var go = WWObjectFactory.Instantiate(objData);


            //			Debug.Log (curObject.objectData.metaData.GetType ());
            //			Debug.Log (curObject.objectData.GetType());

            return go;
        }


        private void DeleteHitObject()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    var wwObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (!wwObject.Equals(curObject)) sceneGraphController.Delete(wwObject.GetId());
                }
            }
        }
    }
}