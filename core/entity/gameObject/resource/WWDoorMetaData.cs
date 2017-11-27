using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource
{
    [Serializable]
    public class WWDoor
    {
        public Vector3 pivot;
        public Vector3 facingDirection;
        public float width;
        public float height;
        public Animation openAnimation;
        public Animation closeAnimation;
        public InteractionType interactionType;
    }
}