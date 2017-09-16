using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using worldWizards.core.entity.common;
using UnityEngine;

namespace worldWizards.core.entity.gameObject
{
    [System.Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public bool north;
        public bool east;
        public bool south;
        public bool west;
        public bool top;
        public bool bottom;
        public WWType type;

        /// <summary>
        /// Serializable boolean values are used to make editing metadata in the inspector easier, but they must be converted to the enum bitflag for use.
        /// </summary>
        /// <returns>WWWalls equivalent to the serializable boolean values set in the inspector.</returns>
        private WWWalls GetWallsEnum()
        {
            WWWalls result = 0;

            if (north)
                result = result | WWWalls.North;
            if (east)
                result = result | WWWalls.East;
            if (south)
                result = result | WWWalls.South;
            if (west)
                result = result | WWWalls.West;
            if (top)
                result = result | WWWalls.Top;
            if (bottom)
                result = result | WWWalls.Bottom;

            return result;
        }
        
        public void GetData(ref WWWalls wallBarriers, ref WWType type)
        {
            wallBarriers = GetWallsEnum();
            type = this.type;
        }
    }
}
