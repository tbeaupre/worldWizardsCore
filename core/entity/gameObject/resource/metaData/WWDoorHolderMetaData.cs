using System;
using UnityEngine;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata necessary for describing a Door Holder.
    /// </summary>
    [Serializable]
    public class WWDoorHolderMetadata
    {
        public bool hasDoorHolder; // is this Door Holder available?
        public float height;
        public Vector3 pivot; // where the door should be placed
        public float width;
    }
}