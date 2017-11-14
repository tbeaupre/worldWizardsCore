using System;
using UnityEngine;

namespace WorldWizards.core.experimental.controllers
{
    public class StandardTool : Tool
    {
        private const float RETICLE_OFFSET = 0.001f; // The reticle offset from the floor
        private const float MOVE_OFFSET = 0.1f; // The distance the player moves per frame.
        
        // Prefabs
        public GameObject laserPrefab;
        public GameObject reticlePrefab; // Teleport reticle prefab
        
        // VR Headset Transform Information
        private Transform cameraRigTransform; // Tranform of [Camera Rig]
        private Transform headTransform; // Player's head transform
        
        // Prefab Instances
        private GameObject laser; // Stores reference to an instance of a laser
        private GameObject reticle; // Instance of reticle

        private bool shouldTeleport; // True when valid teleport location is found
        private Vector3 hitPoint; // Hit point of the laser raycast
        
        private void Awake()
        {
            listenForTrigger = true;
            listenForGrip = true;
            listenForMenu = true;
            listenForPress = true;
            listenForTouch = true;
            
            SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
            cameraRigTransform = controllerManager.transform;
            headTransform = controllerManager.GetComponentInChildren<Camera>().transform;
        }

        // Trigger
        public override void OnTriggerUnclick()
        {
            // Selection Logic.
        }
        
        
        // Grip
        public override void OnUngrip()
        {
            // Hide laser and reticle when button is released.
            DeactivateLaser();

            if (shouldTeleport)
            {
                Teleport(hitPoint);
            }
        }

        public override void UpdateGrip()
        {
            RaycastHit hit;
            // Shoot ray from controller, if it hits something store the point where it hit and show laser
            if (Physics.Raycast(controllerTransform.position, transform.forward, out hit, 100))
            {
                // Found valid teleport location
                DrawLaser(hit);
                hitPoint = hit.point;
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
        private void DrawLaser(RaycastHit hit)
        {
            // Show laser and Reticle
            ActivateLaser();
            
            // Put laser between controller and where raycast hits
            laser.transform.position = Vector3.Lerp(controllerTransform.position, hit.point, .5f);

            // Point laser at position where raycast hit
            laser.transform.LookAt(hit.point);

            // Scale laser so it fits between the two positions
            laser.transform.localScale = new Vector3(laser.transform.localScale.x, laser.transform.localScale.y, hit.distance);
            
            // Move the reticle to where the raycast hit, with an offset to avoid z-fighting
            reticle.transform.position = hit.point + new Vector3(0, 0, RETICLE_OFFSET);
        }
        
        // Teleports the player to the target location.
        private void Teleport(Vector3 target)
        {
            shouldTeleport = false;
            
            var difference = cameraRigTransform.position - headTransform.position;

            difference.y = 0;

            cameraRigTransform.position = target + difference;
        }
        
        
        // Application Menu
        public override void OnMenuUnclick()
        {
            // Menu Logic.
        }

        
        // Touchpad Press
        public override void UpdatePress(Vector2 padPos)
        {
            if (padPos.y > DEADZONE_SIZE)
            {
                cameraRigTransform.position += Vector3.down * MOVE_OFFSET;
            }
            if (padPos.y < -DEADZONE_SIZE)
            {
                cameraRigTransform.position += Vector3.up * MOVE_OFFSET;
            }
        }

        
        // Touchpad Touch
        public override void UpdateTouch(Vector2 padPos)
        {
            if (Math.Abs(padPos.x) > DEADZONE_SIZE / 2)
            {
                Vector3 strafeVector = padPos.x * gameObject.transform.right;
                strafeVector.y = 0;
                strafeVector = strafeVector.normalized * MOVE_OFFSET;
                
                cameraRigTransform.position += strafeVector;
            }
            if (Math.Abs(padPos.y) > DEADZONE_SIZE / 2)
            {
                Vector3 forwardMoveVector = padPos.y * gameObject.transform.forward;
                forwardMoveVector.y = 0;
                forwardMoveVector = forwardMoveVector.normalized * MOVE_OFFSET;
            
                cameraRigTransform.position += forwardMoveVector;  
            }
        }
    }
}