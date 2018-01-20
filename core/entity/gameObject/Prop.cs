using UnityEngine;

namespace WorldWizards.core.entity.gameObject
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    public class Prop : WWObject
    {
        protected override Vector3 GetPositionOffset()
        {
            return Vector3.zero;
        }
    }
}