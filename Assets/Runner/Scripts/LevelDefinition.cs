using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A scriptable object that stores all information
    /// needed to load and set up a Runner level.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "Runner/LevelDefinition", order = 1)]
    public class LevelDefinition : AbstractLevelData
    {
        /// <summary>
        /// The Length of the level.
        /// </summary>
        public float LevelLength = 100.0f;

        /// <summary>
        /// The amount of extra terrain to be added before the start of the level.
        /// </summary>
        public float LevelLengthBufferStart = 5.0f;

        /// <summary>
        /// The amount of extra terrain to be added after the end of the level.
        /// </summary>
        public float LevelLengthBufferEnd = 5.0f;

        /// <summary>
        /// The width of the level.
        /// </summary>
        public float LevelWidth = 5.0f;

        /// <summary>
        /// The thickness of the level.
        /// </summary>
        public float LevelThickness = 0.1f;

        /// <summary>
        /// True means that spawnables should snap to a grid in this level.
        /// </summary>
        public bool SnapToGrid = true;

        /// <summary>
        /// The size of the grid that spawnables will snap to if SnapToGrid 
        /// is true.
        /// </summary>
        public float GridSize = 1.0f;

        /// <summary>
        /// The material applied to the generated terrain for this level.
        /// </summary>
        public Material TerrainMaterial;

        /// <summary>
        /// A prefab placed at the start point of this level.
        /// </summary>
        public GameObject StartPrefab;

        /// <summary>
        /// A prefab placed at the end of this level. This prefab should 
        /// contain collision logic to complete the level.
        /// </summary>
        public GameObject EndPrefab;

        /// <summary>
        /// An array of all SpawnableObjects that exist in this level.
        /// </summary>
        public SpawnableObject[] Spawnables;

        [System.Serializable]
        public class SpawnableObject
        {
            /// <summary>
            /// The prefab spawned by this SpawnableObject.
            /// </summary>
            public GameObject SpawnablePrefab;

            /// <summary>
            /// The world position of this SpawnableObject.
            /// </summary>
            public Vector3 Position = Vector3.zero;

            /// <summary>
            /// The rotational euler angles of this SpawnableObject.
            /// </summary>
            public Vector3 EulerAngles = Vector3.zero;

            /// <summary>
            /// The world scale of this SpawnableObject.
            /// </summary>
            public Vector3 Scale = Vector3.one;

            /// <summary>
            /// The base color to be applied to the materials on 
            /// this SpawnableObject.
            /// </summary>
            public Color BaseColor = Color.white;

            /// <summary>
            /// True if this object should snap to a levels grid. 
            /// Setting this value to false will make this SpawnableObject
            /// ignore the level's snap settings.
            /// </summary>
            public bool SnapToGrid = true;
        }

        /// <summary>
        /// Store all values from updatedLevel into this LevelDefinition.
        /// </summary>
        /// <param name="updatedLevel">
        /// The LevelDefinition that has been edited in the Runner Level Editor Window.
        /// </param>
        /// <summary>
        /// Validates level parameters to ensure they are within acceptable ranges
        /// </summary>
        /// <returns>True if all parameters are valid, false otherwise</returns>
        private bool ValidateLevelParameters()
        {
            if (LevelLength <= 0 || LevelWidth <= 0 || LevelThickness <= 0)
                return false;
            if (LevelLengthBufferStart < 0 || LevelLengthBufferEnd < 0)
                return false;
            if (GridSize <= 0)
                return false;
            if (TerrainMaterial == null || StartPrefab == null || EndPrefab == null)
                return false;
            return true;
        }

        /// <summary>
        /// Store all values from updatedLevel into this LevelDefinition with validation.
        /// </summary>
        /// <param name="updatedLevel">The LevelDefinition that has been edited in the Runner Level Editor Window.</param>
        /// <returns>True if values were successfully saved, false if validation failed</returns>
        public bool SaveValues(LevelDefinition updatedLevel)
        {
            if (updatedLevel == null)
                return false;

            LevelLength = Mathf.Max(0.1f, updatedLevel.LevelLength);
            LevelLengthBufferStart = Mathf.Max(0, updatedLevel.LevelLengthBufferStart);
            LevelLengthBufferEnd = Mathf.Max(0, updatedLevel.LevelLengthBufferEnd);
            LevelWidth = Mathf.Max(0.1f, updatedLevel.LevelWidth);
            LevelThickness = Mathf.Max(0.01f, updatedLevel.LevelThickness);
            SnapToGrid = updatedLevel.SnapToGrid;
            GridSize = Mathf.Max(0.1f, updatedLevel.GridSize);
            TerrainMaterial = updatedLevel.TerrainMaterial;
            StartPrefab = updatedLevel.StartPrefab;
            EndPrefab = updatedLevel.EndPrefab;
            Spawnables = updatedLevel.Spawnables;

            return ValidateLevelParameters();
        }
    }
}