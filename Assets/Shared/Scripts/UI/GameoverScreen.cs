using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Manages the game over screen UI and its interactions.
    /// Handles play again and main menu navigation events.
    /// Inherits from View for UI management integration.
    /// </summary>
    public class GameoverScreen : View
    {
        [SerializeField]
        HyperCasualButton m_PlayAgainButton;
        [SerializeField]
        HyperCasualButton m_GoToMainMenuButton;
        [SerializeField]
        AbstractGameEvent m_PlayAgainEvent;
        [SerializeField]
        AbstractGameEvent m_GoToMainMenuEvent;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_PlayAgainButton == null)
                Debug.LogError($"[{nameof(GameoverScreen)}] Play Again Button reference is missing");
            if (m_GoToMainMenuButton == null)
                Debug.LogError($"[{nameof(GameoverScreen)}] Go To Main Menu Button reference is missing");
            if (m_PlayAgainEvent == null)
                Debug.LogError($"[{nameof(GameoverScreen)}] Play Again Event reference is missing");
            if (m_GoToMainMenuEvent == null)
                Debug.LogError($"[{nameof(GameoverScreen)}] Go To Main Menu Event reference is missing");
        }

        void OnEnable()
        {
            if (m_PlayAgainButton != null)
                m_PlayAgainButton.AddListener(OnPlayAgainButtonClick);
            if (m_GoToMainMenuButton != null)
                m_GoToMainMenuButton.AddListener(OnGoToMainMenuButtonClick);
        }

        void OnDisable()
        {
            if (m_PlayAgainButton != null)
                m_PlayAgainButton.RemoveListener(OnPlayAgainButtonClick);
            if (m_GoToMainMenuButton != null)
                m_GoToMainMenuButton.RemoveListener(OnGoToMainMenuButtonClick);
        }

        void OnPlayAgainButtonClick()
        {
            if (m_PlayAgainEvent != null)
                m_PlayAgainEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(GameoverScreen)}] Play Again Event is null");
        }

        void OnGoToMainMenuButtonClick()
        {
            if (m_GoToMainMenuEvent != null)
                m_GoToMainMenuEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(GameoverScreen)}] Go To Main Menu Event is null");
        }
    }
}