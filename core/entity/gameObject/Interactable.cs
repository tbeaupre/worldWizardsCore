using worldWizards.core.entity.common;
using worldWizards.core.entity.coordinate;

namespace worldWizards.core.entity.gameObject
{
    public class Interactable : WWObject
    {
        private InteractionType interactionType;


		public override void SetPosition(Coordinate coordinate)
		{
			base.SetPosition(coordinate);
		}
    }
}
