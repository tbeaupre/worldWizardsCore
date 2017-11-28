using System;
using UnityEngine;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public WWObjectMetaData wwObjectMetaData;
        public WWTileMetaData wwTileMetaData;
        public WWDoor door;
    }
}