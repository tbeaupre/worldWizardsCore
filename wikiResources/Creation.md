This process is subject to change as development on World Wizards progresses.


To create a **tile** for World Wizards, simply add the **WWResourceMetaData** component to your tile prefab.

World Wizards currently expects all tiles to fit a 10x10 cube and have their pivot at the center of this 10x10 cube. Be sure to configure which walls should be navigable. World Space +Z is considered the North facing wall. To assist in properly setting up tiles, download and import the **TileSetupGuide** unitypackage which contains an model to make sure your tile's dimensions and walls are setup correctly. The TileSetupGuide is available in the wikiResources [folder](https://github.com/tbeaupre/worldWizardsCore/blob/master/wikiResources/) of this repository.

![](https://github.com/tbeaupre/worldWizardsCore/blob/master/wikiResources/TileGuide.png?raw=true "Optional Title")


Then add this tile prefab to your desired asset bundle and build the asset bundle using Unity's AssetBundle Browser.


The resulting asset bundle must be located in your Project/AssetBundle/ directory.
World Wizards currently only searches for asset bundles at this locatoin and does not look inside subfolders like the Windows or OSX folders.



## Using Your newly created Tile in World Wizards

Now that you have created your new World Wizards asset bundle, you probably want to use it in your World WIzards project.

First, create an empty GameObject in your Unity scene. Add the SceneGraphController component to this empty GameObject.
Then create the below script and attach it to your empty GameObject as well. Note, this script uses the **ww_basic_assets** Asset Bundle and instantiates the **tile_wallbrick** tile. Replaces these two strings in the Example script with your AssetBundle name and tile name. Or, you can use the ww_basic_assets available in this repo [folder](https://github.com/tbeaupre/worldWizardsCore/blob/master/wikiResources/) here by moving them to your Project/AssetBundles folder. 

## Create a small script to load your new tile.

    using UnityEngine;
    using WorldWizards.core.controller.level;
    using WorldWizards.core.controller.level.utils;
    using WorldWizards.core.entity.coordinate;
    
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
                // Get a reference to the SceneGraphController
                SceneGraphController sceneGraphController = FindObjectOfType<SceneGraphController>();
            
                // load all asset bundles located at Project/AssetBundles
                ResourceLoader.LoadResources();
                
                // create a coordinate to place the tile 
                var coordinate = new Coordinate(0,0,0);
                // create the data needed to instantiate this tile
                var tileData = WWObjectFactory.CreateNew(coordinate, string.Format("{0}_{1}", assetBundleName, assetName));
                // instantiate the tile in the world
                var tile = WWObjectFactory.Instantiate(tileData); 
                // add the newly created tile to the SceneGraph
                sceneGraphController.Add(tile);
            }  
        }
    }