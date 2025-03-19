using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// Instantiates and initializes a SequenceManager on Start
    /// </summary>
    public class BootLoader : MonoBehaviour
    {
        [SerializeField]
        SequenceManager m_SequenceManagerPrefab;
        
        void Start()
        {
            if (m_SequenceManagerPrefab == null)
            {
                Debug.LogError("[BootLoader] SequenceManager prefab is not assigned!");
                return;
            }

            try
            {
                var sequenceManager = Instantiate(m_SequenceManagerPrefab);
                if (sequenceManager != null)
                {
                    SequenceManager.Instance.Initialize();
                }
                else
                {
                    Debug.LogError("[BootLoader] Failed to instantiate SequenceManager!");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[BootLoader] Error during initialization: {e.Message}");
            }
        }
    }
}