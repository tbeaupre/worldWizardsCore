using UnityEngine;
using UnityEngine.UI;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.controller.builder
{
    public class SceneScalerController : MonoBehaviour
    {
        [SerializeField]
        public Slider _slider;
        
        public static readonly float MIN_SCALE = 0.25f;
        public static readonly float MAX_SCALE = 4f;

        public void Awake()
        {
            _slider.minValue = MIN_SCALE;
            _slider.maxValue = MAX_SCALE;
        }

        public void OnSliderChange()
        {
            var newScale = _slider.value;
            CoordinateHelper.tileLengthScale = newScale;
            ManagerRegistry.Instance.sceneGraphManager.ChangeScale(newScale);
        }
    }
}