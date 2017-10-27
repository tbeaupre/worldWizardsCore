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
            }
            if (trackingSwipe)
            {
                Vector2 curPosition = controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
                float deltaX = curPosition.x - startPosition.x;

                return deltaX / 2;
            }
            
            return 0;
        }
    }
}