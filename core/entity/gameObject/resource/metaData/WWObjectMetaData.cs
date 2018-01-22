using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata required to describe a WWObject.
    /// </summary>
    [Serializable]
    public class WWObjectMetadata
    {
        [Range(1, 10000)] public int baseTileSize = 10; // What is scale at which this Object's art assset was built?
        public WWType type = WWType.Tile; // default to type to Tile
    }
}