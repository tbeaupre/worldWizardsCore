using System;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.experimental.controllers
{
    public class CreateObjectTool : Tool
    {
        private bool trackingSwipe = false;
        private Vector2 startPosition;
        
        // Controllers
        private SceneGraphController sceneGraphController;
        
        // Prefabs
        public Collider gridCollider;
        
        // Object Properties
        private WWObject curObject;
        private int curRotation;
        private int curTileIndex;
        
        // Resources
        private List<string> possibleTiles;

        // Raycast Information
        private bool validTarget = false;
        private Vector3 hitPoint;
        
        public override void Init(SteamVR_TrackedController newController)
        {
            base.Init(newController);
            
            listenForTrigger = true;
            listenForGrip = true;
            listenForMenu = true;
            listenForPress = true;
            listenForTouch = true;
            
            sceneGraphController = FindObjectOfType<SceneGraphController>();
            
            ResourceLoader.LoadResources(); // Should be removed at some point.
            possibleTiles = new List<string>(WWResourceController.bundles.Keys);
            
            float tileLengthScale = CoordinateHelper.tileLengthScale;
            gridCollider.transform.localScale = Vector3.one * tileLengthScale;
        }

        public override void Update()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(controller.transform.position, transform.forward, out raycastHit, 100))
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
            
            base.Update();
        }
        
        private WWObject PlaceObject(Vector3 position)
        {
            var coordinate = CoordinateHelper.convertUnityCoordinateToWWCoordinate(position, curRotation);
            var objData = WWObjectFactory.CreateNew(coordinate, possibleTiles[curTileIndex]);
            var gameObj = WWObjectFactory.Instantiate(objData);
            return gameObj;
        }

        
        // Trigger
        public override void OnTriggerUnclick()
        {
            if (validTarget)
            {
                if (curObject != null)
                {
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                    sceneGraphController.Add(curObject);
                    curObject = null;
                }
            }
        }

        protected override void UpdateTrigger()
        {
            if (validTarget)
            {
                if (curObject == null)
                {
                    curObject = PlaceObject(hitPoint);
                }
                else
                {
                    curObject.transform.position = new Vector3(hitPoint.x,
                        hitPoint.y + 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                        hitPoint.z);
                }
            }
        }

        
        // Grip
        public override void OnUngrip()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(controller.transform.position, transform.forward, out raycastHit, 100))
            {
                WWObject wwObject = raycastHit.transform.gameObject.GetComponent<WWObject>();
                if (!wwObject.Equals(curObject))
                {
                    sceneGraphController.Delete(wwObject.GetId());
                }
            }
        }
        
        
        // Touchpad Press
        public override void OnPadUnclick()
        {
            // Rotation
            if (validTarget && curObject != null)
            {
                if (lastPadPos.x < -DEADZONE_SIZE)
                {
                    curRotation += 90;
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
                if (lastPadPos.x > DEADZONE_SIZE)
                {
                    curRotation -= 90;
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
            }

            // Move Grid
            Vector3 gridPosition = gridCollider.transform.position;
            if (lastPadPos.y > DEADZONE_SIZE)
            {
                gridPosition.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            }
            if (lastPadPos.y < -DEADZONE_SIZE)
            {
                gridPosition.y -= CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            }
            gridCollider.transform.position = gridPosition;
        }
        
        
        // Touchpad Touch
        public override void OnPadUntouch()
        {
            trackingSwipe = false;
        }
        
        protected override void UpdateTouch(Vector2 padPos)
        {
            if (curObject != null)
            {
                if (!trackingSwipe)
                {
                    trackingSwipe = true;
                    startPosition = padPos;
                }
                
                var offset = (int)(possibleTiles.Count * CalculateSwipe(padPos.x));
                if (offset != 0)
                {
                    curTileIndex = (curTileIndex + offset) % possibleTiles.Count;
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
            }
        }
        
        private float CalculateSwipe(float x)
        {
            return (x - startPosition.x) / 2;
        }
    }
}