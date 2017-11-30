using UnityEngine;
using WorldWizards.core.entity.coordinate;
using WorldWizards.core.entity.coordinate.utils;
using WorldWizards.core.manager;

namespace WorldWizards.core.controller.builder
{
    public class GridController : MonoBehaviour
    {
        [SerializeField]
        private GameObject grid;
        [SerializeField] 
        private GameObject playerReferenceScale;
        private int height;

        void Awake()
        {
            grid.transform.position = Vector3.zero;
            grid.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;

            playerReferenceScale.transform.position = Vector3.zero;
            playerReferenceScale.transform.localScale = Vector3.one;

        }

        void Update()
        {
            GetInput();   
        }
        
        private void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StepUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StepDown();
            }
        }

        public void MoveGrid()
        {
            float yPos = height * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale;
            Vector3 gridPosition = grid.transform.position;
            gridPosition.y = yPos;
            grid.transform.position = gridPosition;
            Coordinate c = CoordinateHelper.ConvertUnityCoordinateToWWCoordinate(grid.transform.position);
            ManagerRegistry.Instance.sceneGraphManager.HideObjectsAbove(c.index.y);
            
            // set the scale too
            grid.transform.localScale = Vector3.one * CoordinateHelper.tileLengthScale;
            playerReferenceScale.transform.position = new Vector3(0,
                yPos - (0.5f * CoordinateHelper.baseTileLength * CoordinateHelper.tileLengthScale),
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