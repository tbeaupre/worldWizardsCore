using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace worldWizards.core.input.Tools
{
    public class CreateObjectTool : Tool
    {
        // Prefabs
        public Collider gridCollider;
        
        // Resources
        private string currentAssetBundle;
        private List<string> possibleTiles;
        
        // Object Properties
        private WWObject curObject;
        private int curRotation;
        private int curTileIndex;

        // Raycast Information
        private bool validTarget = false;
        private Vector3 hitPoint;
        
        // Swipe
        private bool trackingSwipe = false;
        private Vector2 swipeStartPosition;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            gridCollider = FindObjectOfType<MeshCollider>();
            gridCollider.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            
            currentAssetBundle = "ww_basic_assets";
            possibleTiles = WWResourceController.GetResourceKeysByAssetBundle(currentAssetBundle);
            Debug.Log("CreateObjectTool::Init(): " + possibleTiles.Count + " Assets Loaded.");
            
            curTileIndex = 0;
            curRotation = 0;
        }

        public void Update()
        {
            Ray ray = new Ray(input.GetControllerPoint(), input.GetControllerDirection());
            RaycastHit raycastHit;
            if (gridCollider.Raycast(ray, out raycastHit, 100))
            {
                validTarget = true;
                hitPoint = raycastHit.point;
                
                // because the tile center is in the middle need to offset
                hitPoint.x += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                hitPoint.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
                hitPoint.z += .5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            }
            else
            {
                validTarget = false;
            }
        }
        
        private void CreateObject(Vector3 position)
        {
            Coordinate coordinate = CoordinateHelper.convertUnityCoordinateToWWCoordinate(position, curRotation);
            WWObjectData objData = WWObjectFactory.CreateNew(coordinate, possibleTiles[curTileIndex]);
            WWObject gameObj = WWObjectFactory.Instantiate(objData);
            curObject = gameObj;
        }

        private void ReplaceObject(Vector3 position)
        {
            if (curObject != null)
            {
                Destroy(curObject.gameObject);
            }
            CreateObject(position);
        }

        
        // Trigger
        public override void OnTriggerUnclick() // Add the object to the SceneGraph.
        {
            if (validTarget)
            {
                if (curObject != null)
                {
                    ReplaceObject(hitPoint);
                    if (!ManagerRegistry.Instance.sceneGraphManager.Add(curObject))
                    {
                        Destroy(curObject); // If the object collided with another, destroy it.
                    }
                    curObject = null;
                }
            }
        }

        public override void UpdateTrigger() // Move the object to where the controller is pointed.
        {
            if (validTarget)
            {
                if (curObject == null)
                {
                    CreateObject(hitPoint);
                }
                else
                {
                    curObject.transform.position = new Vector3(
                        hitPoint.x - 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                        hitPoint.y - 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                        hitPoint.z - 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale);
                }
            }
        }

        
        // Grip
        public override void OnUngrip() // Delete object
        {
            if (curObject == null)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(input.GetControllerPoint(), input.GetControllerDirection(), out raycastHit, 100))
                {
                    WWObject wwObject = raycastHit.transform.gameObject.GetComponent<WWObject>();
                    if (wwObject != null)
                    {
                        ManagerRegistry.Instance.sceneGraphManager.Delete(wwObject.GetId());
                    }
                }
            }
        }
        
        
        // Touchpad Press
        public override void OnPadUnclick(Vector2 lastPadPos)
        {
            // Rotation
            if (validTarget && curObject != null)
            {
                if (lastPadPos.x < -DEADZONE_SIZE)
                {
                    curRotation -= 90;
                    ReplaceObject(hitPoint);
                }
                if (lastPadPos.x > DEADZONE_SIZE)
                {
                    curRotation += 90;
                    ReplaceObject(hitPoint);
                }
            }

            // Move Grid
            if (lastPadPos.y > DEADZONE_SIZE)
            {
                gridCollider.transform.position += new Vector3(0, CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale, 0);
            }
            if (lastPadPos.y < -DEADZONE_SIZE)
            {
                gridCollider.transform.position -= new Vector3(0, CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale, 0);
            }
        }
        
        
        // Touchpad Touch
        public override void OnPadUntouch(Vector2 lastPadPos)
        {
            trackingSwipe = false;

            // Check for presses on the top or bottom of the pad.
            if (Math.Abs(lastPadPos.x) < DEADZONE_SIZE / 2)
            {
                if (lastPadPos.y > DEADZONE_SIZE)
                {
                    curTileIndex = (curTileIndex + 1) % possibleTiles.Count;
                    ReplaceObject(hitPoint);
                }
                if (lastPadPos.y < -DEADZONE_SIZE)
                {
                    curTileIndex = (curTileIndex - 1 + possibleTiles.Count) % possibleTiles.Count;
                    ReplaceObject(hitPoint);
                }
            }
        }
        
        public override void UpdateTouch(Vector2 padPos) // Swipe to change objects.
        {
            if (curObject != null) // Only swipe if there is currently an object in 'hand'.
            {
                if (!trackingSwipe)
                {
                    trackingSwipe = true;
                    swipeStartPosition = padPos;
                }
                
                var offset = (int)(possibleTiles.Count * CalculateSwipe(padPos.x));
                if (offset != 0)
                {
                    swipeStartPosition = padPos;
                    curTileIndex = (curTileIndex + offset + possibleTiles.Count) % possibleTiles.Count;
                    ReplaceObject(hitPoint);
                }
            }
        }
        
        private float CalculateSwipe(float x)
        {
            return (x - swipeStartPosition.x) / 5;
        }
    }
}