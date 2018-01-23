using UnityEngine;
using WorldWizards.SteamVR.Scripts;

namespace WorldWizards.core.viveControllers
{
    public class ShowMenu : MonoBehaviour
    {
        public Transform cameraRigTransform;
        public Camera headCamera;
        public Transform headTransform;

        private bool isMenuActive;
        private GameObject menu;
        private Transform menuTransform;

        private SteamVR_TrackedObject trackedObj;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) trackedObj.index); }
        }

        private void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
            menu = GameObject.Find("Menu");
            menuTransform = menu.transform;
        }

        private void Start()
        {
            isMenuActive = false;
            menu.SetActive(false);
        }

        // Update is called once per frame
        private void Update()
        {
            if (isMenuActive)
            {
                // Change position of the menu based on player head position
                //menuTransform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, headCamera.nearClipPlane));
                //menuTransform.LookAt(headTransform);
            }

            if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                if (!isMenuActive)
                {
                    ShowMainMenu();
                }
                else
                {
                    OnOK();
                }
            }
        }

        private void ShowMainMenu()
        {
            isMenuActive = true;
            menu.SetActive(true);
            Debug.Log("On show, menu active: " + menu.activeInHierarchy);
        }

        private void OnOK()
        {
            isMenuActive = false;
            menu.SetActive(false);
            Debug.Log("On hide, menu active: " + menu.activeInHierarchy);
        }
    }
}