﻿using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.input.Tools.utils;
using WorldWizards.core.input.VRControls;
using WorldWizards.core.manager;
using WorldWizards.SteamVR.Scripts;

namespace WorldWizards.core.input.Tools
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    public class EditObjectTool : Tool
    {
        private Dictionary<WWObject, Coordinate> wwObjectToOrigCoordinates; // set when objects are picked up.
        
        // Raycast Information
        private Vector3 hitPoint;
        private Vector3 hitPointOffset;

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("Edit Object Tool");
            wwObjectToOrigCoordinates = new Dictionary<WWObject, Coordinate>();
            SelectionAwake();
        }
        
        public void Update()
        {
            var lastHitPoint = hitPoint;
            hitPoint = ToolUtilities.RaycastGridOnly(input.GetControllerPoint(),
                input.GetControllerDirection(), gridController.GetGridCollider(), 200);
            if (OnlyMovingProps())
            {
                hitPoint = ToolUtilities.RaycastGridThenCustom(input.GetControllerPoint(),
                    input.GetControllerDirection(), gridController.GetGridCollider(), WWType.Tile, 200);
            }
            if (hitPoint.Equals(Vector3.zero))
            {
                hitPoint = lastHitPoint;
            }
            Debug.DrawLine(Vector3.zero, hitPoint, Color.red);
        }

        
        private bool OnlyMovingProps()
        {
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                if (wwObject.ResourceMetadata.wwObjectMetadata.type != WWType.Prop)
                {
                    return false;
                }
            }
            return true;
        }

        private void MoveObjects(Vector3 position)
        {
            if (OnlyMovingProps())
            {
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    var origOffset = CoordinateHelper.WWCoordToUnityCoord(kvp.Value);
                    var y = input.GetControllerPoint().y;
                    var origin = new Vector3(position.x + origOffset.x, y, position.z + origOffset.z);
                    var stuckPoint = ToolUtilities.RaycastGridThenCustom(origin, Vector3.down,
                        gridController.GetGridCollider(), WWType.Tile, 200f);                    
                    kvp.Key.SetPosition(stuckPoint);
                }   
            }

            else
            {
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    kvp.Key.SetPosition(position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value));
                }
            }
        }


        private Vector3 GetDeltaSnap(Vector3 position)
        {
            Vector3 deltaSnap = Vector3.zero;
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                if (kvp.Key.ResourceMetadata.wwObjectMetadata.type == WWType.Tile)
                {
          
                    var coord = CoordinateHelper.UnityCoordToWWCoord(
                        position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value));
                    var original = CoordinateHelper.WWCoordToUnityCoord(coord);
                    coord.SnapToGrid();
                    var afterSnap = CoordinateHelper.WWCoordToUnityCoord(coord);
                    deltaSnap = afterSnap - original;
                    break;
                }
            }
            return deltaSnap;
        }

        private void PlaceObjects(Vector3 position)
        {
            if (OnlyMovingProps())
            {
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    var origOffset = CoordinateHelper.WWCoordToUnityCoord(kvp.Value);
                    var y = input.GetControllerPoint().y;
                    var origin = new Vector3(position.x + origOffset.x, y, position.z + origOffset.z);
                    var stuckPoint = ToolUtilities.RaycastGridThenCustom(origin, Vector3.down,
                        gridController.GetGridCollider(), WWType.Tile, 200f);                    
                    var coord = CoordinateHelper.UnityCoordToWWCoord(stuckPoint);
                    kvp.Key.SetPosition(coord);
                }      
            }
            else
            {
                var delta = GetDeltaSnap(position);
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    var coord = CoordinateHelper.UnityCoordToWWCoord(
                        position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value) + delta);
                    kvp.Key.SetPosition(coord);
                }
            }
        }

        private void RevertSelectedPositions()
        {
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                kvp.Key.SetPosition(kvp.Value);
                if (!ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(kvp.Key))
                {
                    Debug.LogError("RevertSelectedPositions : Failed to add selection back to Scene Graph");
                }
            }
        }

        private void UpdateOffsets()
        {
            // update the origin coordinates to new coordaintes, and do not deselect current selection
            var temp = new List<WWObject>(wwObjectToOrigCoordinates.Keys);
            foreach (var wwObject in temp)
            {
                wwObjectToOrigCoordinates[wwObject] = wwObject.GetCoordinate();
            }
        }

        private void CompleteEdit()
        {
            if (ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().DoesNotCollide(
                new List<WWObject>(wwObjectToOrigCoordinates.Keys)))
            {
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(kvp.Key);
                }
            }
            else
            {
                RevertSelectedPositions();
            }
        }

        private bool onTriggerDown;
        // Trigger
        public override void OnTriggerUnclick()
        {
            onTriggerDown = false;
            PlaceObjects(hitPoint - hitPointOffset);
            CompleteEdit();

            UpdateOffsets();
        }

        private void DeselectAll()
        {
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                kvp.Key.Deselect();
            }
            wwObjectToOrigCoordinates.Clear();
        }


        private Vector3 GetCenterPoint()
        {
            var centerPivot = Vector3.zero;
            int count = 0;
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                centerPivot += wwObject.transform.position;
                count++;
            }
            if (count != 0)
            {
                centerPivot /= count;
            }    
            Bounds bounds = new Bounds (centerPivot, Vector3.one);

            List<Renderer> renderers = new List<Renderer>();
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                var objectsRenderers = kvp.Key.GetAllRenderers();
                foreach (var objRenderer in objectsRenderers)
                {
                    renderers.Add(objRenderer);
                }
            }
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate (renderer.bounds);
            }
            centerPivot = bounds.center;
            return centerPivot;
        }

        private void SetHitPointOffset()
        {
            hitPointOffset = GetCenterPoint();
            hitPointOffset.y = hitPoint.y;
        }

        public override void UpdateTrigger()
        {
            if (!onTriggerDown) 
            {   // runs only once per event
                onTriggerDown = true;
                SetHitPointOffset();
                foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
                {
                    ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Remove(wwObject.GetId());
                }
            }
            MoveObjects(hitPoint - hitPointOffset);
        }

        // Touchpad Press
        public override void OnPadUnclick(Vector2 lastPadPos)
        {
            if (lastPadPos.x < -DEADZONE_SIZE)
            {
                RotateObjects(-90);
            }
            if (lastPadPos.x > DEADZONE_SIZE)
            {
                RotateObjects(90);
            }
            
            // Move Grid
            if (lastPadPos.y > DEADZONE_SIZE)
            {
                MoveVertically(1);
            }
            if (lastPadPos.y < -DEADZONE_SIZE)
            {
                MoveVertically(-1);
            }
        }

        private void MoveVertically(int height)
        {
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Remove(wwObject.GetId());
                var newPos = wwObject.transform.position;
                newPos.y += height * CoordinateHelper.GetTileScale();
                var coord = CoordinateHelper.UnityCoordToWWCoord(newPos);
                wwObject.SetPosition(coord);
            }
            CompleteEdit();
            UpdateOffsets();
        }

        
        private void RotateObjects(int rotation)
        {
            if (wwObjectToOrigCoordinates.Count == 0) return;
      
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Remove(wwObject.GetId());
            }

            var centerPivot = GetCenterPoint();

            List<Vector3> before = new List<Vector3>();
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                before.Add(wwObject.transform.position);
                wwObject.transform.RotateAround(centerPivot, Vector3.up, rotation);
            }

            Vector3 deltaSnap = Vector3.zero;
            int i = 0;
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {            
                if (wwObject.ResourceMetadata.wwObjectMetadata.type == WWType.Tile)
                {
                    var afterCoord = CoordinateHelper.UnityCoordToWWCoord(wwObject.transform.position);
                    afterCoord.SnapToGrid();
                    var afterSnap = CoordinateHelper.WWCoordToUnityCoord(afterCoord);
                    deltaSnap = afterSnap - before[i];
                    break;
                }
                i++;
            }
            
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                var coord = CoordinateHelper.UnityCoordToWWCoord(
                    wwObject.transform.position + deltaSnap);
                wwObject.SetPosition(coord);
                wwObject.SetRotation((int) wwObject.transform.localEulerAngles.y);
            }
            CompleteEdit();
            UpdateOffsets();
        }
        
        // Selection Code
        // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
        
        private bool justClicked; // defaults to false
        private List<WWObject> selectableUnits;
        // This variable will store the location of wherever we first click before dragging.
        private Vector2 initialClickPosition = Vector2.zero;
        public SelectionCanvasController selectionCanvasController;

        void SelectionAwake()
        {
            var go = Instantiate(Resources.Load("Prefabs/SelectionCanvasController")) as GameObject;
            selectionCanvasController = go.GetComponent<SelectionCanvasController>();
        }
         
        public override void OnUngrip()
        {
            Debug.Log("OnGripUp");
            justClicked = false;
            // reset
            initialClickPosition = Vector2.zero;
            selectionCanvasController.marqueeRectTransform.anchoredPosition = Vector2.zero;
            selectionCanvasController.marqueeRectTransform.sizeDelta = Vector2.zero;
        }
        
        public override void UpdateGrip()
        {
            Vector3 pointerPosAsScreenPos;
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                Vector3 pointerOffset =  input.GetControllerPoint() + (input.GetControllerDirection().normalized * 200f);
                pointerPosAsScreenPos = Camera.main.WorldToScreenPoint(pointerOffset);
            }
            else // desktop controls
            {
                pointerPosAsScreenPos = Input.mousePosition;
            }
            // treat this as OnPress
            if (!justClicked) 
            {
                Debug.Log("OnGripUp");
                justClicked = true;
                selectableUnits = new List<WWObject>(FindObjectsOfType<WWObject>());
                // Get the initial click position of the mouse. No need to convert to GUI space
                // since we are using the lower left as anchor and pivot.
                initialClickPosition = new Vector2(pointerPosAsScreenPos.x, pointerPosAsScreenPos.y);
                // The anchor is set to the same place.
                selectionCanvasController.marqueeRectTransform.anchoredPosition = initialClickPosition;
                // Check if the player just wants to select a single unit opposed to 
                // drawing a marquee and selecting a range of units
                var hitWWObject = ToolUtilities.RaycastNoGrid(input.GetControllerPoint(), input.GetControllerDirection(), 200f);
                if (hitWWObject != null)
                {
                    selectableUnits.Remove(hitWWObject);
                    if (!wwObjectToOrigCoordinates.ContainsKey(hitWWObject))
                    {
                        wwObjectToOrigCoordinates.Add(hitWWObject, hitWWObject.GetCoordinate());
                        hitWWObject.Select();
                    }
                }
            }
            else
            {
                Debug.Log("OnTriggerDown");
                Vector2 currentMousePosition = new Vector2(pointerPosAsScreenPos.x, pointerPosAsScreenPos.y);         
                Vector2 difference = currentMousePosition - initialClickPosition;
                 
                // Copy the initial click position to a new variable. Using the original variable will cause
                // the anchor to move around to wherever the current mouse position is,
                // which isn't desirable.
                Vector2 startPoint = initialClickPosition;
                 
                // The following code accounts for dragging in various directions.
                if (difference.x < 0)
                {
                    startPoint.x = currentMousePosition.x;
                    difference.x = -difference.x;
                }
                if (difference.y < 0)
                {
                    startPoint.y = currentMousePosition.y;
                    difference.y = -difference.y;
                }
                 
                // Set the anchor, width and height every frame.
                selectionCanvasController.marqueeRectTransform.anchoredPosition = startPoint;
                selectionCanvasController.marqueeRectTransform.sizeDelta = difference;

                foreach (WWObject wwObject in selectableUnits)
                {
                    if (!ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter()
                        || ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() ==
                        wwObject.ResourceMetadata.wwObjectMetadata.type)
                    {
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(wwObject.tileFader.GetMeshCenter());
                        if (screenPos.z > 0) // ignore objects behind the camera
                        {
                            var screenPoint = new Vector2(screenPos.x, screenPos.y);
                            if (!RectTransformUtility.RectangleContainsScreenPoint(
                                selectionCanvasController.marqueeRectTransform,
                                screenPoint))
                            {
                                if (wwObjectToOrigCoordinates.ContainsKey(wwObject))
                                {
                                    wwObject.Deselect();
                                    wwObjectToOrigCoordinates.Remove(wwObject);
                                }
                            }
                            else if (!wwObjectToOrigCoordinates.ContainsKey(wwObject))
                            {
                                wwObjectToOrigCoordinates.Add(wwObject, wwObject.GetCoordinate());
                                wwObject.Select();
                            }
                        }
                    }
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

        private void OnDestroy()
        {
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                kvp.Key.Deselect();
            }            
            Destroy(selectionCanvasController.gameObject);
        }
    }
}