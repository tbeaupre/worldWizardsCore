using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldWizards.core.entity.gameObject;

namespace worldWizards.core
{
    public class TestResource : WWResourceNEW
    {
        public override void Update(WWResourceMetaData instanceData)
        {
            // Making a hallway.
            instanceData.path = "tileTemp";
            instanceData.wallBarriers = WWWalls.Bottom | WWWalls.East | WWWalls.West | WWWalls.Top;
        }
    }
}
