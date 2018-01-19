using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.controller.builder
{
    public class SceneScalerController : MonoBehaviour
    {
        private static readonly float MIN_SCALE = 0.1f;
        private static readonly float MAX_SCALE = 2f;

        [SerializeField] public Slider _slider;

        /// <summary>
        /// Called from a UI Event. 
        /// </summary>
        public void OnSliderChange()
        {
            float newScale = _slider.value;
            CoordinateHelper.tileLengthScale = newScale;
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().ChangeScale(newScale);
        }
    }
}