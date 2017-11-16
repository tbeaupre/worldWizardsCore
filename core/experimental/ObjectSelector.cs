using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    public class ObjectSelector : MonoBehaviour
    {

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    var wwObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (wwObject)  ManagerRegistry.Instance.sceneGraphImpl.Delete(wwObject.GetId());
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    var wwObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (wwObject) wwObject.Unparent();
                }
            }
        }
    }
}