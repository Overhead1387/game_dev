using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Runner;
using UnityEngine;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// This View contains level selection screen functionalities
    /// </summary>
    public class LevelSelectionScreen : View
    {
        [SerializeField]
        HyperCasualButton m_QuickPlayButton;
        [SerializeField]
        HyperCasualButton m_BackButton;
        [Space]
        [SerializeField]
        LevelSelectButton m_LevelButtonPrefab;
        [SerializeField]
        RectTransform m_LevelButtonsRoot;
        [SerializeField]
        AbstractGameEvent m_NextLevelEvent;
        [SerializeField]
        AbstractGameEvent m_BackEvent;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_QuickPlayButton == null)
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] Quick Play Button reference is missing");
            if (m_BackButton == null)
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] Back Button reference is missing");
            if (m_LevelButtonPrefab == null)
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] Level Button Prefab reference is missing");
            if (m_LevelButtonsRoot == null)
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] Level Buttons Root reference is missing");
            if (m_NextLevelEvent == null)
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] Next Level Event reference is missing");
            if (m_BackEvent == null)
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] Back Event reference is missing");
        }
#if UNITY_EDITOR
        [SerializeField]
        bool m_UnlockAllLevels;
#endif

        readonly List<LevelSelectButton> m_Buttons = new();

        void Start()
        {
            var levels = SequenceManager.Instance.Levels;
            for (int i = 0; i < levels.Length; i++)
            {
                m_Buttons.Add(Instantiate(m_LevelButtonPrefab, m_LevelButtonsRoot));
            }

            ResetButtonData();
        }

        void OnEnable()
        {
            ResetButtonData();
            
            if (m_QuickPlayButton != null)
                m_QuickPlayButton.AddListener(OnQuickPlayButtonClicked);
            if (m_BackButton != null)
                m_BackButton.AddListener(OnBackButtonClicked);
        }

        void OnDisable()
        {
            if (m_QuickPlayButton != null)
                m_QuickPlayButton.RemoveListener(OnQuickPlayButtonClicked);
            if (m_BackButton != null)
                m_BackButton.RemoveListener(OnBackButtonClicked);
        }

        void ResetButtonData()
        {
            var levelProgress = SaveManager.Instance.LevelProgress;
            for (int i = 0; i < m_Buttons.Count; i++)
            {
                var button = m_Buttons[i];
                var unlocked = i <= levelProgress;
#if UNITY_EDITOR
                unlocked = unlocked || m_UnlockAllLevels;
#endif
                button.SetData(i, unlocked, OnClick);
            }
        }
        
        void OnClick(int startingIndex)
        {
            if (startingIndex < 0)
                throw new Exception("Button is not initialized");

            if (SequenceManager.Instance != null)
                SequenceManager.Instance.SetStartingLevel(startingIndex);
            else
                Debug.LogError($"[{nameof(LevelSelectionScreen)}] SequenceManager instance is null");

            if (m_NextLevelEvent != null)
                m_NextLevelEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(LevelSelectionScreen)}] Next Level Event is null");
        }
        
        void OnQuickPlayButtonClicked()
        {
            OnClick(SaveManager.Instance.LevelProgress);
        }
        
        void OnBackButtonClicked()
        {
            if (m_BackEvent != null)
                m_BackEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(LevelSelectionScreen)}] Back Event is null");
        }
    }
}
