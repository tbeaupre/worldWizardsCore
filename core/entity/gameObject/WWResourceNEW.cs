using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace worldWizards.core.entity.gameObject
{
    public abstract class WWResourceNEW : ScriptableObject
    {
        public virtual void LoadMetaData(WWResourceMetaData instanceData) { }
    }
}
