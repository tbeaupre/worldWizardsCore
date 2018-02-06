using System;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.input.Tools
{
    public class EditObjectTool : Tool
    {
        private Dictionary<WWObject, Coordinate> wwObjectToOrigCoordinates; // set when objects are picked up.
        
        // Raycast Information
        private Vector3 hitPoint;
        private Vector3 hitPointOffset;
        
        // Swipe
        private bool trackingSwipe = false;
        private Vector2 swipeStartPosition;
        
        
        protected override void Awake()
        {
            base.Awake();
            
            Debug.Log("Edit Object Tool");

            wwObjectToOrigCoordinates = new Dictionary<WWObject, Coordinate>();
            SelectionAwake();
        }

        
        public void Update()
        {
            Ray ray = new Ray(input.GetControllerPoint(), input.GetControllerDirection());
            RaycastHit raycastHit;
            if (gridController.GetGridCollider().Raycast(ray, out raycastHit, 100))
            {
                hitPoint = raycastHit.point;
                hitPoint.y = 0; // IGNORE Y
            }
        }

        private void MoveObjects(Vector3 position)
        {
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                kvp.Key.SetPosition(position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value));
            }
        }
        
        
        private void PlaceObjects(Vector3 position)
        {
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                var rememberRot = kvp.Value.Rotation;
                var coord = CoordinateHelper.UnityCoordToWWCoord(
                    position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value), rememberRot);
                kvp.Key.SetPosition(coord);
            }
        }

        private bool onTriggerDown;
        // Trigger
        public override void OnTriggerUnclick()
        {
            onTriggerDown = false;
//            var target = CoordinateHelper.GetTileCenter(hitPoint);
            PlaceObjects(hitPoint - hitPointOffset);
            if (ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().DoesNotCollide(
                new List<WWObject>(wwObjectToOrigCoordinates.Keys))){
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(kvp.Key);
                }
            }
            else
            {   // snap back failed to place objects because the collided with scene graph
                // TODO rotation will be lost with this cooordinate conversion
                foreach (var kvp in wwObjectToOrigCoordinates)
                {
                    kvp.Key.SetPosition(kvp.Value);
                }
            }
            wwObjectToOrigCoordinates.Clear();
            _highlightsFx.objectRenderers.Clear();
        }

        private void SetHitPointOffset()
        {
            hitPointOffset = hitPoint;
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
            // Rotation
//            if (curObjects != null)
//            {
//                if (lastPadPos.x < -DEADZONE_SIZE)
//                {
//                    RotateObjects(-90);
//                }
//                if (lastPadPos.x > DEADZONE_SIZE)
//                {
//                    RotateObjects(90);
//                }
//            }
        }

        private void RotateObjects(int rotation)
        {
//            foreach (WWObject curObject in curObjects)
//            {
//                curObject.SetRotation(curObject.GetCoordinate().Rotation + rotation);
//                if (originalOffsets.ContainsKey(curObject))
//                {
//                    Vector3 offset = originalOffsets[curObject];
//                    Vector3 temp = new Vector3(offset.z * Math.Sign(rotation), offset.y, offset.x * -Math.Sign(rotation));
//                    originalOffsets[curObject] = temp;
//                }
//            }
        }
        
        
        
        // Selection Code
        // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
        
        private bool justClicked; // defaults to false
        
        private Texture marqueeGraphics;
        private Rect backupRect;
        private Vector2 marqueeOrigin;
        private Rect marqueeRect;
        private Vector2 marqueeSize;
        private List<WWObject> SelectableUnits;

        private HighlightsFX _highlightsFx;

        void SelectionAwake()
        {
            marqueeGraphics = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            _highlightsFx = FindObjectOfType<HighlightsFX>();
        }

        private void OnGUI()
        {
            marqueeRect = new Rect(marqueeOrigin.x, marqueeOrigin.y, marqueeSize.x, marqueeSize.y);
            GUI.color = new Color(0, 0, 0, .3f);
            GUI.DrawTexture(marqueeRect, marqueeGraphics);
        }
        
        public override void OnUngrip()
        {
            Debug.Log("OnTriggerUp");
            // reset state
            justClicked = false;
            marqueeRect.width = 0;
            marqueeRect.height = 0;
            backupRect.width = 0;
            backupRect.height = 0;
            marqueeSize = Vector2.zero;
        }
        
        public override void UpdateGrip()
        {
            if (!justClicked)
            {
                Debug.Log("OnTriggerPressed");
                justClicked = true;
                // treat this as OnPress

                SelectableUnits = new List<WWObject>(FindObjectsOfType<WWObject>());

                float _invertedY = Screen.height - Input.mousePosition.y;
                marqueeOrigin = new Vector2(Input.mousePosition.x, _invertedY);

                //Check if the player just wants to select a single unit opposed to 
                // drawing a marquee and selecting a range of units
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var hitWWObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (hitWWObject != null)
                    {
                        SelectableUnits.Remove(hitWWObject);
                        hitWWObject.Select();
                    }
                }
            }
            else
            {
                Debug.Log("OnTriggerDown");
                float _invertedY = Screen.height - Input.mousePosition.y;
                marqueeSize = new Vector2(Input.mousePosition.x - marqueeOrigin.x, (marqueeOrigin.y - _invertedY) * -1);
                
                //FIX FOR RECT.CONTAINS NOT ACCEPTING NEGATIVE VALUES
                if (marqueeRect.width < 0)
                {
                    backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width), marqueeRect.y,
                        Mathf.Abs(marqueeRect.width), marqueeRect.height);
                }
                else if (marqueeRect.height < 0)
                {
                    backupRect = new Rect(marqueeRect.x, marqueeRect.y - Mathf.Abs(marqueeRect.height),
                        marqueeRect.width, Mathf.Abs(marqueeRect.height));
                }
                if (marqueeRect.width < 0 && marqueeRect.height < 0)
                {
                    backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width),
                        marqueeRect.y - Mathf.Abs(marqueeRect.height), Mathf.Abs(marqueeRect.width),
                        Mathf.Abs(marqueeRect.height));
                }

                List<Renderer> objectRenderers = new List<Renderer>();
                foreach (WWObject wwObject in SelectableUnits)
                {
                    //Convert the world position of the unit to a screen position and then to a GUI point
                    Vector3 _screenPos = Camera.main.WorldToScreenPoint(wwObject.tileFader.GetMeshCenter());
                    var _screenPoint = new Vector2(_screenPos.x, Screen.height - _screenPos.y);
                    //Ensure that any units not within the marquee are currently unselected
                    if (!marqueeRect.Contains(_screenPoint) || !backupRect.Contains(_screenPoint))
                    {
//                        wwObject.Deselect();
                        if (wwObjectToOrigCoordinates.ContainsKey(wwObject))
                        {
                            wwObjectToOrigCoordinates.Remove(wwObject);
                        }
                    }
                    if (marqueeRect.Contains(_screenPoint) || backupRect.Contains(_screenPoint))
                    {
//                        wwObject.Select();
                        wwObjectToOrigCoordinates.Add(wwObject, wwObject.GetCoordinate() );
                        foreach (var r in wwObject.GetAllRenderers())
                        {
                            objectRenderers.Add(r);
                        }
                    }
                }
                _highlightsFx.objectRenderers.Clear();
                foreach (var r in  objectRenderers)
                {
                    _highlightsFx.objectRenderers.Add(r);
                }
            }
        }
        
    }
}