using UnityEngine;

namespace WorldWizards.core.experimental
{
    [AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
    public class MouseOrbitImproved : MonoBehaviour
    {
        public float distance = 5.0f;
        public float distanceMax = 15f;

        public float distanceMin = .5f;

        private Rigidbody rigidbody;

        public Transform target;

        private float x;
        public float xSpeed = 120.0f;
        private float y;
        public float yMaxLimit = 80f;

        public float yMinLimit = -20f;
        public float ySpeed = 120.0f;

        // Use this for initialization
        private void Start()
        {
            var angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            rigidbody = GetComponent<Rigidbody>();

            // Make the rigid body not change rotation
            if (rigidbody != null)
                rigidbody.freezeRotation = true;
        }

        private void LateUpdate()
        {
            if (target)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                var rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.position, transform.position, out hit))
                    distance -= hit.distance;
                var negDistance = new Vector3(0.0f, 0.0f, -distance);
                var position = rotation * negDistance + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}