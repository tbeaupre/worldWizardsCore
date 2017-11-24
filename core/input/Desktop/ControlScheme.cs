using UnityEngine;

namespace worldWizards.core.input.Desktop
{
    // Associates keyboard keys with the buttons on a VR Controller.
    public struct ControlScheme
    {
        public readonly KeyCode triggerKey;
        public readonly KeyCode gripKey;
        public readonly KeyCode menuKey;
        public readonly KeyCode upKey;
        public readonly KeyCode downKey;
        public readonly KeyCode leftKey;
        public readonly KeyCode rightKey;
        public readonly KeyCode pressModKey;

        public ControlScheme(KeyCode newTriggerKey, KeyCode newGripKey, KeyCode newMenuKey,
            KeyCode newUpKey, KeyCode newDownKey, KeyCode newLeftKey, KeyCode newRightKey, KeyCode newPressModKey)
        {
            triggerKey = newTriggerKey;
            gripKey = newGripKey;
            menuKey = newMenuKey;
            upKey = newUpKey;
            downKey = newDownKey;
            leftKey = newLeftKey;
            rightKey = newRightKey;
            pressModKey = newPressModKey;
        }
    }
}