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
        public GameObject teleportReticlePrefab; // Teleport reticle prefab
        
        // VR Headset Transform Information
        private Transform cameraRigTransform; // Tranform of [Camera Rig]
        private Transform headTransform; // Player's head transform
        
        // Prefab Instances
        private GameObject laser; // Stores reference to an instance of a laser
        private GameObject reticle; // Instance of reticle

        private bool shouldTeleport; // True when valid teleport location is found
        private Vector3 hitPoint; // Hit point of the laser raycast
        
        protected override void Awake()
        {
            base.Awake();
            trigger = true;
            grip = true;
            appMenu = true;
            press = true;
            touch = true;

            SteamVR_ControllerManager controllerManager = FindObjectOfType<SteamVR_ControllerManager>();
            cameraRigTransform = controllerManager.transform;
            headTransform = controllerManager.GetComponent<Camera>().transform;
        }

        private void Start()
        {
            laser = Instantiate(laserPrefab);
            reticle = Instantiate(teleportReticlePrefab);
        }

        // Trigger
        protected override void OnTrigger()
        {
            // Selection Logic.
        }
        
        
        // Grip
        protected override void OnGrip()
        {
            // Hide laser and reticle when button is released.
            laser.SetActive(false);
            reticle.SetActive(false);

            if (shouldTeleport)
            {
                Teleport(hitPoint);
            }
        }

        protected override void UpdateGrip()
        {
            RaycastHit hit;
            // Shoot ray from controller, if it hits something store the point where it hit and show laser
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
            {
                // Found valid teleport location
                DrawLaser(hit);
                hitPoint = hit.point;
                shouldTeleport = true;
            }
            else
            {
                // Hide laser and reticle when no valid target is found.
                laser.SetActive(false);
                reticle.SetActive(false);
            }
        }

        // Draws the laser and reticle.
        private void DrawLaser(RaycastHit hit)
        {
            // Show laser and Reticle
            laser.SetActive(true);
            reticle.SetActive(true);
            
            // Put laser between controller and where raycast hits
            laser.transform.position = Vector3.Lerp(trackedObj.transform.position, hit.point, .5f);

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
        protected override void OnAppMenu()
        {
            // Menu Logic.
        }

        
        // Touchpad Press
        protected override void UpdateTouchpadPress(Vector2 pos)
        {
            if (pos.y > DEADZONE_SIZE)
            {
                cameraRigTransform.position += Vector3.up * MOVE_OFFSET;
            }
            if (pos.y < -DEADZONE_SIZE)
            {
                cameraRigTransform.position += Vector3.down * MOVE_OFFSET;
            }
        }

        
        // Touchpad Touch
        protected override void UpdateTouchpadTouch(Vector2 pos)
        {
            cameraRigTransform.position += pos.y * Vector3.forward * MOVE_OFFSET;
            cameraRigTransform.position += pos.x * Vector3.right * MOVE_OFFSET;
        }
    }
}