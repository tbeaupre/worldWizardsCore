using UnityEngine;

using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    public class Tile : WWObject
    {
		public override void SetPosition(Coordinate coordinate )
		{
			coordinate.offset = Vector3.zero;
			base.SetPosition(coordinate);
		}
    }
}
