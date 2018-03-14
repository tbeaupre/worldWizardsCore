using System.Collections.Generic;
using UnityEngine;

namespace WorldWizards.core.controller.builder
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// Material Switcher maintains a list of materials for a GameObject's entire hierarchy
    /// and allows toggling between the original list with another material. Material Switcher supports meshes
    /// with multiple materials and submeshes.
    /// </summary>
    public class MaterialSwitcher
    {
        // a list of material arrays. Each array is belongs to a renderer in the hierarchy.
        private readonly List<Material[]> originalMaterials;
        // the array of renderes in the hierarchy.
        private readonly Renderer[] renderers;   
        // same as original, except each material is the switch material.
        private readonly List<Material[]> switchMaterials;

        /// <summary>
        /// Get all of the renderers as a list.
        /// </summary>
        /// <returns></returns>
        public List<Renderer> GetAllRenderers()
        {
            var renderers = new List<Renderer>();
            foreach (var rend in this.renderers)
            {
                renderers.Add(rend);
            }
            return renderers;
        }

        /// <summary>
        /// Constructor that searches the entire GameObject hierarchy,
        /// stores the material arrays, and then constructs an equivalent array
        /// populated by the switchMaterial to support toggling.
        /// </summary>
        /// <param name="gameObject"></param>
        public MaterialSwitcher(GameObject gameObject, Material switchMaterial)
        {
            originalMaterials = new List<Material[]>(); // init
            switchMaterials = new List<Material[]>(); // init
            // only want to perform this operation once, so cache it
            renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                originalMaterials.Add(rend.materials);
                // a renderer can have just one or many originalMaterials
                var swithMatArray = new Material[rend.materials.Length];
                for (int i = 0; i < rend.materials.Length; i++)
                {
                    swithMatArray[i] = switchMaterial;
                }
                switchMaterials.Add(swithMatArray);
            }
        }

        /// <summary>
        /// Turn the original materials off by setting original materials to the switch material.
        /// </summary>
        public void SwitchOn()
        {
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = switchMaterials[i];
            }
        }

        /// <summary>
        /// Turn the original regular original materials back on.
        /// </summary>
        public void SwitchOff()
        {
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].materials = originalMaterials[i];
            }
        }
        
        
        /// <summary>
        /// Computes the center of the object hierarchy by averaging the bounding boxes.
        /// </summary>
        /// <returns>The center of the object hierarchy based on averaged bounding boxes.</returns>
        public Vector3 GetMeshCenter()
        {
            Vector3 result = Vector3.zero;
            var allRenderers = GetAllRenderers();
            foreach (var rend in allRenderers)
            {
                result += rend.bounds.center;
            }
            if (allRenderers.Count > 0) // prevent divide by 0
            {
                result = result / allRenderers.Count;
            }
            return result;
        }
    }
}