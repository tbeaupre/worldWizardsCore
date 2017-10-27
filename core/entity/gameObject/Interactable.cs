using WorldWizards.core.entity.common;
using WorldWizards.core.entity.coordinate;

namespace WorldWizards.core.entity.gameObject
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