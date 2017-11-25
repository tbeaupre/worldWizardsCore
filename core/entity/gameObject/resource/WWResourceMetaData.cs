using System;
using System.CodeDom;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using WorldWizards.core.entity.common;

namespace WorldWizards.core.entity.gameObject.resource
{
    [Serializable]
    public class WWResourceMetaData : MonoBehaviour
    {
        public WWType type;
        public WWCollisions wwCollisions;

        public int baseTileSize = 10;
        
        [HideInInspector]
        public DoorHolder northDoorHolder;
        [HideInInspector]
        public DoorHolder southDoorHolder;
        [HideInInspector]
        public DoorHolder eastDoorHolder;
        [HideInInspector]
        public DoorHolder westDoorHolder;
    }
    
    
    /// <summary>
    /// This class is a UI helper that makes it easier for artists to setup the doors in the Unity Inspector.
    /// </summary>
    [CustomEditor(typeof(WWResourceMetaData))]
    public class WWResourceMetaDataEditor : Editor
    {
        private readonly string NORTH = "North";
        private readonly string EAST = "East";
        private readonly string SOUTH = "South";
        private readonly string WEST = "West";
        
        private GameObject pivot;
        private GameObject x1;
        private GameObject x2;
        private GameObject y;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector ();
            WWResourceMetaData wwResourceMetaDataScript = target as WWResourceMetaData;

            paintDoor( wwResourceMetaDataScript.northDoorHolder, NORTH, wwResourceMetaDataScript.baseTileSize);
            paintDoor(wwResourceMetaDataScript.eastDoorHolder, EAST,wwResourceMetaDataScript.baseTileSize);
            paintDoor( wwResourceMetaDataScript.southDoorHolder, SOUTH, wwResourceMetaDataScript.baseTileSize);
            paintDoor(wwResourceMetaDataScript.westDoorHolder, WEST, wwResourceMetaDataScript.baseTileSize);
        }

        private void paintDoor(DoorHolder doorHolder, String direction, int baseTileSize)
        {
            doorHolder.hasDoorHolder = GUILayout.Toggle(
                doorHolder.hasDoorHolder, string.Format(" Has {0} Door Holder", direction));
            if (doorHolder.hasDoorHolder)
            {
                doorHolder.pivot = EditorGUILayout.Vector3Field(
                    string.Format("{0} Door Pivot", direction), doorHolder.pivot);
                doorHolder.width = EditorGUILayout.FloatField(
                    string.Format("{0} Door Width", direction), doorHolder.width);
                doorHolder.height = EditorGUILayout.FloatField(
                    string.Format("{0} Door Height", direction), doorHolder.height);
                if (GUILayout.Button("Create Helpers"))
                {
                    CreateHelpers(direction);
                }
                if (GUILayout.Button(direction +  "Get Door Dimensions and Pivot"))
                {
                    GetDoorDimensions(doorHolder, direction, baseTileSize);
                }
            }
        }

        private void CreateHelpers(string direction)
        {
            WWResourceMetaData wwResourceMetaDataScript = target as WWResourceMetaData;
            Vector3 spawnPos = wwResourceMetaDataScript.transform.position;
            Vector3 widthOffset = Vector3.zero;
            
            spawnPos.y -= wwResourceMetaDataScript.baseTileSize * 0.5f;
            if (direction.Equals(NORTH))
            {
                spawnPos.z += wwResourceMetaDataScript.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(EAST))
            {
                spawnPos.x +=  wwResourceMetaDataScript.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
            else if (direction.Equals(SOUTH))
            {
                spawnPos.z -=  wwResourceMetaDataScript.baseTileSize * 0.5f;
                widthOffset = Vector3.right;
            }
            else if (direction.Equals(WEST))
            {
                spawnPos.x -=  wwResourceMetaDataScript.baseTileSize * 0.5f;
                widthOffset = Vector3.fwd;
            }
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
            pivot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pivot.name = "pivot";
            pivot.transform.position = spawnPos;
            x1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x1.name = "x1";
            x1.transform.position = spawnPos + (widthOffset * 0.3f * wwResourceMetaDataScript.baseTileSize);
            x2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x2.name = "x2";
            x2.transform.position = spawnPos + (widthOffset * 0.3f * wwResourceMetaDataScript.baseTileSize * -1);
            y = GameObject.CreatePrimitive(PrimitiveType.Cube);
            y.name = "y";
            y.transform.position = new Vector3(spawnPos.x,
                spawnPos.y +wwResourceMetaDataScript.baseTileSize * 0.75f,
                spawnPos.z);
        }

        private void GetDoorDimensions(DoorHolder doorHolder, string direction, int baseTileSize)
        {
            // return if any of the helpers are null
            if (x1 == null || x2 == null || y == null || pivot == null)
            {
                return;
            }
            var width = Vector3.Distance(x1.transform.position, x2.transform.position) / baseTileSize;
            var height = Math.Abs(y.transform.position.y - x1.transform.position.y) / baseTileSize;
            doorHolder.width = width ;
            doorHolder.height = height;
            doorHolder.pivot = pivot.transform.position / baseTileSize;
            DestroyImmediate(pivot);
            DestroyImmediate(x1);
            DestroyImmediate(x2);
            DestroyImmediate(y);
        }
    }
}