using System;
using UnityEngine;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public WWDoorMetaData doorMetaData;
        public WWObjectMetaData wwObjectMetaData;
        public WWTileMetaData wwTileMetaData;
    }
}