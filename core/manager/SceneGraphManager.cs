using System;
using System.Collections.Generic;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;

namespace WorldWizards.core.manager
{
    /// <summary>
    /// </summary>
    public interface SceneGraphManager
    {
        /// <summary>
        ///     Determine how many WWObjects are in the scene graph
        /// </summary>
        /// <returns>The number if WWObjects in the scene graph.</returns>
        int SceneSize();

        bool Add(WWObject wwObject);

        void HideObjectsAbove(int height);

        WWWalls GetWallsAtCoordinate(Coordinate coordinate);

        /// <summary>
        ///     Gets the objects in the SceneGraph in the given coordinate index space.
        /// </summary>
        /// <returns>the objects in the SceneGraph in the given coordinate index space.</returns>
        /// <param name="coordinate">The coordinate to space to get.</param>
        List<WWObject> GetObjectsInCoordinateIndex(Coordinate coordinate);


        void Delete(Guid id);

        void Remove(Guid id);

        void Load();

        void Save();

        /// <summary>
        ///     Clears all the objects in the scene graph.
        /// </summary>
        void ClearAll();
    }
}