using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.controller.builder
{
    public class GridController : MonoBehaviour
    {
        private int height;

        private GameObject playerReferenceScale;

        private void Awake()
        {
            height = -1;
            gameObject.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            playerReferenceScale = Instantiate(Resources.Load("Prefabs/PlayerScale")) as GameObject;

            playerReferenceScale.transform.localScale = Vector3.one;
            playerReferenceScale.transform.position = new Vector3(0,
                (height - 0.5f) * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                0);
        }

        public void MoveGrid()
        {
            float yPos = height * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            
            Vector3 gridPosition = gameObject.transform.position;
            gridPosition.y = yPos;
            gameObject.transform.position = gridPosition;
            
            Coordinate c = CoordinateHelper.UnityCoordToWWCoord(gameObject.transform.position, 0);
            ManagerRegistry.Instance.sceneGraphManager.HideObjectsAbove(c.Index.y);

            // set the scale too
            gameObject.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            playerReferenceScale.transform.position = new Vector3(0,
                yPos - 0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale,
                0);
        }

        public void StepUp()
        {
            height++;
            MoveGrid();
        }

        public void StepDown()
        {
            height--;
            MoveGrid();
        }
    }
}