﻿using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData
{
    [Serializable]
    public class WWObjectMetaData
    {
        public WWType type = WWType.Tile;
        [Range(1,10000)]
        public int baseTileSize = 10;
    }
}