using System;
using UnityEngine;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// This component is attached to all WWObjects that are to be created and added to an 
    /// asset bundle. The artist, or person who sets up the prefab needs to configure 
    /// the properties. This component is composed of various pieces of metadata that are relevent to describing a WWObject.
    /// The metadata is subject to change and be extended as more features are added to World Wizards.
    /// WWResourceMetadata is composed of all possible types of metadata.
    /// </summary>
    [Serializable]
    public class WWResourceMetadata : MonoBehaviour
    {
        public WWDoorMetadata doorMetadata;
        public WWObjectMetadata wwObjectMetadata;
        public WWTileMetadata wwTileMetadata;
    }
}