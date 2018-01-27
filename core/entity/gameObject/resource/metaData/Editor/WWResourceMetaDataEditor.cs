using System;
using UnityEditor;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData.Editor
{
    // @author - Brian Keeley-DeBonis bjkeeleydebonis@wpi.edu
    /// <summary>
    /// This class is a UI helper that makes it easier for artists to setup assets in the Unity Inspector.
    /// </summary>
    [CustomEditor(typeof(WWResourceMetadata))]
    public class WWResourceMetadataEditor : UnityEditor.Editor
    {
        private static readonly string NORTH = "North";
        private static readonly string EAST = "East";
        private static readonly string SOUTH = "South";
        private static readonly string WEST = "West";
        private static readonly string TOP = "Top";
        private static readonly string BOTTOM = "Bottom";

        private static GameObject pivot;
        private static GameObject x1;
        private static GameObject x2;
        private static GameObject y;
        private static GameObject facingDirection;

        public override void OnInspectorGUI()
        {
            var script = target as WWResourceMetadata;
            script.wwObjectMetadata.type =
                (WWType) EditorGUILayout.EnumPopup("Asset Type", script.wwObjectMetadata.type);
            script.wwObjectMetadata.baseTileSize =
                EditorGUILayout.IntSlider("Base Tile Size", script.wwObjectMetadata.baseTileSize, 1, 10000);
            if (script.wwObjectMetadata.type == WWType.Tile)
            {
                DisplayCollisionsProperties(script);
                DisplayDoorHolderProperties(script.wwTileMetadata.northWwDoorHolderMetadata, NORTH,
                    script.wwObjectMetadata.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetadata.eastWwDoorHolderMetadata, EAST,
                    script.wwObjectMetadata.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetadata.southWwDoorHolderMetadata, SOUTH,
                    script.wwObjectMetadata.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetadata.westWwDoorHolderMetadata, WEST,
                    script.wwObjectMetadata.baseTileSize);
            }
            else if (script.wwObjectMetadata.type == WWType.Door)
            {
                DisplayDoorProperties(script.doorMetadata, script.wwObjectMetadata.baseTileSize);
            }
        }

        private void DisplayCollisionsProperties(WWResourceMetadata script)
        {
            GUILayout.Label("Walls");
            EditorGUILayout.BeginHorizontal();
            script.wwTileMetadata.wwWallMetadata.north = GUILayout.Toggle(
                script.wwTileMetadata.wwWallMetadata.north, "North");
            script.wwTileMetadata.wwWallMetadata.east = GUILayout.Toggle(
                script.wwTileMetadata.wwWallMetadata.east, "East");
            script.wwTileMetadata.wwWallMetadata.south = GUILayout.Toggle(
                script.wwTileMetadata.wwWallMetadata.south, "South");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            script.wwTileMetadata.wwWallMetadata.west = GUILayout.Toggle(
                script.wwTileMetadata.wwWallMetadata.west, "West");
            script.wwTileMetadata.wwWallMetadata.top = GUILayout.Toggle(
                script.wwTileMetadata.wwWallMetadata.top, "Top");
            script.wwTileMetadata.wwWallMetadata.bottom = GUILayout.Toggle(
                script.wwTileMetadata.wwWallMetadata.bottom, "Bottom");
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayDoorProperties(WWDoorMetadata doorMetadata, int baseTileSize)
        {
            doorMetadata.facingDirection =
                EditorGUILayout.Vector3Field("Door Facing Direction", doorMetadata.facingDirection);
            doorMetadata.width = EditorGUILayout.FloatField("Door width", doorMetadata.width);
            doorMetadata.height = EditorGUILayout.FloatField("Door Height", doorMetadata.height);

            doorMetadata.openAnimation = EditorGUILayout.ObjectField("Open Animation",
                doorMetadata.openAnimation, typeof(Animation), false) as Animation;
            doorMetadata.closeAnimation = EditorGUILayout.ObjectField("Close Animation",
                doorMetadata.closeAnimation, typeof(Animation), false) as Animation;
            if (GUILayout.Button("Create Helpers"))
            {
                CreateDoorHelpers();
            }
            if (GUILayout.Button("Get Door Dimensions and Pivot"))
            {
                GetDoorDimensions(doorMetadata, baseTileSize);
            }
        }

        private void DisplayDoorHolderProperties(WWDoorHolderMetadata wwDoorHolderMetadata, string direction,
            int baseTileSize)
        {
            wwDoorHolderMetadata.hasDoorHolder = GUILayout.Toggle(
                wwDoorHolderMetadata.hasDoorHolder, string.Format(" Has {0} Door Holder", direction));
            if (wwDoorHolderMetadata.hasDoorHolder)
            {
                wwDoorHolderMetadata.pivot = EditorGUILayout.Vector3Field(
                    string.Format("{0} Door Pivot", direction), wwDoorHolderMetadata.pivot);
                wwDoorHolderMetadata.width = EditorGUILayout.FloatField(
                    string.Format("{0} Door Width", direction), wwDoorHolderMetadata.width);
                wwDoorHolderMetadata.height = EditorGUILayout.FloatField(
                    string.Format("{0} Door Height", direction), wwDoorHolderMetadata.height);
                if (GUILayout.Button("Create Helpers"))
                {
                    CreateDoorHolderHelpers(direction);
                }
                if (GUILayout.Button(direction + "Get Door Dimensions and Pivot"))
                {
                    GetDoorHolderDimensions(wwDoorHolderMetadata, direction, baseTileSize);
                }
            }
        }

        private void CreateDoorHelpers()
        {
            var script = target as WWResourceMetadata;
            Vector3 spawnPos = script.transform.position;
            Vector3 widthOffset = Vector3.zero;
            Vector3 extents = CalculateLocalBounds(script.gameObject);

            if (extents.z > extents.x)
            {
                widthOffset = Vector3.forward;
            }
            else
            {
                widthOffset = Vector3.right;
            }
            DestroyHelpers();
            CreateDoorPrimitives(spawnPos, widthOffset, script.wwObjectMetadata.baseTileSize);
        }

        private void CreateDoorHolderHelpers(string direction)
        {
            var script = target as WWResourceMetadata;
            Vector3 spawnPos = script.transform.position;
            Vector3 widthOffset = Vector3.zero;

            spawnPos.y -= script.wwObjectMetadata.baseTileSize * 0.5f;
            if (direction.Equals(NORTH))
            {
                spawnPos.z += script.wwObjectMetadata.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(EAST))
            {
                spawnPos.x += script.wwObjectMetadata.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            else if (direction.Equals(SOUTH))
            {
                spawnPos.z -= script.wwObjectMetadata.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(WEST))
            {
                spawnPos.x -= script.wwObjectMetadata.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            DestroyHelpers();

            CreateDoorHolderPrimitives(spawnPos, widthOffset, script.wwObjectMetadata.baseTileSize);
        }

        private void DestroyHelpers()
        {
            if (pivot != null)
            {
                DestroyImmediate(pivot);
            }
            if (x1 != null)
            {
                DestroyImmediate(x1);
            }
            if (x2 != null)
            {
                DestroyImmediate(x2);
            }
            if (y != null)
            {
                DestroyImmediate(y);
            }
            if (facingDirection != null)
            {
                DestroyImmediate(facingDirection);
            }
        }

        private void CreateDoorPrimitives(Vector3 spawnPos, Vector3 widthOffset, int baseTileSize)
        {
            x1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x1.name = "x1";
            x1.transform.position = spawnPos + widthOffset * 0.3f * baseTileSize;
            x2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x2.name = "x2";
            x2.transform.position = spawnPos + widthOffset * 0.3f * baseTileSize * -1;
            y = GameObject.CreatePrimitive(PrimitiveType.Cube);
            y.name = "y";
            y.transform.position = new Vector3(spawnPos.x,
                spawnPos.y + baseTileSize * 0.75f,
                spawnPos.z);
            facingDirection = GameObject.CreatePrimitive(PrimitiveType.Cube);
            facingDirection.name = "facing direction";
            Vector3 facingVector = Vector3.forward;
            if (widthOffset.x < widthOffset.z)
            {
                facingVector = Vector3.right;
            }
            facingDirection.transform.position = spawnPos + facingVector;
        }

        private void CreateDoorHolderPrimitives(Vector3 spawnPos, Vector3 widthOffset, int baseTileSize)
        {
            pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pivot.name = "pivot";
            pivot.transform.position = spawnPos;
            x1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x1.name = "x1";
            x1.transform.position = spawnPos + widthOffset * 0.3f * baseTileSize;
            x2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x2.name = "x2";
            x2.transform.position = spawnPos + widthOffset * 0.3f * baseTileSize * -1;
            y = GameObject.CreatePrimitive(PrimitiveType.Cube);
            y.name = "y";
            y.transform.position = new Vector3(spawnPos.x,
                spawnPos.y + baseTileSize * 0.75f,
                spawnPos.z);
        }

        private void GetDoorHolderDimensions(WWDoorHolderMetadata wwDoorHolderMetadata, string direction,
            int baseTileSize)
        {
            if (DoorHolderHelpersAreNull())
            {
                return;
            }
            float width = Vector3.Distance(x1.transform.position, x2.transform.position) / baseTileSize;
            float height = Math.Abs(y.transform.position.y - x1.transform.position.y) / baseTileSize;
            wwDoorHolderMetadata.width = width;
            wwDoorHolderMetadata.height = height;
            wwDoorHolderMetadata.pivot = pivot.transform.position / baseTileSize * 2f;
            DestroyHelpers();
        }

        private void GetDoorDimensions(WWDoorMetadata doorMetadata, int baseTileSize)
        {
            if (DoorHelpersAreNull())
            {
                return;
            }
            float width = Vector3.Distance(x1.transform.position, x2.transform.position);
            float height = Math.Abs(y.transform.position.y - x1.transform.position.y);
            doorMetadata.width = width;
            doorMetadata.height = height;
            var script = target as WWResourceMetadata;
            doorMetadata.facingDirection = (facingDirection.transform.position - script.transform.position).normalized;
            DestroyHelpers();
        }

        private bool DoorHolderHelpersAreNull()
        {
            return x1 == null || x2 == null || y == null || pivot == null;
        }

        private bool DoorHelpersAreNull()
        {
            return x1 == null || x2 == null || y == null || facingDirection == null;
        }

        private Vector3 CalculateLocalBounds(GameObject gameObject)
        {
            var bounds = new Bounds(gameObject.transform.position, Vector3.zero);
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                bounds.Encapsulate(renderer.bounds);
            Vector3 localCenter = bounds.center - gameObject.transform.position;
            bounds.center = localCenter;
            return new Vector3(bounds.extents.x, bounds.extents.y, bounds.extents.z);
        }
    }
}