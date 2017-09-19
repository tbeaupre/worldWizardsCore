using UnityEngine;

using worldWizards.core.entity.gameObject;
using worldWizards.core.controller.level;

namespace worldWizards.core.experimental
{
	public class ObjectSelector : MonoBehaviour
	{

		SceneGraphController sceneGraphController;

		void Start ()
		{
			sceneGraphController = FindObjectOfType<SceneGraphController> ();
		}

		void Update ()
		{

			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit)) {
					WWObject wwObject = hit.transform.gameObject.GetComponent<WWObject> ();
					if (wwObject) {
						sceneGraphController.Delete (wwObject.GetId());
					}
				}
			}

			if (Input.GetKeyDown(KeyCode.Space)) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit)) {
					WWObject wwObject = hit.transform.gameObject.GetComponent<WWObject> ();
					if (wwObject) {
						wwObject.Unparent ();
					}
				}
			}
		}



	}



}

