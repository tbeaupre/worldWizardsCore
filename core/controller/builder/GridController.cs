using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.controller.builder
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// A simple controller for scaling and moving a Grid Collider up and down in the Builder.
    /// </summary>
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GameObject grid;
        private int height;
        private GameObject playerReferenceScale;

        /// <summary>
        /// Basic setup
        /// </summary>
        private void Awake()
        {
            grid.transform.position = Vector3.zero;
            grid.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            playerReferenceScale = Instantiate(Resources.Load("Prefabs/PlayerScale")) as GameObject;

            playerReferenceScale.transform.position = Vector3.zero;
            playerReferenceScale.transform.localScale = Vector3.one;
        }

        /// <summary>
        /// Get the grid collider.
        /// </summary>
        /// <returns> The Grid's Collider</returns>
        public Collider GetGridCollider()
        {
            return GetComponent<Collider>();
        }

        /// <summary>
        /// Refresh the grid position and scale.
        /// </summary>
        public void RefreshGrid()
        {
            float yPos = height * CoordinateHelper.GetTileScale();
            Vector3 gridPosition = grid.transform.position;
            gridPosition.y = yPos;
            grid.transform.position = gridPosition;
            Coordinate c = CoordinateHelper.UnityCoordToWWCoord(grid.transform.position, 0);
            ManagerRegistry.Instance.GetAnInstance<SceneGraphManager>().HideObjectsAbove(c.Index.y);            

            // set the scale too
            grid.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            playerReferenceScale.transform.position = new Vector3(0,
                yPos - 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                0);
        }

        /// <summary>
        /// Move the grid up one step.
        /// </summary>
        public void StepUp()
        {
            height++;
            RefreshGrid();
        }

        /// <summary>
        /// Move the grid down one step.
        /// </summary>
        public void StepDown()
        {
            height--;
            RefreshGrid();
        }
    }
}