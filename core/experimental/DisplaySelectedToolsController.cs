using UnityEngine;
using UnityEngine.UI;
using worldWizards.core.input;

namespace WorldWizards.core.experimental
{
    public class DisplaySelectedToolsController : MonoBehaviour
    {
        private InputManager inputManager;
        [SerializeField] private Text toolNameText;

        void Awake()
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        void Update()
        {
            toolNameText.text = string.Format("Left: {0} | Right : {1}", inputManager.GetLeftToolName(),
                inputManager.GetRighttToolName());
        }
    }
}