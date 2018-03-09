using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.controller.builder;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.entity.gameObject.utils;
using WorldWizards.core.entity.level.utils;
using WorldWizards.core.input.Tools.utils;
using WorldWizards.core.input.VRControls;
using WorldWizards.core.manager;
using WorldWizards.SteamVR.Scripts;

namespace WorldWizards.core.input.Tools
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    public class CreateObjectTool : Tool
    {
        // Prefabs
        private static GridController gridController;
        
        // Object Properties
        private WWObject curObject;
        private int curRotation;
        private int curTileIndex;

        // Raycast Information
        private Vector3 hitPoint;
        
        // Swipe
        private bool trackingSwipe = false;
        private Vector2 swipeStartPosition;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            Debug.Log("Create Object Tool");

            gridController = FindObjectOfType<GridController>();

            curTileIndex = 0;
            curRotation = 0;
        }

        public void Update()
        {
            hitPoint = ToolUtilities.RaycastGridOnly(input.GetControllerPoint(),
                input.GetControllerDirection(), gridController.GetGridCollider(), 200);  
            
            // if we are placing a prop
            if (curObject != null && curObject.ResourceMetadata.wwObjectMetadata.type == WWType.Prop)
            { // raycast against all tiles and ignore the grid
                hitPoint = ToolUtilities.RaycastGridThenCustom(input.GetControllerPoint(),
                    input.GetControllerDirection(), gridController.GetGridCollider(), WWType.Tile, 200);
            }
        }
         
        private void CreateObject(Vector3 position)
        {
            if (curObject != null)
            {
                Destroy(curObject.gameObject);
            }

            Coordinate coordinate = CoordinateHelper.UnityCoordToWWCoord(position);
            var wwTransform = new WWTransform(coordinate, curRotation);
            if (ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetPossibleObjectKeys().Count > 0)
            {
                WWObjectData objData = WWObjectFactory.CreateNew(wwTransform,
                    ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetPossibleObjectKeys()[curTileIndex]);
                curObject = WWObjectFactory.Instantiate(objData);
            }
            
        }  
        
        private void ReplaceObject(Vector3 position)
        {
            if (curObject != null)
            {
                Destroy(curObject.gameObject);
            }
            CreateObject(position);
        }

        void MoveObject()
        {
            var position = hitPoint;          
            if (curObject.ResourceMetadata.wwObjectMetadata.type == WWType.Tile)
            {
                position.y += 0.5f * CoordinateHelper.GetTileScale();
            }
            curObject.SetPosition(position);
        }


        public void PlaceObject()
        {
            if (curObject != null)
            {
                MoveObject();
                curObject.SetPosition(CoordinateHelper.UnityCoordToWWCoord(curObject.transform.position));
                if (!ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(curObject))
                {
                    Destroy(curObject.gameObject); // If the object collided with another, destroy it.
                }
                curObject = null;
            }
        }

        // Trigger
        public override void OnTriggerUnclick() // Add the object to the SceneGraph.
        {
            PlaceObject();
            onUpdatTriggerDown = false;
        }

        private bool onUpdatTriggerDown;
        public override void UpdateTrigger() // Move the object to where the controller is pointed.
        {
            if (!onUpdatTriggerDown)
            {
                onUpdatTriggerDown = true;
                CreateObject(hitPoint);
            }
            MoveObject();
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
                        ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Delete(wwObject.GetId());
                    }
                }
            }
        }
        
        // Touchpad Press
        public override void OnPadUnclick(Vector2 lastPadPos)
        {
            // Rotation
            if (curObject != null)
            {
                if (lastPadPos.x < -DEADZONE_SIZE)
                {
                    curRotation -= 90;
                    curObject.SetRotation(curRotation);
                }
                if (lastPadPos.x > DEADZONE_SIZE)
                {
                    curRotation += 90;
                    curObject.SetRotation(curRotation);
                }
            }

            // Move Grid
            if (lastPadPos.y > DEADZONE_SIZE)
            {
                gridController.StepUp();
            }
            if (lastPadPos.y < -DEADZONE_SIZE)
            {
                gridController.StepDown();
            }
        }
        
        
        // Touchpad Touch
        public override void OnPadUntouch(Vector2 lastPadPos)
        {
            trackingSwipe = false;
            var availableObjects = ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetPossibleObjectKeys();

            // Check for presses on the top or bottom of the pad.
            if (Math.Abs(lastPadPos.x) < DEADZONE_SIZE / 2 && availableObjects.Count > 0)
            {
                if (lastPadPos.y > DEADZONE_SIZE)
                {
                    // increment and wrap around
                    curTileIndex = (curTileIndex + 1) % availableObjects.Count;
                    ReplaceObject(hitPoint);  
                }
                else if (lastPadPos.y < -DEADZONE_SIZE)
                {
                    // decrement and wrap around
                    curTileIndex = (curTileIndex - 1 + availableObjects.Count) % availableObjects.Count;
                    ReplaceObject(hitPoint);   
                }
            }
        }
        
        public override void UpdateTouch(Vector2 padPos) // Swipe to change objects.
        {
            var possibleObjectKeys =
                ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetPossibleObjectKeys();
            if (curObject != null) // Only swipe if there is currently an object in 'hand'.
            {
                if (!trackingSwipe)
                {
                    trackingSwipe = true;
                    swipeStartPosition = padPos;
                }
                
                var offset = (int)(possibleObjectKeys.Count * CalculateSwipe(padPos.x));
                if (offset != 0)
                {
                    swipeStartPosition = padPos;
                    curTileIndex = (curTileIndex + offset + possibleObjectKeys.Count) % possibleObjectKeys.Count;
                    ReplaceObject(hitPoint);
                }
            }
        }

        /// <summary>
        ///     Handle menu button click.
        /// </summary>
        public override void OnMenuUnclick()
        {
            string ASSET_BUNDLES_MENU = "AssetBundlesMenu";
            // If a VR device is present, switch controller's tool to MenuTraversalTool and set the AssetBundleMenu active
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
                controllerManager.right.GetComponent<VRListener>().ChangeTool(typeof(MenuTraversalTool));
                ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive(ASSET_BUNDLES_MENU, true);
            }
            // Else if desktop controls, decide whether we need to activate or deactivate the AssetBundleMenu based on its status
            else
            {
                if (ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference(ASSET_BUNDLES_MENU).activeSelf)
                {
                    ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive(ASSET_BUNDLES_MENU, false);
                }
                else
                {
                    ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive(ASSET_BUNDLES_MENU, true);
                }
            }
            base.OnMenuUnclick();
        }
        
        private float CalculateSwipe(float x)
        {
            return (x - swipeStartPosition.x) / 5;
        }
    }
}