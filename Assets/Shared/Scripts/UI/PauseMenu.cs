using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains pause menu functionalities
    /// </summary>
    public class PauseMenu : View
    {
        [SerializeField]
        HyperCasualButton m_ContinueButton;

        [SerializeField]
        HyperCasualButton m_QuitButton;

        [SerializeField]
        AbstractGameEvent m_ContinueEvent;

        [SerializeField]
        AbstractGameEvent m_QuitEvent;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_ContinueButton == null)
                Debug.LogError($"[{nameof(PauseMenu)}] Continue Button reference is missing");
            if (m_QuitButton == null)
                Debug.LogError($"[{nameof(PauseMenu)}] Quit Button reference is missing");
            if (m_ContinueEvent == null)
                Debug.LogError($"[{nameof(PauseMenu)}] Continue Event reference is missing");
            if (m_QuitEvent == null)
                Debug.LogError($"[{nameof(PauseMenu)}] Quit Event reference is missing");
        }

        void OnEnable()
        {
            if (m_ContinueButton != null)
                m_ContinueButton.AddListener(OnContinueClicked);
            if (m_QuitButton != null)
                m_QuitButton.AddListener(OnQuitClicked);
        }

        void OnDisable()
        {
            if (m_ContinueButton != null)
                m_ContinueButton.RemoveListener(OnContinueClicked);
            if (m_QuitButton != null)
                m_QuitButton.RemoveListener(OnQuitClicked);
        }

        void OnContinueClicked()
        {
            if (m_ContinueEvent != null)
                m_ContinueEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(PauseMenu)}] Continue Event is null");
        }

        void OnQuitClicked()
        {
            if (m_QuitEvent != null)
                m_QuitEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(PauseMenu)}] Quit Event is null");
        }
    }
}
