using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.gameObject;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    /// <summary>
    ///     Example script for loading a tile from an asset bundle, instantiating it, and adding it to the SceneGraph.
    /// </summary>
    public class Example : MonoBehaviour
    {
        private readonly string assetBundleName = "ww_basic_assets"; // the exact name of the Asset Bundle
        private readonly string assetName = "tile_wallbrick"; // the exact name of the tile inside of the Asset Bundle

        private void Start()
        {
            // create a coordinate to place the tile 
            var coordinate = new Coordinate(0, 0, 0);
            // create the data needed to instantiate this tile
            WWObjectData tileData =
                WWObjectFactory.CreateNew(coordinate, string.Format("{0}_{1}", assetBundleName, assetName));
            // instantiate the tile in the world
            WWObject tile = WWObjectFactory.Instantiate(tileData);
            // add the newly created tile to the SceneGraph
            ManagerRegistry.Instance.sceneGraphManager.Add(tile);
        }
    }
}