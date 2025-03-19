using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HyperCasual.Runner
{
    public class LoadLevelFromDef : AbstractState
    {
        public readonly LevelDefinition m_LevelDefinition;
        readonly SceneController m_SceneController;
        readonly GameObject[] m_ManagerPrefabs;

        public LoadLevelFromDef(SceneController sceneController, AbstractLevelData levelData, GameObject[] managerPrefabs)
        {
            if (levelData is LevelDefinition levelDefinition)
                m_LevelDefinition = levelDefinition;

            m_ManagerPrefabs = managerPrefabs;
            m_SceneController = sceneController;
        }
        
        public override IEnumerator Execute()
        {
            try
            {
                if (m_LevelDefinition == null)
                    throw new Exception($"{nameof(m_LevelDefinition)} is null!");

                yield return m_SceneController.LoadNewScene(nameof(m_LevelDefinition));

                // Pre-instantiate managers to improve loading efficiency
                var managerInstances = new GameObject[m_ManagerPrefabs.Length];
                for (int i = 0; i < m_ManagerPrefabs.Length; i++)
                {
                    if (m_ManagerPrefabs[i] != null)
                        managerInstances[i] = Object.Instantiate(m_ManagerPrefabs[i]);
                }

                // Load level after managers are ready
                if (GameManager.Instance != null)
                    GameManager.Instance.LoadLevel(m_LevelDefinition);
                else
                    Debug.LogError("GameManager instance not found!");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading level: {e.Message}");
                throw;
            }
        }
    }
}