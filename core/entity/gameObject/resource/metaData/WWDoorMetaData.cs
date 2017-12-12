using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWDoorMetaData
    {
        public Animation closeAnimation;

        public Vector3 facingDirection;
        public float height;
        public InteractionType interactionType;
        public Animation openAnimation;
        public float width;
    }
}