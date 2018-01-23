using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Metadata required for describing a Door.
    /// </summary>
    [Serializable]
    public class WWDoorMetadata
    {
        public Vector3 facingDirection; // the direction the door faces to open
        public float height;
        public float width;
        public InteractionType interactionType;
        public Animation closeAnimation;
        public Animation openAnimation;
    }
}