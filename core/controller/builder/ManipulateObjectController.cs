using System.Collections.Generic;
using UnityEngine;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.controller.builder
{
    /// <summary>
    ///     The ManipulateObjectController handles translation, rotation, and scaling events
    ///     for World Wizards Objects.
    /// </summary>
    public class ManipulateObjectController : MonoBehaviour
    {
        private List<WWObject> selection;

        public void OnTranslate()
        {
        }

        public void OnRotate()
        {
        }

        public void OnScale()
        {
        }
    }
}