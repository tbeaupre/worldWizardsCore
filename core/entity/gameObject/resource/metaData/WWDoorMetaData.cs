using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWDoorMetaData
    {
//        public Vector3 pivotOffset;
        public Vector3 facingDirection;
        public float width;
        public float height;
        public Animation openAnimation;
        public Animation closeAnimation;
        public InteractionType interactionType;
    }
}