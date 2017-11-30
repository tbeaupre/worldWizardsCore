using UnityEngine;
using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
{
    public class Interactable : WWObject
    {
        private InteractionType interactionType;
        
        protected override Vector3 GetPositionOffset()
        {
            return Vector3.zero;
        }
    }
}