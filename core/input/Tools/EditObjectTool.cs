using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace worldWizards.core.input.Tools
{
    public class EditObjectTool : Tool
    {
        
        // Prefabs
        private static Collider gridCollider;
        
        // Object Properties
        private List<WWObject> curObjects;
        private Dictionary<WWObject, Vector3> originalOffsets; // set when objects are picked up.
        
        private List<WWObject> hoveredObjects;

        // Raycast Information
        private bool validTarget = false;
        private Vector3 hitPoint;
        
        // Swipe
        private bool trackingSwipe = false;
        private Vector2 swipeStartPosition;
        
        
        protected override void Awake()
        {
            base.Awake();

            originalOffsets = new Dictionary<WWObject, Vector3>();

            if (gridCollider == null)
            {
                gridCollider = FindObjectOfType<MeshCollider>();
                gridCollider.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            }
        }

        public void Update()
        {
            Ray ray = new Ray(input.GetControllerPoint(), input.GetControllerDirection());
            RaycastHit raycastHit;
            if (gridCollider.Raycast(ray, out raycastHit, 100))
            {
                hitPoint = raycastHit.point;

                if (curObjects != null)
                {
                    validTarget = true;
                }
                else
                {
                    Coordinate coordinate = CoordinateHelper.UnityCoordToWWCoord(hitPoint);
                    hoveredObjects = ManagerRegistry.Instance.sceneGraphManager.GetObjectsInCoordinateIndex(coordinate);
                    validTarget = hoveredObjects.Count > 0;
                }
            }
            else
            {
                validTarget = false;
            }
        }

        private void MoveObjects(Vector3 position)
        {
            foreach (WWObject curObject in curObjects)
            {
                Vector3 offset = Vector3.zero;
                if (originalOffsets.ContainsKey(curObject))
                {
                    offset = originalOffsets[curObject];
                }
                curObject.SetPosition(position + offset, false);
            }
        }

        
        // Trigger
        public override void OnTriggerUnclick()
        {
            if (validTarget)
            {
                Vector3 target = hitPoint;
                if (originalOffsets.Count != curObjects.Count)
                {
                    // Contains some tiles, so move to center of tile. Else, just props so it doesn't matter.
                    target = CoordinateHelper.GetTileCenter(target);
                }
                foreach (WWObject curObject in curObjects)
                {
                    Vector3 offset = Vector3.zero;
                    if (originalOffsets.ContainsKey(curObject))
                    {
                        offset = originalOffsets[curObject];
                    }
                    curObject.SetPosition(target + offset, true);
                    ManagerRegistry.Instance.sceneGraphManager.Add(curObject);
                }
                curObjects = null;
                originalOffsets.Clear();
            }
        }

        public override void UpdateTrigger()
        {
            if (validTarget)
            {
                // Check if something is already selected (if this is the first frame the trigger is down, there won't)
                if (curObjects == null)
                {
                    curObjects = hoveredObjects;
                    foreach (WWObject curObject in curObjects)
                    {
                        ManagerRegistry.Instance.sceneGraphManager.Remove(curObject.GetId());
                        if (!(curObject is Tile))
                        {
                            Vector3 wwOffset = curObject.objectData.coordinate.offset / 2 * CoordinateHelper.GetTileScale();
                            originalOffsets.Add(curObject, wwOffset);
                        }
                    }
                }
                MoveObjects(hitPoint);
            }
        }

        /*
        // Grip
        public override void OnUngrip()
        {
            if (curObject == null)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(controller.GetControllerPoint(), controller.GetControllerDirection(), out raycastHit, 100))
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
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
                if (lastPadPos.x > DEADZONE_SIZE)
                {
                    curRotation += 90;
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
        public override void OnPadUntouch(Vector2 lastPadPos)
        {
            trackingSwipe = false;

            if (Math.Abs(lastPadPos.x) < DEADZONE_SIZE / 2)
            {
                if (lastPadPos.y > DEADZONE_SIZE)
                {
                    curTileIndex = (curTileIndex + 1) % possibleTiles.Count;
                    if (curObject != null) Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
                if (lastPadPos.y < -DEADZONE_SIZE)
                {
                    curTileIndex = (curTileIndex - 1 + possibleTiles.Count) % possibleTiles.Count;
                    if (curObject != null) Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
            }
        }
        
        public override void UpdateTouch(Vector2 padPos)
        {
            if (curObject != null)
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
                    Destroy(curObject.gameObject);
                    curObject = PlaceObject(hitPoint);
                }
            }
        }
        
        private float CalculateSwipe(float x)
        {
            return (x - swipeStartPosition.x) / 5;
        }
        */
    }
}