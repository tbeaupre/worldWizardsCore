using System;
using UnityEditor;
using UnityEngine;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource.metaData.Editor
{
    /// <summary>
    ///     This class is a UI helper that makes it easier for artists to setup assets in the Unity Inspector.
    /// </summary>
    // This if UNITY_EDITOR is probably not necassary, just here to prevent issues with asset bundle building.
    #if UNITY_EDITOR
    [CustomEditor(typeof(WWResourceMetaData))]
    public class WWResourceMetaDataEditor : UnityEditor.Editor
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
            var script = target as WWResourceMetaData;
            script.wwObjectMetaData.type =
                (WWType) EditorGUILayout.EnumPopup("Asset Type", script.wwObjectMetaData.type);
            script.wwObjectMetaData.baseTileSize =
                EditorGUILayout.IntSlider("Base Tile Size", script.wwObjectMetaData.baseTileSize, 1, 10000);
            if (script.wwObjectMetaData.type == WWType.Tile)
            {
                DisplayCollisionsProperties(script);
                DisplayDoorHolderProperties(script.wwTileMetaData.northWwDoorHolder, NORTH,
                    script.wwObjectMetaData.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetaData.eastWwDoorHolder, EAST,
                    script.wwObjectMetaData.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetaData.southWwDoorHolder, SOUTH,
                    script.wwObjectMetaData.baseTileSize);
                DisplayDoorHolderProperties(script.wwTileMetaData.westWwDoorHolder, WEST,
                    script.wwObjectMetaData.baseTileSize);
            }
            else if (script.wwObjectMetaData.type == WWType.Door)
            {
                DisplayDoorProperties(script.door, script.wwObjectMetaData.baseTileSize);
            }
        }

        private void DisplayCollisionsProperties(WWResourceMetaData script)
        {
            GUILayout.Label("Walls");
            EditorGUILayout.BeginHorizontal();
            script.wwTileMetaData.wwWallMetaData.north = GUILayout.Toggle(
                script.wwTileMetaData.wwWallMetaData.north, "North");
            script.wwTileMetaData.wwWallMetaData.east = GUILayout.Toggle(
                script.wwTileMetaData.wwWallMetaData.east, "East");
            script.wwTileMetaData.wwWallMetaData.south = GUILayout.Toggle(
                script.wwTileMetaData.wwWallMetaData.south, "South");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            script.wwTileMetaData.wwWallMetaData.west = GUILayout.Toggle(
                script.wwTileMetaData.wwWallMetaData.west, "West");
            script.wwTileMetaData.wwWallMetaData.top = GUILayout.Toggle(
                script.wwTileMetaData.wwWallMetaData.top, "Top");
            script.wwTileMetaData.wwWallMetaData.bottom = GUILayout.Toggle(
                script.wwTileMetaData.wwWallMetaData.bottom, "Bottom");
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayDoorProperties(WWDoor door, int baseTileSize)
        {
//            door.pivotOffset = EditorGUILayout.Vector3Field("Door Pivot Offset", door.pivotOffset);
            door.facingDirection = EditorGUILayout.Vector3Field("Door Facing Direction", door.facingDirection);
            door.width = EditorGUILayout.FloatField("Door width", door.width);
            door.height = EditorGUILayout.FloatField("Door Height", door.height);

            door.openAnimation = EditorGUILayout.ObjectField("Open Animation",
                door.openAnimation, typeof(Animation), false) as Animation;
            door.closeAnimation = EditorGUILayout.ObjectField("Close Animation",
                door.closeAnimation, typeof(Animation), false) as Animation;
            if (GUILayout.Button("Create Helpers"))
            {
                CreateDoorHelpers();
            }
            if (GUILayout.Button("Get Door Dimensions and Pivot"))
            {
                GetDoorDimensions(door, baseTileSize);
            }
        }

        private void DisplayDoorHolderProperties(WWDoorHolder wwDoorHolder, string direction, int baseTileSize)
        {
            wwDoorHolder.hasDoorHolder = GUILayout.Toggle(
                wwDoorHolder.hasDoorHolder, string.Format(" Has {0} Door Holder", direction));
            if (wwDoorHolder.hasDoorHolder)
            {
                wwDoorHolder.pivot = EditorGUILayout.Vector3Field(
                    string.Format("{0} Door Pivot", direction), wwDoorHolder.pivot);
                wwDoorHolder.width = EditorGUILayout.FloatField(
                    string.Format("{0} Door Width", direction), wwDoorHolder.width);
                wwDoorHolder.height = EditorGUILayout.FloatField(
                    string.Format("{0} Door Height", direction), wwDoorHolder.height);
                if (GUILayout.Button("Create Helpers"))
                {
                    CreateDoorHolderHelpers(direction);
                }
                if (GUILayout.Button(direction + "Get Door Dimensions and Pivot"))
                {
                    GetDoorHolderDimensions(wwDoorHolder, direction, baseTileSize);
                }
            }
        }

        private void CreateDoorHelpers()
        {
            var script = target as WWResourceMetaData;
            Vector3 spawnPos = script.transform.position;
            Vector3 widthOffset = Vector3.zero;
            Vector3 extents = CalculateLocalBounds(script.gameObject);
//            spawnPos.y -= script.wwObjectMetaData.baseTileSize * 0.5f;

            if (extents.z > extents.x)
            {
//                spawnPos.x += script.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.forward;
            }
            else
            {
//                spawnPos.z += script.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            DestroyHelpers();
            CreateDoorPrimitives(spawnPos, widthOffset, script.wwObjectMetaData.baseTileSize);
        }

        private void CreateDoorHolderHelpers(string direction)
        {
            var script = target as WWResourceMetaData;
            Vector3 spawnPos = script.transform.position;
            Vector3 widthOffset = Vector3.zero;

            spawnPos.y -= script.wwObjectMetaData.baseTileSize * 0.5f;
            if (direction.Equals(NORTH))
            {
                spawnPos.z += script.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(EAST))
            {
                spawnPos.x += script.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            else if (direction.Equals(SOUTH))
            {
                spawnPos.z -= script.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(WEST))
            {
                spawnPos.x -= script.wwObjectMetaData.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            DestroyHelpers();

            CreateDoorHolderPrimitives(spawnPos, widthOffset, script.wwObjectMetaData.baseTileSize);
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

        private void GetDoorHolderDimensions(WWDoorHolder wwDoorHolder, string direction, int baseTileSize)
        {
            if (DoorHolderHelpersAreNull())
            {
                return;
            }
            float width = Vector3.Distance(x1.transform.position, x2.transform.position) / baseTileSize;
            float height = Math.Abs(y.transform.position.y - x1.transform.position.y) / baseTileSize;
            wwDoorHolder.width = width;
            wwDoorHolder.height = height;
            wwDoorHolder.pivot = pivot.transform.position / baseTileSize;
            DestroyHelpers();
        }

        private void GetDoorDimensions(WWDoor door, int baseTileSize)
        {
            if (DoorHelpersAreNull())
            {
                return;
            }
            float width = Vector3.Distance(x1.transform.position, x2.transform.position);
            float height = Math.Abs(y.transform.position.y - x1.transform.position.y);
            door.width = width;
            door.height = height;
            door.facingDirection = (facingDirection.transform.position - pivot.transform.position).normalized;
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
    #endif
}