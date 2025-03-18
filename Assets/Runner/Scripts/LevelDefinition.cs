using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    [CreateAssetMenu(fileName = "Data", menuName = "Runner/LevelData", order = 1)]
    public class LevelData : AbstractLevelData
    {
        public float Length = 100.0f;
        public float StartBuffer = 5.0f;
        public float EndBuffer = 5.0f;
        public float Width = 5.0f;
        public float Thickness = 0.1f;
        public bool SnapToGrid = true;
        public float GridSize = 1.0f;
        public Material TerrainMaterial;
        public GameObject StartMarkerPrefab;
        public GameObject EndMarkerPrefab;
        public SpawnableData[] Entities;

        [System.Serializable]
        public class SpawnableData
        {
            public GameObject EntityPrefab;
            public Vector3 Position = Vector3.zero;
            public Vector3 EulerAngles = Vector3.zero;
            public Vector3 Scale = Vector3.one;
            public Color BaseColor = Color.white;
            public bool SnapToGrid = true;
        }

        public void UpdateValues(LevelData updatedData)
        {
            Length = updatedData.Length;
            StartBuffer = updatedData.StartBuffer;
            EndBuffer = updatedData.EndBuffer;
            Width = updatedData.Width;
            Thickness = updatedData.Thickness;
            SnapToGrid = updatedData.SnapToGrid;
            GridSize = updatedData.GridSize;
            TerrainMaterial = updatedData.TerrainMaterial;
            StartMarkerPrefab = updatedData.StartMarkerPrefab;
            EndMarkerPrefab = updatedData.EndMarkerPrefab;
            Entities = updatedData.Entities;
        }
    }
}