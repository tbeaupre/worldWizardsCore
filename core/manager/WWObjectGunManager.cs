using System.Collections.Generic;
using WorldWizards.core.controller.level;
using WorldWizards.core.entity.common;

namespace worldWizardsCore.core.manager
{
    /// <summary>
    ///     Controls what is in the object gun when the player is placing new objects in the scene
    ///     Allows for filters to be added based on WWType and/or asset bundle
    ///     AssetBundleMenu sets the values
    ///     Default: All objects in "ww_basic_assets"
    /// </summary>
    
    public class WWObjectGunManager : Manager
    {
        private List<string> possibleObjects;
        private string currentAssetBundle = "ww_basic_assets";
        private bool doFilter;
        private WWType filterType;

        /// <summary>
        ///     Set the asset bundle for use in object gun
        /// </summary>
        /// <param name="assetBundleTag">The asset bundle chosen</param>
        public void SetCurrentAssetBundle(string assetBundleTag)
        {
            currentAssetBundle = assetBundleTag;
        }

        /// <summary>
        ///     Sets whether the object gun should use filtering or not
        /// </summary>
        /// <param name="filter">True if filtering, false if no filtering</param>
        public void SetDoFilter(bool filter)
        {
            doFilter = filter;
        }

        /// <summary>
        ///     Gets whether the object gun should be filtered
        /// </summary>
        /// <returns>True if filtering, false if not filtering</returns>
        public bool GetDoFilter()
        {
            return doFilter;
        }

        /// <summary>
        ///     Set the type of object we are filtering for
        /// </summary>
        /// <param name="type">The type of object we want to filter for</param>
        public void SetFilterType(WWType type)
        {
            filterType = type;
        }

        /// <summary>
        ///     Get the type of object we are filtering for
        /// </summary>
        /// <returns>The type of object we are filtering for</returns>
        public WWType GetFilterType()
        {
            return filterType;
        }
        
        /// <summary>
        ///     Get possible objects for object gun based on filter and selected asset bundle
        /// </summary>
        /// <returns>List of all objects that should be in object gun</returns>
        public List<string> GetPossibleObjectKeys()
        {
            if (doFilter)
            {
                possibleObjects = WWResourceController.GetResourceKeysByAssetBundleFiltered(currentAssetBundle, filterType);
            }
            else
            {
                possibleObjects = WWResourceController.GetResourceKeysByAssetBundle(currentAssetBundle);
            }

            return possibleObjects;
        }
    }
}