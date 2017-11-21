using UnityEngine;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.viveControllers
{
    public class ControllerMakeCube : MonoBehaviour
    {
        private SteamVR_TrackedObject trackedObj;

        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int) trackedObj.index); }
        }

        private void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        // Update is called once per frame
        private void Update()
        {
            // Put the cube where the controller is
            if (Controller.GetHairTriggerDown())
            {
                Debug.Log("Controller Position: " + trackedObj.transform.position);
                CreateCube(trackedObj.transform.position);
            }
        }

        private void CreateCube(Vector3 controllerPos)
        {
            Coordinate cubePosition = CoordinateHelper.convertUnityCoordinateToWWCoordinate(controllerPos);
            Debug.Log("Cube Position: " + cubePosition.index.x + ", " + cubePosition.index.y + ", " +
                      cubePosition.index.z);

            WWObjectData data = WWObjectFactory.CreateNew(cubePosition, "white");
            WWObject obj = WWObjectFactory.Instantiate(data);
            ManagerRegistry.Instance.sceneGraphManager.Add(obj);
        }
    }
}

// Good reference material for now

/*private GameObject collidingObject;
private GameObject objectInHand;

private SteamVR_Controller.Device Controller
{
    get { return SteamVR_Controller.Input((int)trackedObj.index);  }
}

void Awake()
{
    trackedObj = GetComponent<SteamVR_TrackedObject>();
}

private void SetCollidingObject(Collider col)
{
    if (collidingObject || !col.GetComponent<Rigidbody>())
    {
        return;
    }

    collidingObject = col.gameObject;
}

// When our collider enters another objects collider, set up other collider as potential grab target
// (i.e. set it to collidingObject)
public void OnTriggerEnter(Collider other)
{
    SetCollidingObject(other);
}

// Ensures target is set when player holds controller over an object
public void OnTriggerStay(Collider other)
{
    SetCollidingObject(other);
}

// Set collidingObject to null when we are no longer colliding with an object
public void OnTriggerExit(Collider other)
{
    if (!collidingObject)
    {
        return;
    }

    collidingObject = null;
}

// Move colliding object into player's hand
// Add fixed joint to connect controller to object
private void GrabObject()
{
    objectInHand = collidingObject;
    collidingObject = null;

    var joint = AddFixedJoint();
    joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
}

// Create new fixed joint
// Set up so it doesnt break easily
private FixedJoint AddFixedJoint()
{
    FixedJoint fx = gameObject.AddComponent<FixedJoint>();
    fx.breakForce = 20000;
    fx.breakTorque = 20000;
    return fx;
}

private void ReleaseObject()
{
    if (GetComponent<FixedJoint>())
    {
        // Remove the connection to the held object and destroy the joint
        GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());

        // Add velocity to the object so you can throw things
        objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
        objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
    }

    objectInHand = null;
}

// Update is called once per frame
void Update () {

    // If player clicks trigger and theres a potential grab target, grab it
    if (Controller.GetHairTriggerDown())
    {
        if (collidingObject)
        {
            GrabObject();
        }
    }

    // If player releases trigger and they're holding an object, release it
    if (Controller.GetHairTriggerUp())
    {
        if(objectInHand)
        {
            ReleaseObject();
        }
    }
}*/