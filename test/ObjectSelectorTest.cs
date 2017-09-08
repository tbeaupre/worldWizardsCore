using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

using worldWizards.core.entity.gameObject;
using worldWizards.core.controller.level;

namespace worldWizards.test
{
	public class ObjectSelectorTest : MonoBehaviour
	{

		SceneGraphController sceneGraphController;

		void Start ()
		{
			sceneGraphController = FindObjectOfType<SceneGraphController> ();
		}

		void Update ()
		{

			if (Input.GetButtonDown ("Fire1")) {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit)) {
					WWObject wwObject = hit.transform.gameObject.GetComponent<WWObject> ();
					if (wwObject) {
						sceneGraphController.DeleteDescending (wwObject.GetId());
					}
				}
			}
		}



	}



}

