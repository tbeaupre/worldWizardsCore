using System;
using UnityEngine;

namespace worldWizards.core.experimental.controllers.Desktop
{
    public class MoveCameraWithMouse : MonoBehaviour
    {
        float speed = 1.5f;
        int boundary = 75;
        int width;
        int height;
 
        void Start ()
        {
            width = Screen.width;
            height = Screen.height;
        }
        
        void Update ()
        {
            Vector2 mousePos = Input.mousePosition;
            if (mousePos.x < 0) mousePos.x = 0;
            if (mousePos.y < 0) mousePos.y = 0;
            if (mousePos.x > width) mousePos.x = width;
            if (mousePos.y > height) mousePos.y = height;
            
            if (mousePos.x > width - boundary)
            {
                transform.RotateAround(transform.position, Vector3.up, (mousePos.x - width + boundary) * Time.deltaTime * speed);
            }
		
            if (mousePos.x < boundary)
            {
                transform.RotateAround(transform.position, Vector3.up, (mousePos.x - boundary) * Time.deltaTime * speed);
            }
		
            if (mousePos.y > height - boundary)
            {
                transform.Rotate(new Vector3 (-(mousePos.y - height + boundary) * Time.deltaTime * speed, 0.0f, 0.0f));
            }

            if (mousePos.y < boundary)
            {
                transform.Rotate(new Vector3 (-(mousePos.y - boundary) * Time.deltaTime * speed, 0.0f, 0.0f));
            }
        }
    }
}