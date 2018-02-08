using System;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.entity.common
{
    /// <summary>
    ///     A enumeration of all possible World Wizards object types.
    /// </summary>
    public enum WWType
    {
        Tile,
        Prop,
        Interactable,
        Door,
        None
    }

    /// <summary>
    ///     A helper class containing a number of static functions to help with using the WWType enum.
    /// </summary>
    public static class WWTypeHelper
    {
        public static Type ConvertToSysType(WWType type)
        {
            switch (type)
            {
                case WWType.Interactable:
                    return typeof(Interactable);
                case WWType.Prop:
                    return typeof(Prop);
                case WWType.Tile:
                    return typeof(Tile);
                case WWType.Door:
                    return typeof(Door);
                default:
                    return typeof(Tile);
            }
        }
    }
}