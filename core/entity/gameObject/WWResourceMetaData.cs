using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace worldWizards.core.entity.gameObject
{
    [System.Serializable]
    public class WWResourceMetaData
    {
        public string path = "";
        public WWWalls wallBarriers = WWWalls.Bottom;
    }
}
