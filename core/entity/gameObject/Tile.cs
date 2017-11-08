using UnityEngine;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {
        public override void SetPosition(Coordinate coordinate)
        {
            coordinate.SetOffset(Vector3.zero);
            base.SetPosition(coordinate);
        }
    }
}