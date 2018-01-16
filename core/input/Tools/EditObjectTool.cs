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
        }

        public void Update()
        {
            Ray ray = new Ray(input.GetControllerPoint(), input.GetControllerDirection());
            RaycastHit raycastHit;
            if (gridController.GetGridCollider().Raycast(ray, out raycastHit, 100))
            {
                hitPoint = raycastHit.point;

                if (curObjects != null)
                {
                    validTarget = true;
                }
                else
                {
                    Coordinate coordinate = CoordinateHelper.UnityCoordToWWCoord(hitPoint, 0);
                    hoveredObjects =  ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().GetObjectsInCoordinateIndex(coordinate);
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
                    ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(curObject);
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
                        ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Remove(curObject.GetId());
                        if (!(curObject is Tile))
                        {
                            Vector3 wwOffset = curObject.objectData.coordinate.Offset / 2 * CoordinateHelper.GetTileScale();
                            originalOffsets.Add(curObject, wwOffset);
                        }
                    }
                }
                MoveObjects(hitPoint);
            }
        }

        // Touchpad Press
        public override void OnPadUnclick(Vector2 lastPadPos)
        {
            // Rotation
            if (curObjects != null)
            {
                if (lastPadPos.x < -DEADZONE_SIZE)
                {
                    RotateObjects(-90);
                }
                if (lastPadPos.x > DEADZONE_SIZE)
                {
                    RotateObjects(90);
                }
            }
        }

        private void RotateObjects(int rotation)
        {
            foreach (WWObject curObject in curObjects)
            {
                curObject.SetRotation(curObject.GetCoordinate().Rotation + rotation);
                if (originalOffsets.ContainsKey(curObject))
                {
                    Vector3 offset = originalOffsets[curObject];
                    Vector3 temp = new Vector3(offset.z * Math.Sign(rotation), offset.y, offset.x * -Math.Sign(rotation));
                    originalOffsets[curObject] = temp;
                }
            }
        }
    }
}