using System;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource
{
    [Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public bool bottom;
        public bool east;
        public bool north;
        public bool south;
        public bool top;
        public WWType type;
        public bool west;

        public WWResourceMetaData()
        {
            north = false;
            east = false;
            south = false;
            west = false;
            top = false;
            bottom = false;
            type = WWType.Tile;
        }

        /// <summary>
        ///     Serializable boolean values are used to make editing metadata in the inspector easier, but they must be converted
        ///     to the enum bitflag for use.
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
        //        }
        //            type = this.type;
        //            wallBarriers = GetWallsEnum();
        //        {


        //        public void GetData(ref WWWalls wallBarriers, ref WWType type)
    }
}