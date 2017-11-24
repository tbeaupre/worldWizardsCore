using UnityEngine;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {
        public override void SetPosition(Coordinate coordinate)
        {
            var coordinateNoOffset = new Coordinate(coordinate.index, coordinate.rotation);
            base.SetPosition(coordinateNoOffset);
        }
    }
}