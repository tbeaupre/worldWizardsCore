using UnityEngine;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
{
    public class Prop : WWObject
    {
        protected override Vector3 GetPositionOffset()
        {
            return Vector3.zero;
        }
    }
}