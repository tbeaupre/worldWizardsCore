using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.gameObject;

namespace worldWizards.core.input.Tools
{
    /// <summary>
    /// Based on this script from 
    /// https://paulbutera.wordpress.com/2013/04/04/unity-rts-tutorial-part-1-marquee-selection-of-units/
    /// </summary>
    public class SelectionTool : Tool
    {
        private bool justClicked; // defaults to false
        
        private Texture marqueeGraphics;
        private Rect backupRect;
        private Vector2 marqueeOrigin;
        private Rect marqueeRect;
        private Vector2 marqueeSize;
        private List<WWObject> SelectableUnits;

        void Awake()
        {
            marqueeGraphics = new Texture2D(2, 2, TextureFormat.ARGB32, false); 
        }

        private void OnGUI()
        {
            marqueeRect = new Rect(marqueeOrigin.x, marqueeOrigin.y, marqueeSize.x, marqueeSize.y);
            GUI.color = new Color(0, 0, 0, .3f);
            GUI.DrawTexture(marqueeRect, marqueeGraphics);
        }
        
        public override void OnTriggerUnclick()
        {
            Debug.Log("OnTriggerUp");
            // reset state
            justClicked = false;
            marqueeRect.width = 0;
            marqueeRect.height = 0;
            backupRect.width = 0;
            backupRect.height = 0;
            marqueeSize = Vector2.zero;
        }

        public override void UpdateTrigger()
        {
            if (!justClicked)
            {
                Debug.Log("OnTriggerPressed");
                justClicked = true;
                // treat this as OnPress

                SelectableUnits = new List<WWObject>(FindObjectsOfType<WWObject>());

                float _invertedY = Screen.height - Input.mousePosition.y;
                marqueeOrigin = new Vector2(Input.mousePosition.x, _invertedY);

                //Check if the player just wants to select a single unit opposed to 
                // drawing a marquee and selecting a range of units
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    var hitWWObject = hit.transform.gameObject.GetComponent<WWObject>();
                    if (hitWWObject != null)
                    {
                        SelectableUnits.Remove(hitWWObject);
                        hitWWObject.Select();
                    }
                }
            }
            else
            {
                Debug.Log("OnTriggerDown");
                float _invertedY = Screen.height - Input.mousePosition.y;
                marqueeSize = new Vector2(Input.mousePosition.x - marqueeOrigin.x, (marqueeOrigin.y - _invertedY) * -1);
                
                //FIX FOR RECT.CONTAINS NOT ACCEPTING NEGATIVE VALUES
                if (marqueeRect.width < 0)
                {
                    backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width), marqueeRect.y,
                        Mathf.Abs(marqueeRect.width), marqueeRect.height);
                }
                else if (marqueeRect.height < 0)
                {
                    backupRect = new Rect(marqueeRect.x, marqueeRect.y - Mathf.Abs(marqueeRect.height),
                        marqueeRect.width, Mathf.Abs(marqueeRect.height));
                }
                if (marqueeRect.width < 0 && marqueeRect.height < 0)
                {
                    backupRect = new Rect(marqueeRect.x - Mathf.Abs(marqueeRect.width),
                        marqueeRect.y - Mathf.Abs(marqueeRect.height), Mathf.Abs(marqueeRect.width),
                        Mathf.Abs(marqueeRect.height));
                }
                foreach (WWObject wwObject in SelectableUnits)
                {
                    //Convert the world position of the unit to a screen position and then to a GUI point
                    Vector3 _screenPos = Camera.main.WorldToScreenPoint(wwObject.transform.position);
                    var _screenPoint = new Vector2(_screenPos.x, Screen.height - _screenPos.y);
                    //Ensure that any units not within the marquee are currently unselected
                    if (!marqueeRect.Contains(_screenPoint) || !backupRect.Contains(_screenPoint))
                    {
                        wwObject.Deselect();
                    }
                    if (marqueeRect.Contains(_screenPoint) || backupRect.Contains(_screenPoint))
                    {
                        wwObject.Select();
                    }
                }
            }
        }
    }
}