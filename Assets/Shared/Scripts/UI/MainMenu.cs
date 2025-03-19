using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains main menu functionalities
    /// </summary>
    public class MainMenu : View
    {
        [SerializeField]
        HyperCasualButton m_StartButton;
        [SerializeField]
        HyperCasualButton m_SettingsButton;
        [SerializeField]
        HyperCasualButton m_ShopButton;
        [SerializeField]
        AbstractGameEvent m_StartButtonEvent;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_StartButton == null)
                Debug.LogError($"[{nameof(MainMenu)}] Start Button reference is missing");
            if (m_SettingsButton == null)
                Debug.LogError($"[{nameof(MainMenu)}] Settings Button reference is missing");
            if (m_ShopButton == null)
                Debug.LogError($"[{nameof(MainMenu)}] Shop Button reference is missing");
            if (m_StartButtonEvent == null)
                Debug.LogError($"[{nameof(MainMenu)}] Start Button Event reference is missing");
        }

        void OnEnable()
        {
            if (m_StartButton != null)
                m_StartButton.AddListener(OnStartButtonClick);
            if (m_SettingsButton != null)
                m_SettingsButton.AddListener(OnSettingsButtonClick);
            if (m_ShopButton != null)
                m_ShopButton.AddListener(OnShopButtonClick);
        }
        
        void OnDisable()
        {
            if (m_StartButton != null)
                m_StartButton.RemoveListener(OnStartButtonClick);
            if (m_SettingsButton != null)
                m_SettingsButton.RemoveListener(OnSettingsButtonClick);
            if (m_ShopButton != null)
                m_ShopButton.RemoveListener(OnShopButtonClick);
        }

        void OnStartButtonClick()
        {
            if (m_StartButtonEvent != null)
                m_StartButtonEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(MainMenu)}] Start Button Event is null");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayEffect(SoundID.ButtonSound);
            else
                Debug.LogWarning($"[{nameof(MainMenu)}] AudioManager instance is null");
        }

        void OnSettingsButtonClick()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.Show<SettingsMenu>();
            else
                Debug.LogError($"[{nameof(MainMenu)}] UIManager instance is null");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayEffect(SoundID.ButtonSound);
            else
                Debug.LogWarning($"[{nameof(MainMenu)}] AudioManager instance is null");
        }

        void OnShopButtonClick()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.Show<ShopView>();
            else
                Debug.LogError($"[{nameof(MainMenu)}] UIManager instance is null");

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayEffect(SoundID.ButtonSound);
            else
                Debug.LogWarning($"[{nameof(MainMenu)}] AudioManager instance is null");
        }
    }
}