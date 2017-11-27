using System;

namespace WorldWizards.core.entity.gameObject.resource
{
    [Serializable]
    public class WWTileMetaData
    {
        public WWOccupiedWalls wwOccupiedWalls;
        public WWDoorHolder northWwDoorHolder;
        public WWDoorHolder southWwDoorHolder;
        public WWDoorHolder eastWwDoorHolder;
        public WWDoorHolder westWwDoorHolder;
    }
}