using System;
using UnityEngine;

namespace WorldWizards.core.menus
{   
    /// <summary>
    ///     WWButton holds all the info for a button used in World Wizards.
    ///     It allows buttons to be clicked with the VR controller laser.
    ///     Also holds any metadata the button might need when clicked.
    ///     All buttons inherit from WWButton.
    /// </summary>
    
    [RequireComponent(typeof(RectTransform))]
    public class WWButton : MonoBehaviour {

        // Lets the controller laser interact with buttons and use onClick function
        private BoxCollider boxCollider;
        private RectTransform rectTransform;
        private String metaData;                // Holds info we may need to get onClick

        private void OnEnable()
        {
            ValidateCollider();
        }

        private void OnValidate()
        {
            ValidateCollider();
        }

        private void ValidateCollider()
        {
            rectTransform = GetComponent<RectTransform>();

            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
            {
                boxCollider = gameObject.AddComponent<BoxCollider>();
            }

            boxCollider.size = rectTransform.sizeDelta;
        }

        /// <summary>
        ///     Get button's metadata
        /// </summary>
        /// <returns>Metadata string</returns>
        public String GetMetaData()
        {
            return metaData;
        }

        /// <summary>
        ///     Set button's metadata
        /// </summary>
        /// <param name="metadata">What string the metadata will be set to</param>
        public void SetMetadata(String metadata)
        {
            metaData = metadata;
        }
    }
}
