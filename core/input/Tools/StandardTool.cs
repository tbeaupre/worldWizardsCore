using System;
using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.input.Tools.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.input.Tools
{
    public class StandardTool : Tool
    {
        private const float RETICLE_OFFSET = 0.001f; // The reticle offset from the floor
        private const float MOVE_OFFSET = 0.15f; // The distance the player moves per frame.
        
        // Prefabs
        public GameObject laserPrefab;
        public GameObject reticlePrefab; // Teleport reticle prefab
        
        // Prefab Instances
        private GameObject laser; // Stores reference to an instance of a laser
        private GameObject reticle; // Instance of reticle

        private bool shouldTeleport; // True when valid teleport location is found
        private Vector3 hitPoint; // Hit point of the laser raycast

        
        // Trigger
        public override void OnTriggerUnclick()
        {
            // Selection Logic.
        }
        
        
        // Grip
        public override void OnUngrip() // Teleport to laser location.
        {
            // Hide laser and reticle when button is released.
            DeactivateLaser();

            if (shouldTeleport)
            {
                Teleport(hitPoint);
            }
        }

        public override void UpdateGrip() // Update laser position.
        {
            hitPoint = ToolUtilities.RaycastGridThenCustom(input.GetControllerPoint(),
                input.GetControllerDirection(), gridController.GetGridCollider(), WWType.Tile, 200);
            if (hitPoint != Vector3.zero)
            {
                var distance = Vector3.Distance(hitPoint, input.GetControllerPoint());
                DrawLaser(distance);
                shouldTeleport = true;
            }
            else
            {
                // Hide laser and reticle when no valid target is found.
                DeactivateLaser();
            }
        }

        private void DeactivateLaser()
        {
            if (laser != null)
            {
                Destroy(laser);
                Destroy(reticle);
            }
        }

        private void ActivateLaser()
        {
            if (laser == null)
            {
                laser = Instantiate(laserPrefab);
                reticle = Instantiate(reticlePrefab);
            }
        }

        // Draws the laser and reticle.
        private void DrawLaser(float hitDistance)
        {
            // Show laser and Reticle
            ActivateLaser();
            
            // Put laser between controller and where raycast hits
            laser.transform.position = Vector3.Lerp(input.GetControllerPoint(), hitPoint, .5f);

            // Point laser at position where raycast hit
            laser.transform.LookAt(hitPoint);

            // Scale laser so it fits between the two positions
            laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, hitDistance);
            
            // Move the reticle to where the raycast hit, with an offset to avoid z-fighting
            reticle.transform.position = hitPoint + new Vector3(0, 0, RETICLE_OFFSET);
        }
        
        // Teleports the player to the target location.
        private void Teleport(Vector3 target)
        {
            shouldTeleport = false;
            
            input.GetCameraRigTransform().position = target + input.GetHeadOffset();
        }
        
        
        // Application Menu
        /*public override void OnMenuUnclick()
        {
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
                controllerManager.right.GetComponent<VRListener>().ChangeTool(typeof(MenuTraversalTool));
                ManagerRegistry.Instance.GetAnInstance<WWMenuManager>().SetMenuActive("AssetBundlesMenu", true);
            }
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
        }*/

        
        // Touchpad Press
        public override void UpdatePress(Vector2 padPos) // Y movement.
        {
            if (padPos.y > DEADZONE_SIZE)
            {
                input.GetCameraRigTransform().position += Vector3.down * MOVE_OFFSET;
            }
            if (padPos.y < -DEADZONE_SIZE)
            {
                input.GetCameraRigTransform().position += Vector3.up * MOVE_OFFSET;
            }
        }

        
        // Touchpad Touch
        public override void UpdateTouch(Vector2 padPos) // XZ movement.
        {
            if (Math.Abs(padPos.x) > DEADZONE_SIZE / 2)
            {
                // Move perpendicular to the controller direction in the XZ plane.
                Vector3 strafeVector = padPos.x * input.GetControllerDirection();
                strafeVector = Quaternion.AngleAxis(90, Vector3.up) * strafeVector;
                strafeVector.y = 0;
                strafeVector = strafeVector.normalized * MOVE_OFFSET;
                
                input.GetCameraRigTransform().position += strafeVector;
            }
            if (Math.Abs(padPos.y) > DEADZONE_SIZE / 2)
            {
                // Move in the controller direction in the XZ plane.
                Vector3 forwardMoveVector = padPos.y * input.GetControllerDirection();
                forwardMoveVector.y = 0;
                forwardMoveVector = forwardMoveVector.normalized * MOVE_OFFSET;
            
                input.GetCameraRigTransform().position += forwardMoveVector;  
            }
        }
    }
}