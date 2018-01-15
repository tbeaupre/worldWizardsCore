using System;
using UnityEngine;

namespace worldWizardsCore.core.menus
{
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

        public String GetMetaData()
        {
            return metaData;
        }

        public void SetMetadata(String metadata)
        {
            metaData = metadata;
        }
    }
}
