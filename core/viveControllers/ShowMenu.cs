using System.Collections;
using System.Collections.Generic;
using worldWizards.core.controller.level.utils;
using worldWizards.core.controller.level;
using worldWizards.core.entity.coordinate;
using worldWizards.core.entity.coordinate.utils;
using worldWizards.core.entity.gameObject;
using UnityEngine;

public class ShowMenu : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;
    private SceneGraphController sceneGraphController;
    private GameObject menu;
    private Transform menuTransform;
    public Transform cameraRigTransform;
    public Transform headTransform;
    public Camera headCamera;

    private bool isMenuActive;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        menu = GameObject.Find("Menu");
        menuTransform = menu.transform;
    }

    void Start()
    {
        sceneGraphController = FindObjectOfType<SceneGraphController>();

        isMenuActive = false;
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMenuActive)
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

    void ShowMainMenu()
    {
        isMenuActive = true;
        menu.SetActive(true);
        Debug.Log("On show, menu active: " + menu.activeInHierarchy);
    }

    void OnOK()
    {
        isMenuActive = false;
        menu.SetActive(false);
        Debug.Log("On hide, menu active: " + menu.activeInHierarchy);
    }
}
