using UnityEngine;

namespace WorldWizards.core.viveControllers
{
    public class LaserPointer : MonoBehaviour
    {
        // Teleport variables
        public Transform cameraRigTransform; // Tranform of [Camera Rig]

        public Transform headTransform; // Player's head transform
        private Vector3 hitPoint; // Position where laser hits
        private GameObject laser; // Stores reference to an instance of a laser
        public GameObject laserPrefab;
        private Transform laserTransform;
        private GameObject reticle; // Instance of reticle
        private bool shouldTeleport; // True when valid teleport location is found
        public LayerMask teleportMask; // To filter areas on which teleports are allowed
        public Vector3 teleportReticleOffset; // Is the reticle offset from the floor
        public GameObject teleportReticlePrefab; // Teleport reticle prefab
        private Transform teleportReticleTransform; // Teleport reticle transform

        // Laser pointer variables
        private SteamVR_TrackedObject trackedObj;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) trackedObj.index); }
        }

        private void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        private void Start()
        {
            laser = Instantiate(laserPrefab);
            laserTransform = laser.transform;

            reticle = Instantiate(teleportReticlePrefab);
            teleportReticleTransform = reticle.transform;
        }

        private void ShowLaser(RaycastHit hit)
        {
            // Show laser
            laser.SetActive(true);

            // Put laser between controller and where raycast hits
            laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);

            // Point laser at position where raycast hit
            laserTransform.LookAt(hitPoint);

            // Scale laser so it fits between the two positions
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
        }

        // Update is called once per frame
        private void Update()
        {
            // If touchpad is held down
            if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                RaycastHit hit;
                // Shoot ray from controller, if it hits something store the point where it hit and show laser
                if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
                {
                    hitPoint = hit.point;
                    ShowLaser(hit);

                    // Show teleport reticle
                    reticle.SetActive(true);

                    // Move the reticle to where the raycast hit, with an offset to avoid z-fighting
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset;

                    // Found valid teleport location
                    shouldTeleport = true;
                }
            }
            else
            {
                // Hide laser when player releases touchpad
                laser.SetActive(false);

                // Hide reticle in the absence of a valid teleport location
                reticle.SetActive(false);
            }

            // Check if player is trying to teleport and teleport them if they are pointing to valid location
            if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
                Teleport();
        }

        private void Teleport()
        {
            shouldTeleport = false;
            reticle.SetActive(false);

            var difference = cameraRigTransform.position - headTransform.position;

            difference.y = 0;

            cameraRigTransform.position = hitPoint + difference;
        }
    }
}