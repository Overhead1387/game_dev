using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A level editor window that allows the user to
    /// load levels in the level editor scene, modify level
    /// parameters, and save that level to be loaded in
    /// a Runner game.
    /// </summary>
    public class RunnerLevelEditorWindow : EditorWindow
    {
        internal bool HasLoadedLevel { get; private set; }
        internal LevelData SourceLevelData => m_SourceLevelData;
        LevelData m_SourceLevelData;
        LevelData m_LoadedLevelData;

        GameObject m_LevelParentGO;
        GameObject m_LoadedLevelGO;
        GameObject m_TerrainGO;
        GameObject m_LevelMarkersGO;

        List<SpawnableEntity> m_SelectedSpawnables = new List<SpawnableEntity>();
        Color m_ActiveColor;
        bool m_CurrentLevelNotLoaded;
        bool m_AutoSaveShowSettings;
        bool m_AutoSaveLevel;
        bool m_AutoSavePlayer;
        bool m_AutoSaveCamera;

        bool m_AutoSaveSettingsLoaded;
        bool m_AttemptedToLoadPreviousLevel;

        const string k_EditorPrefsPreviouslyLoadedLevelPath = "PreviouslyLoadedLevelPath";

        const string k_AutoSaveSettingsInitializedKey = "AutoSaveInitialized";
        const string k_AutoSaveLevelKey = "AutoSaveLevel";
        const string k_AutoSavePlayerKey = "AutoSavePlayer";
        const string k_AutoSaveCameraKey = "AutoSaveCamera";
        const string k_AutoSaveShowSettingsKey = "AutoSaveShowSettings";

        const string k_LevelParentGameObjectName = "LevelParent";
        const string k_LevelEditorSceneName = "RunnerLevelEditor";
        const string k_LevelEditorScenePath = "Assets/Runner/Scenes/RunnerLevelEditor.unity";

        /// <summary>
        /// Returns the loaded LevelData.
        /// </summary>
        public LevelData LoadedLevelData => m_LoadedLevelData;

        static readonly Color s_Blue = new Color(0.0f, 0.0f, 1.0f, 1.0f);
        static readonly string s_LevelParentTag = "LevelParent";

        [MenuItem("Window/Runner Level Editor")]
        static void Init()
        {
            RunnerLevelEditorWindow window = (RunnerLevelEditorWindow)EditorWindow.GetWindow(typeof(RunnerLevelEditorWindow), false, "Level Editor");
            window.Show();

            // Load auto-save settings
            window.LoadAutoSaveSettings();
        }

        void OnFocus()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;

            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorApplication.playModeStateChanged += OnPlayModeChanged;

            EditorSceneManager.sceneSaved -= OnSceneSaved;
            EditorSceneManager.sceneSaved += OnSceneSaved;
        }

        /// <summary>
        /// Load the auto-save settings from EditorPrefs.
        /// </summary>
        public void LoadAutoSaveSettings()
        {
            bool autoSaveSettingsInitialized = EditorPrefs.GetBool(k_AutoSaveSettingsInitializedKey);

            if (!autoSaveSettingsInitialized)
            {
                // Default all auto-save values to true and save them to Editor Prefs
                // the first time the user opens the window

                m_AutoSaveLevel = true;
                m_AutoSavePlayer = true;
                m_AutoSaveCamera = true;

                EditorPrefs.SetBool(k_AutoSaveLevelKey, m_AutoSaveLevel);
                EditorPrefs.SetBool(k_AutoSavePlayerKey, m_AutoSavePlayer);
                EditorPrefs.SetBool(k_AutoSaveCameraKey, m_AutoSaveCamera);

                EditorPrefs.SetBool(k_AutoSaveSettingsInitializedKey, true);
                return;
            }

            m_AutoSaveShowSettings = EditorPrefs.GetBool(k_AutoSaveShowSettingsKey);
            m_AutoSaveLevel = EditorPrefs.GetBool(k_AutoSaveLevelKey);
            m_AutoSavePlayer = EditorPrefs.GetBool(k_AutoSavePlayerKey);
            m_AutoSaveCamera = EditorPrefs.GetBool(k_AutoSaveCameraKey);

            m_AutoSaveSettingsLoaded = true;
        }

        void OnPlayModeChanged(PlayModeStateChange state)
        {
            if ((state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode) && m_SourceLevelData != null)
            {
                Scene scene = EditorSceneManager.GetActiveScene();
                if (scene.name.Equals(k_LevelEditorSceneName))
                {
                    // Reload the scene automatically
                    LoadLevel(m_SourceLevelData);
                }
            }
            else if (state == PlayModeStateChange.ExitingEditMode && m_SourceLevelData != null && !LevelNotLoaded())
            {
                Scene scene = EditorSceneManager.GetActiveScene();
                if (scene.name.Equals(k_LevelEditorSceneName))
                {
                    // Save the scene automatically before testing
                    SaveLevel(m_LoadedLevelData);
                }
            }
        }

        void OnSceneSaved(Scene scene)
        {
            if (m_SourceLevelData != null && !LevelNotLoaded())
            {
                if (scene.name.Equals(k_LevelEditorSceneName))
                {
                    SaveLevel(m_LoadedLevelData);
                }
            }
        }

        void OnSelectionChange()
        {
            // Needed to update color options when a SpawnableEntity is selected
            Repaint();
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            EditorSceneManager.sceneSaved -= OnSceneSaved;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (m_LoadedLevelData == null)
            {
                string levelPath = EditorPrefs.GetString(k_EditorPrefsPreviouslyLoadedLevelPath);
                bool levelPathExists = !string.IsNullOrEmpty(levelPath);

                // Attempt to load previously loaded level
                if (!m_AttemptedToLoadPreviousLevel && levelPathExists)
                {
                    m_SourceLevelData = AssetDatabase.LoadAssetAtPath<LevelData>(levelPath);

                    if (m_SourceLevelData != null)
                    {
                        LoadLevel(m_SourceLevelData);
                    }
                    else
                    {
                        Debug.LogError($"Could not load level with path {levelPath}. Specify a valid level to continue.");
                        m_AttemptedToLoadPreviousLevel = true;
                    }
                }
                else if (levelPathExists)
                {
                    Debug.LogError($"Could not load level with path {levelPath}. Specify a valid level to continue.");
                    m_AttemptedToLoadPreviousLevel = true;
                }

                return;
            }

            if (m_LoadedLevelData.SnapToGrid)
            {
                float nearestGridPositionToLevelWidth = m_LoadedLevelData.LevelWidth + m_LoadedLevelData.LevelWidth % m_LoadedLevelData.GridSize;
                float nearestGridPositionToLevelLength = m_LoadedLevelData.LevelLength + m_LoadedLevelData.LevelLength % m_LoadedLevelData.GridSize;

                int numberOfGridLinesWide = (int)Mathf.Ceil(nearestGridPositionToLevelWidth / m_LoadedLevelData.GridSize);
                int numberOfGridLinesLong = (int)Mathf.Ceil(nearestGridPositionToLevelLength / m_LoadedLevelData.GridSize);

                Handles.BeginGUI();
                Handles.color = s_Blue;

                // Empty label is needed to draw lines below
                Handles.Label(Vector3.zero, "");

                float gridWidth = numberOfGridLinesWide * m_LoadedLevelData.GridSize;
                float gridLength = numberOfGridLinesLong * m_LoadedLevelData.GridSize;

                // Draw horizontal grid lines (parallel to X axis) from the start
                // of the level to the end of the level
                for (int z = 0; z <= numberOfGridLinesLong; z++)
                {
                    float zPosition = z * m_LoadedLevelData.GridSize;
                    Handles.DrawLine(new Vector3(-gridWidth, 0.0f, zPosition), new Vector3(gridWidth, 0.0f, zPosition));
                }

                // Draw vertical grid lines (parallel to Z axis) from the center out
                for (int x = 0; x <= numberOfGridLinesWide; x++)
                {
                    float xPosition = x * m_LoadedLevelData.GridSize;
                    Handles.DrawLine(new Vector3(-xPosition, 0.0f, 0.0f), new Vector3(-xPosition, 0.0f, gridLength));

                    // Only draw one grid line at the center of the level
                    if (x > 0)
                    {
                        Handles.DrawLine(new Vector3(xPosition, 0.0f, 0.0f), new Vector3(xPosition, 0.0f, gridLength));
                    }
                }
                Handles.EndGUI();
            }
        }

        void OnGUI()
        {
            if (!m_AutoSaveSettingsLoaded)
            {
                // Load auto-save settings
                LoadAutoSaveSettings();
            }

            if (Application.isPlaying)
            {
                GUILayout.Label("Exit play mode to continue editing level.");
                return;
            }

            Scene scene = SceneManager.GetActiveScene();
            if (!scene.name.Equals(k_LevelEditorSceneName))
            {
                if (GUILayout.Button("Open Level Editor Scene"))
                {
                    EditorSceneManager.OpenScene(k_LevelEditorScenePath);
                    if (m_SourceLevelData != null)
                    {
                        LoadLevel(m_SourceLevelData);
                    }
                }
                return;
            }

            m_SourceLevelData = (LevelData)EditorGUILayout.ObjectField("Level Definition", m_SourceLevelData, typeof(LevelData), false, null);

            if (m_SourceLevelData == null)
            {
                GUILayout.Label("Select a LevelData ScriptableObject to begin.");
                HasLoadedLevel = false;
                return;
            }

            if (m_LoadedLevelData != null && !m_SourceLevelData.name.Equals(m_LoadedLevelData.name))
            {
                // Automatically load the new source level if it has changed.
                LoadLevel(m_SourceLevelData);
                return;
            }

            if (Event.current.type == EventType.Layout)
            {
                m_CurrentLevelNotLoaded = LevelNotLoaded();
            }

            if (m_LoadedLevelGO != null && !m_CurrentLevelNotLoaded)
            {
                if (GUILayout.Button("Reload Level"))
                {
                    LoadLevel(m_SourceLevelData);
                }
            }
            else
            {
                LoadLevel(m_SourceLevelData);
            }

            if (m_LoadedLevelData == null || m_CurrentLevelNotLoaded)
            {
                GUILayout.Label("No level loaded.");
                return;
            }

            if (GUILayout.Button("Save Level"))
            {
                SaveLevel(m_LoadedLevelData);
            }

            // Auto-save

            m_AutoSaveShowSettings = EditorGUILayout.BeginFoldoutHeaderGroup(m_AutoSaveShowSettings, "Auto-Save Settings");

            if (m_AutoSaveShowSettings)
            {
                EditorGUI.BeginChangeCheck();
                m_AutoSaveLevel = EditorGUILayout.Toggle(new GUIContent("Save Level on Play", "Any changes made to the level being edited will be automatically saved when entering play mode."), m_AutoSaveLevel);
                m_AutoSavePlayer = EditorGUILayout.Toggle(new GUIContent("Save Player on Play", "Any changes made to the Player prefab will be automatically saved when entering play mode and reflected when playing the game via the Boot scene."), m_AutoSavePlayer);
                m_AutoSaveCamera = EditorGUILayout.Toggle(new GUIContent("Save Camera on Play", "Any changes made to the GameplayCamera prefab will be automatically saved when entering play mode and reflected when playing the game via the Boot scene."), m_AutoSaveCamera);
                if (EditorGUI.EndChangeCheck())
                {
                    SaveAutoSaveSettings();
                }
            }
            EditorGUILayout.Space();

            // Level Size Parameters

            GUILayout.Label("Terrain", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            m_LoadedLevelData.LevelLength = Mathf.Max(0.0f, EditorGUILayout.FloatField("Length", m_LoadedLevelData.LevelLength));
            m_LoadedLevelData.LevelWidth = Mathf.Max(0.0f, EditorGUILayout.FloatField("Width", m_LoadedLevelData.LevelWidth));
            m_LoadedLevelData.LevelLengthBufferStart = Mathf.Max(0.0f, EditorGUILayout.FloatField("Start Buffer", m_LoadedLevelData.LevelLengthBufferStart));
            m_LoadedLevelData.LevelLengthBufferEnd = Mathf.Max(0.0f, EditorGUILayout.FloatField("End Buffer", m_LoadedLevelData.LevelLengthBufferEnd));
            m_LoadedLevelData.LevelThickness = Mathf.Max(EditorGUILayout.FloatField("Level Thickness", m_LoadedLevelData.LevelThickness));
            m_LoadedLevelData.TerrainMaterial = (Material)EditorGUILayout.ObjectField("Terrain Material", m_LoadedLevelData.TerrainMaterial, typeof(Material), false, null);
            if (EditorGUI.EndChangeCheck() && m_TerrainGO != null && m_LevelParentGO != null)
            {
                GameController.BuildTerrain(m_LoadedLevelData, ref m_TerrainGO);
                m_TerrainGO.PlayerTransform.SetParent(m_LevelParentGO.PlayerTransform);
            }
            EditorGUILayout.Space();

            // SpawnableEntity Snapping

            GUILayout.Label("Snapping Options", EditorStyles.boldLabel);
            m_LoadedLevelData.SnapToGrid = EditorGUILayout.Toggle("Snap to Grid", m_LoadedLevelData.SnapToGrid);
            if (m_LoadedLevelData.SnapToGrid)
            {
                // Ensure the grid size is never too small, zero, or negative
                m_LoadedLevelData.GridSize = Mathf.Max(0.1f, EditorGUILayout.FloatField("Grid Size", m_LoadedLevelData.GridSize));
            }
            EditorGUILayout.Space();

            // Necessary Prefabs

            GUILayout.Label("Prefabs", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            m_LoadedLevelData.StartPrefab = (GameObject)EditorGUILayout.ObjectField("Start Prefab", m_LoadedLevelData.StartPrefab, typeof(GameObject), false, null);
            m_LoadedLevelData.EndPrefab = (GameObject)EditorGUILayout.ObjectField("End Prefab", m_LoadedLevelData.EndPrefab, typeof(GameObject), false, null);
            if (EditorGUI.EndChangeCheck())
            {
                GameController.PlaceLevelMarkers(m_LoadedLevelData, ref m_LevelMarkersGO);
                m_LevelMarkersGO.PlayerTransform.SetParent(m_LevelParentGO.PlayerTransform);
            }
            EditorGUILayout.Space();

            // SpawnableEntity Coloring
            if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
            {
                m_SelectedSpawnables.Clear();
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    SpawnableEntity SpawnableEntity = Selection.gameObjects[i].GetComponent<SpawnableEntity>();
                    if (SpawnableEntity != null && PrefabUtility.IsPartOfNonAssetPrefabInstance(Selection.gameObjects[i]))
                    {
                        m_SelectedSpawnables.Add(SpawnableEntity);
                    }
                }

                if (m_SelectedSpawnables.Count > 0)
                {
                    GUILayout.Label("Selected SpawnableEntity Options", EditorStyles.boldLabel);
                    m_ActiveColor = EditorGUILayout.ColorField("Base Color", m_ActiveColor);
                    if (GUILayout.Button("Apply Base Color to Selected Spawnables"))
                    {
                        for (int i = 0; i < m_SelectedSpawnables.Count; i++)
                        {
                            m_SelectedSpawnables[i].SetBaseColor(m_ActiveColor);
                        }
                    }
                }
                EditorGUILayout.Space();
            }

            EditorGUILayout.HelpBox($"New objects added to the level require a {nameof(SpawnableEntity)} type component added to the GameObject", MessageType.Info);
        }

        bool LevelNotLoaded()
        {
            return m_LoadedLevelData == null || m_LevelParentGO == null || m_LoadedLevelGO == null || m_TerrainGO == null || m_LevelMarkersGO == null;
        }

        void LoadLevel(LevelData LevelData)
        {
            UnloadOpenLevels();

            if (!EditorSceneManager.GetActiveScene().path.Equals(k_LevelEditorScenePath))
                return;

            m_LoadedLevelData = Instantiate(LevelData);
            m_LoadedLevelData.name = LevelData.name;

            m_LevelParentGO = new GameObject(k_LevelParentGameObjectName);
            m_LevelParentGO.tag = s_LevelParentTag;

            GameController.LoadLevel(m_LoadedLevelData, ref m_LoadedLevelGO);
            GameController.BuildTerrain(m_LoadedLevelData, ref m_TerrainGO);
            GameController.PlaceLevelMarkers(m_LoadedLevelData, ref m_LevelMarkersGO);

            m_LoadedLevelGO.PlayerTransform.SetParent(m_LevelParentGO.PlayerTransform);
            m_TerrainGO.PlayerTransform.SetParent(m_LevelParentGO.PlayerTransform);
            m_LevelMarkersGO.PlayerTransform.SetParent(m_LevelParentGO.PlayerTransform);
            HasLoadedLevel = true;

            string levelPath = AssetDatabase.GetAssetPath(LevelData);
            EditorPrefs.SetString(k_EditorPrefsPreviouslyLoadedLevelPath, levelPath);

            m_AttemptedToLoadPreviousLevel = false;

            Repaint();
        }

        void UnloadOpenLevels()
        {
            GameObject[] levelParents = GameObject.FindGameObjectsWithTag(s_LevelParentTag);
            for (int i = 0; i < levelParents.Length; i++)
            {
                DestroyImmediate(levelParents[i]);
            }

            m_LevelParentGO = null;
        }

        void SaveLevel(LevelData LevelData)
        {
            if (m_AutoSaveLevel)
            {
                // Update array of spawnables based on what is currently in the scene
                SpawnableEntity[] spawnables = (SpawnableEntity[])Object.FindObjectsOfType(typeof(SpawnableEntity));
                LevelData.Spawnables = new LevelData.SpawnableObject[spawnables.Length];
                for (int i = 0; i < spawnables.Length; i++)
                {
                    try
                    {
                        LevelData.Spawnables[i] = new LevelData.SpawnableObject()
                        {
                            SpawnablePrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(spawnables[i].gameObject),
                            Position = spawnables[i].SavedPosition,
                            EulerAngles = spawnables[i].PlayerTransform.eulerAngles,
                            Scale = spawnables[i].PlayerTransform.lossyScale,
                            BaseColor = spawnables[i].BaseColor
                        };
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }

                // Overwrite source level definition with current version
                m_SourceLevelData.UpdateValues(LevelData);
            }

            if (m_AutoSavePlayer)
            {
                Player[] players = (Player[])Object.FindObjectsOfType(typeof(Player));
                if (players.Length == 1)
                {
                    GameObject playerPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(players[0].gameObject);
                    if (playerPrefab != null)
                    {
                        PrefabUtility.ApplyPrefabInstance(players[0].gameObject, InteractionMode.UserAction);
                    }
                    else
                    {
                        Debug.LogError("Player could not be found on a prefab instance. Changes could not be saved.");
                    }
                }
                else
                {
                    if (players.Length == 0)
                    {
                        Debug.LogWarning("No instance of Player found in the scene. No changes saved!");
                    }
                    else
                    {
                        Debug.LogWarning("More than two instances of Player found in the scene. No changes saved!");
                    }
                }
            }

            if (m_AutoSaveCamera)
            {
                CameraController[] cameraManagers = (CameraController[])Object.FindObjectsOfType(typeof(CameraController));
                if (cameraManagers.Length == 1)
                {
                    GameObject cameraManagerPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(cameraManagers[0].gameObject);
                    if (cameraManagerPrefab != null)
                    {
                        PrefabUtility.ApplyPrefabInstance(cameraManagers[0].gameObject, InteractionMode.UserAction);
                    }
                    else
                    {
                        Debug.LogError("CameraController could not be found on a prefab instance. Changes could not be saved.");
                    }
                }
                else
                {
                    if (cameraManagers.Length == 0)
                    {
                        Debug.LogWarning("No instance of CameraController found in the scene. No changes saved!");
                    }
                    else
                    {
                        Debug.LogWarning("More than two instances of CameraController found in the scene. No changes saved!");
                    }
                }
            }

            // Set level definition dirty so the changes will be written to disk
            EditorUtility.SetDirty(m_SourceLevelData);

            // Write changes to disk
            AssetDatabase.SaveAssets();
        }

        void SaveAutoSaveSettings()
        {
            // Write auto-save settings to EditorPrefs
            EditorPrefs.SetBool(k_AutoSaveLevelKey, m_AutoSaveLevel);
            EditorPrefs.SetBool(k_AutoSavePlayerKey, m_AutoSavePlayer);
            EditorPrefs.SetBool(k_AutoSaveCameraKey, m_AutoSaveCamera);
            EditorPrefs.SetBool(k_AutoSaveShowSettingsKey, m_AutoSaveShowSettings);
        }
    }
}