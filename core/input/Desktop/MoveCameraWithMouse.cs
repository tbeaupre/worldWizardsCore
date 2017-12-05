using UnityEngine;

namespace worldWizards.core.input.Desktop
{
    /**
     * Script which rotates the camera when the user moves the mouse to the edge of the screen.
     */
    public class MoveCameraWithMouse : MonoBehaviour
    {
        private const float SPEED = 1.5f;
        private const int BOUNDARY = 75;
        private int width;
        private int height;
 
        void Start ()
        {
            width = Screen.width;
            height = Screen.height;
        }
        
        void Update ()
        {
            Vector2 mousePos = Input.mousePosition;
            
            // Clamps the mouse position to the boundary of the screen.
            if (mousePos.x < 0) mousePos.x = 0;
            if (mousePos.y < 0) mousePos.y = 0;
            if (mousePos.x > width) mousePos.x = width;
            if (mousePos.y > height) mousePos.y = height;
            
            if (mousePos.x > width - BOUNDARY)
            {
                transform.RotateAround(transform.position, Vector3.up, (mousePos.x - width + BOUNDARY) * Time.deltaTime * SPEED);
            }
            if (mousePos.x < BOUNDARY)
            {
                transform.RotateAround(transform.position, Vector3.up, (mousePos.x - BOUNDARY) * Time.deltaTime * SPEED);
            }
            if (mousePos.y > height - BOUNDARY)
            {
                transform.Rotate(new Vector3 (-(mousePos.y - height + BOUNDARY) * Time.deltaTime * SPEED, 0.0f, 0.0f));
            }
            if (mousePos.y < BOUNDARY)
            {
                transform.Rotate(new Vector3 (-(mousePos.y - BOUNDARY) * Time.deltaTime * SPEED, 0.0f, 0.0f));
            }
        }
    }
}