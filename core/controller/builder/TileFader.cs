using System.Collections.Generic;
using UnityEngine;

namespace WorldWizards.core.controller.builder
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// TileFader maintains a list of materials for a GameObjects entire hierarchy
    /// and can toggle them with another material.
    /// </summary>
    public class TileFader
    {
        private readonly Material[] fadeMatList; // the material to toggle

        // handle materials for both mesh and skinned renderers
        private readonly List<Material[]> meshMaterials;
        private readonly MeshRenderer[] meshRenderers;
        private readonly List<Material[]> skinnedMaterials;
        private readonly SkinnedMeshRenderer[] skinnedRenderers;

        /// <summary>
        /// Constructor that searches the entire GameObject hierarchy and
        /// determines the original regular materials.
        /// </summary>
        /// <param name="gameObject"></param>
        public TileFader(GameObject gameObject)
        {
            fadeMatList = new Material[1];
            fadeMatList[0] = Resources.Load("Materials/TileFadeMat") as Material;
            meshMaterials = new List<Material[]>();
            skinnedMaterials = new List<Material[]>();

            meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer mesh in meshRenderers)
                meshMaterials.Add(mesh.materials);
            skinnedRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer skin in skinnedRenderers)
                skinnedMaterials.Add(skin.materials);
        }

        /// <summary>
        /// Turn the original regular materials on.
        /// </summary>
        public void On()
        {
            for (var i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].materials = meshMaterials[i];
            for (var i = 0; i < skinnedRenderers.Length; i++)
                skinnedRenderers[i].materials = skinnedMaterials[i];
        }

        /// <summary>
        /// Turn the materials off by setting materials to the faded material.
        /// </summary>
        public void Off()
        {
            for (var i = 0; i < meshRenderers.Length; i++)
                meshRenderers[i].materials = fadeMatList;
            for (var i = 0; i < skinnedRenderers.Length; i++)
                skinnedRenderers[i].materials = fadeMatList;
        }
    }
}