using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;

namespace WorldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {
        public override void SetPosition(Vector3 position, bool snapToGrid)
        {
            if (snapToGrid)
            {
                position = CoordinateHelper.GetTileCenter(position);
            }
            base.SetPosition(position, snapToGrid);
        }

        protected override Vector3 GetPositionOffset()
        {
            return new Vector3(
                0,
                0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                0);
        }
    }
}