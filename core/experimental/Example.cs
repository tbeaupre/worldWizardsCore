using UnityEngine;
using WorldWizards.core.controller.level;
using WorldWizards.core.controller.level.utils;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.manager;

namespace WorldWizards.core.experimental
{
    /// <summary>
    /// Example script for loading a tile from an asset bundle, instantiating it, and adding it to the SceneGraph.
    /// </summary>
    public class Example : MonoBehaviour
    {
        
        string assetBundleName = "ww_basic_assets"; // the exact name of the Asset Bundle
        string assetName = "tile_wallbrick"; // the exact name of the tile inside of the Asset Bundle

        void Start()
        {        
            // load all asset bundles located at Project/AssetBundles
            ResourceLoader.LoadResources();
            
            // create a coordinate to place the tile 
            var coordinate = new Coordinate(0,0,0);
            // create the data needed to instantiate this tile
            var tileData = WWObjectFactory.CreateNew(coordinate, string.Format("{0}_{1}", assetBundleName, assetName));
            // instantiate the tile in the world
            var tile = WWObjectFactory.Instantiate(tileData); 
            // add the newly created tile to the SceneGraph
            ManagerRegistry.Instance.sceneGraphImpl.Add(tile);
        }  
    }
}