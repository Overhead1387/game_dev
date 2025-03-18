using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HyperCasual.Runner
{
public class PlayerController : MonoBehaviour
    {
        private void Awake()
        {
            Debug.LogWarning("PlayerController is deprecated. Please use Player instead.");
            if (Player.Instance == null)
            {
                Debug.LogError("Player instance not found!");
            }
        }

        public static PlayerController Instance => Player.Instance as PlayerController;


        public float Speed => Player.Instance.CurrentSpeed;
    }

    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private AbstractGameEvent _victoryEvent;
        [SerializeField] private AbstractGameEvent _defeatEvent;

        private LevelData _currentLevelData;
        private bool _isGameActive;
        private GameObject _currentLevelContainer;
        private GameObject _currentTerrainContainer;
        private GameObject _levelMarkersContainer;
        private readonly List<SpawnableEntity> _activeEntities = new List<SpawnableEntity>();

#if UNITY_EDITOR
        private bool _isEditorMode;
#endif

        private void Awake()
        {
            Instance ??= this;
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

#if UNITY_EDITOR
            if (LevelLoader.Instance != null)
            {
                StartGame();
                _isEditorMode = true;
            }
#endif
        }

        public void LoadLevel(LevelData levelData)
        {
            _currentLevelData = levelData ?? throw new System.ArgumentNullException(nameof(levelData), "Level data cannot be null.");
            LoadLevelInternal(_currentLevelData, ref _currentLevelContainer);
            GenerateTerrain(_currentLevelData, ref _currentTerrainContainer);
            PlaceLevelMarkersInternal(_currentLevelData, ref _levelMarkersContainer);
            StartGame();
        }

        public void UnloadCurrentLevel()
        {
            DestroyGameObject(ref _currentLevelContainer);
            DestroyGameObject(ref _levelMarkersContainer);
            DestroyGameObject(ref _currentTerrainContainer);
            _currentLevelData = null;
        }

        private static void LoadLevelInternal(LevelData levelData, ref GameObject levelContainer)
        {
            if (levelData == null)
            {
                Debug.LogError("Invalid level data provided!");
                return;
            }

            DestroyGameObject(ref levelContainer);

            levelContainer = new GameObject("LevelContainer");
            LevelLoader levelLoader = levelContainer.AddComponent<LevelLoader>();
            levelLoader.LevelData = levelData;

            PlayerTransform levelParent = levelContainer.PlayerTransform;

            foreach (var entityData in levelData.Entities)
            {
                if (entityData.EntityPrefab == null)
                {
                    Debug.LogWarning("Skipping entity with null prefab.");
                    continue;
                }

                GameObject entityInstance = InstantiateEntity(entityData, levelParent);
                if (entityInstance == null)
                {
                    continue;
                }

                if (entityInstance.TryGetComponent(out SpawnableEntity entity))
                {
                    entity.SetBaseColor(entityData.BaseColor);
                    entity.SetScale(entityData.Scale);
                    levelLoader.AddEntity(entity);
                }
            }
        }

        private static void PlaceLevelMarkersInternal(LevelData levelData, ref GameObject markersContainer)
        {
            DestroyGameObject(ref markersContainer);

            markersContainer = new GameObject("LevelMarkers");

            if (levelData.StartMarkerPrefab != null)
            {
                Instantiate(levelData.StartMarkerPrefab,
                    new Vector3(levelData.StartMarkerPrefab.PlayerTransform.position.x, levelData.StartMarkerPrefab.PlayerTransform.position.y, 0.0f),
                    Quaternion.identity, markersContainer.PlayerTransform);
            }

            if (levelData.EndMarkerPrefab != null)
            {
                Instantiate(levelData.EndMarkerPrefab,
                    new Vector3(levelData.EndMarkerPrefab.PlayerTransform.position.x, levelData.EndMarkerPrefab.PlayerTransform.position.y, levelData.LevelLength),
                    Quaternion.identity, markersContainer.PlayerTransform);
            }
        }

        private static void GenerateTerrain(LevelData levelData, ref GameObject terrainContainer)
        {
            TerrainBuilder.TerrainDimensions dimensions = new TerrainBuilder.TerrainDimensions
            {
                Width = levelData.LevelWidth,
                Length = levelData.LevelLength,
                StartBuffer = levelData.LevelLengthBufferStart,
                EndBuffer = levelData.LevelLengthBufferEnd,
                Thickness = levelData.LevelThickness
            };

            TerrainBuilder.BuildTerrain(dimensions, levelData.TerrainMaterial, ref terrainContainer);
        }

        private void StartGame()
        {
            ResetLevel();
            _isGameActive = true;
        }

        public void ResetLevel()
        {
            Player.Instance?.ResetPlayer();
            CameraController.Instance?.ResetCamera();
            LevelLoader.Instance?.ResetEntities();
        }

        public void OnVictory()
        {
            _victoryEvent.Raise();

#if UNITY_EDITOR
            if (_isEditorMode)
            {
                ResetLevel();
            }
#endif
        }

        public void OnDefeat()
        {
            _defeatEvent.Raise();

#if UNITY_EDITOR
            if (_isEditorMode)
            {
                ResetLevel();
            }
#endif
        }

        public bool IsGameActive => _isGameActive;

        private static GameObject InstantiateEntity(SpawnableData entityData, PlayerTransform parent)
        {
            GameObject instance;

            if (Application.isPlaying)
            {
                instance = Instantiate(entityData.EntityPrefab, entityData.Position, Quaternion.Euler(entityData.EulerAngles), parent);
            }
            else
            {
#if UNITY_EDITOR
                instance = (GameObject)PrefabUtility.InstantiatePrefab(entityData.EntityPrefab, parent);
                instance.PlayerTransform.position = entityData.Position;
                instance.PlayerTransform.eulerAngles = entityData.EulerAngles;
#else
                return null;
#endif
            }

            return instance;
        }

        private static void DestroyGameObject(ref GameObject obj)
        {
            if (obj == null) return;

            if (Application.isPlaying)
            {
                Destroy(obj);
            }
            else
            {
                DestroyImmediate(obj);
            }

            obj = null;
        }
    }
}