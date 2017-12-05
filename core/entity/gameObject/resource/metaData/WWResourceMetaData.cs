using System;
using UnityEngine;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    /// <summary>
    /// This component is attached to all WWObjects that are to be created and added to an 
    /// asset bundle. The artist, or person  who sets up the prefab needs to configure 
    /// the properties. This component is composed of various pieces of meta data that are relevent.
    /// The meta data is subject to change and be extended as more features are added to World Wizards.
    /// </summary>
    [Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public WWDoorMetaData doorMetaData;
        public WWObjectMetaData wwObjectMetaData;
        public WWTileMetaData wwTileMetaData;
    }
}