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
        private readonly SwipeGesture swipe = new SwipeGesture();
        
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
        
        protected override void Awake()
        {
            base.Awake();
            trigger = true;
            grip = true;
            appMenu = true;
            press = true;
            touch = true;
            
            sceneGraphController = FindObjectOfType<SceneGraphController>();
            
            ResourceLoader.LoadResources(); // Should be removed at some point.
            possibleTiles = new List<string>(WWResourceController.bundles.Keys);
            
            gridCollider.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
        }

        protected override void Update()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out raycastHit, 100))
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
        protected override void OnTrigger()
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
        protected override void OnGrip()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out raycastHit, 100))
            {
                WWObject wwObject = raycastHit.transform.gameObject.GetComponent<WWObject>();
                if (!wwObject.Equals(curObject))
                {
                    sceneGraphController.Delete(wwObject.GetId());
                }
            }
        }
        
        
        // Touchpad Press
        protected override void OnTouchpadPress(Vector2 pos)
        {
            // Rotation
            if (validTarget && curObject != null)
            {
                if (pos.x < -DEADZONE_SIZE)
                {
                    curRotation += 90;
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
                if (pos.x > DEADZONE_SIZE)
                {
                    curRotation -= 90;
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
            }

            // Move Grid
            Vector3 gridPosition = gridCollider.transform.position;
            if (pos.y > DEADZONE_SIZE)
            {
                gridPosition.y += CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            }
            if (pos.y < -DEADZONE_SIZE)
            {
                gridPosition.y -= CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            }
            gridCollider.transform.position = gridPosition;
        }
        
        
        // Touchpad Touch
        protected override void UpdateTouchpadPress(Vector2 pos)
        {
            if (curObject != null)
            {
                var offset = (int)(possibleTiles.Count * swipe.GetSwipeRatio(Controller));
                if (offset != 0)
                {
                    curTileIndex = (curTileIndex + offset) % possibleTiles.Count;
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
            }
        }
    }
}