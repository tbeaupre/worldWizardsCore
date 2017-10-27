using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.experimental;

public class UpdatedControls : MonoBehaviour
{
    
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    
    private readonly SwipeGesture swipe = new SwipeGesture();

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start () {
		
    }
	
    // Update is called once per frame
    void Update () {
        // Button Press
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
        }

        float swipeRatio = swipe.GetSwipeRatio(Controller);
    }
}
