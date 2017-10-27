using UnityEngine;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {
        public override void SetPosition(Coordinate coordinate)
        {
            coordinate.offset = Vector3.zero;
            base.SetPosition(coordinate);
        }
    }
}