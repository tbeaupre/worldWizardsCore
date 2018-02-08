using System.Collections.Generic;
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
        private List<Renderer> objectRenderers;
        
        // Raycast Information
        private Vector3 hitPoint;
        private Vector3 hitPointOffset;
        
        // Swipe

        protected override void Awake()
        {
            base.Awake();
            
            Debug.Log("Edit Object Tool");

            wwObjectToOrigCoordinates = new Dictionary<WWObject, Coordinate>();
            objectRenderers = new List<Renderer>();
            SelectionAwake();
        }
        
        public void Update()
        {
            hitPoint = ToolUtilities.RaycastGridOnly(input.GetControllerPoint(),
                input.GetControllerDirection(), gridController.GetGridCollider(), 200);
            if (OnlyMovingProps())
            {
                hitPoint = ToolUtilities.RaycastGridThenCustom(input.GetControllerPoint(),
                    input.GetControllerDirection(), gridController.GetGridCollider(), WWType.Tile, 200);
            }
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
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                kvp.Key.SetPosition(position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value));
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
                        position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value), 0);
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
            var delta = GetDeltaSnap(position);
            
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
//                var rememberRot = kvp.Value.Rotation;
                var rememberRot = (int )kvp.Key.transform.rotation.eulerAngles.y;
                var coord = CoordinateHelper.UnityCoordToWWCoord(
                    position + CoordinateHelper.WWCoordToUnityCoord(kvp.Value) + delta, rememberRot);
                kvp.Key.SetPosition(coord);
            }
        }

        private void ReverSelectedPositions()
        {
            foreach (var kvp in wwObjectToOrigCoordinates)
            {
                kvp.Key.SetPosition(kvp.Value);
                if (!ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().Add(kvp.Key))
                {
                    Debug.LogError("ReverSelectedPositions : Failed to add selection back to Scene Graph");
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
                ReverSelectedPositions();
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
                var rememberRot = (int) wwObject.transform.rotation.eulerAngles.y;
                var newPos = wwObject.transform.position;
                newPos.y += height * CoordinateHelper.GetTileScale();
                var coord = CoordinateHelper.UnityCoordToWWCoord(newPos, rememberRot);
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
            Renderer[] renderers = GetComponentsInChildren<Renderer> ();
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate (renderer.bounds);
            }
            centerPivot = bounds.center;

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
                    var afterCoord = CoordinateHelper.UnityCoordToWWCoord(wwObject.transform.position, 0);
                    afterCoord.SnapToGrid();
                    var afterSnap = CoordinateHelper.WWCoordToUnityCoord(afterCoord);
                    deltaSnap = afterSnap - before[i];
                    break;
                }
                i++;
            }
            
            foreach (var wwObject in wwObjectToOrigCoordinates.Keys)
            {
                var rememberRot = (int) wwObject.transform.rotation.eulerAngles.y;
                var coord = CoordinateHelper.UnityCoordToWWCoord(
                    wwObject.transform.position + deltaSnap, rememberRot);
                wwObject.SetPosition(coord);
            }
            CompleteEdit();
            UpdateOffsets();
        }
        
        /// <summary>
        ///     Handle menu button click.
        /// </summary>
        public override void OnMenuUnclick()
        {
            // If a VR device is present, switch controller's tool to MenuTraversalTool and set the AssetBundleMenu active
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
                controllerManager.right.GetComponent<VRListener>().ChangeTool(typeof(MenuTraversalTool));
                ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive("AssetBundlesMenu", true);
            }
            // Else if desktop controls, decide whether we need to activate or deactivate the AssetBundleMenu based on its status
            else
            {
                if (ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().GetMenuReference("AssetBundlesMenu").activeSelf)
                {
                    ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive("AssetBundlesMenu", false);
                }
                else
                {
                    ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive("AssetBundlesMenu", true);
                }
            }
            base.OnMenuUnclick();
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

                objectRenderers.Clear();
                foreach (WWObject wwObject in SelectableUnits)
                {
                    if (!ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetDoFilter()
                        || ManagerRegistry.Instance.GetAnInstance<WWObjectGunManager>().GetFilterType() ==
                        wwObject.ResourceMetadata.wwObjectMetadata.type)
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
                            wwObjectToOrigCoordinates.Add(wwObject, wwObject.GetCoordinate());
                            foreach (var r in wwObject.GetAllRenderers())
                            {
                                objectRenderers.Add(r);
                            }
                        }
                    }

                    _highlightsFx.objectRenderers.Clear();
                    foreach (var r in objectRenderers)
                    {
                        _highlightsFx.objectRenderers.Add(r);
                    }
                }
            }
        }
        
    }
}