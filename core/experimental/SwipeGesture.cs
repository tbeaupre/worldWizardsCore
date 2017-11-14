using UnityEngine;

namespace WorldWizards.core.experimental
{
    public class SwipeGesture
    {
        private bool trackingSwipe = false;
        private Vector2 startPosition;
	
        public float GetSwipeRatio (SteamVR_Controller.Device controller) {
            // Swipe
            if (controller.GetTouchDown(Valve.VR.EVRButtonId.k_EButton_Axis0)) // Was up, is down.
            {
                trackingSwipe = true;
                startPosition = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            }
            else if (controller.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_Axis0)) // Was down, is up.
            {
                trackingSwipe = false;
                return CalculateSwipe(controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            }
            if (trackingSwipe)
            {
                return CalculateSwipe(controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            }
            
            return 0;
        }

        private float CalculateSwipe(Vector2 curPosition)
        {
            return (curPosition.x - startPosition.x) / 2;
        }
    }
}