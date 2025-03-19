using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Settings menu that manages audio and quality settings for the game.
    /// Provides UI controls for music, sound effects, master volume and graphics quality.
    /// </summary>
    public class SettingsMenu : View
    {
        [SerializeField, Tooltip("Back button to return to previous menu")]
        HyperCasualButton m_Button;
        [SerializeField, Tooltip("Toggle for enabling/disabling background music")]
        Toggle m_EnableMusicToggle;
        [SerializeField, Tooltip("Toggle for enabling/disabling sound effects")]
        Toggle m_EnableSfxToggle;
        [SerializeField, Tooltip("Slider to control master volume (0-1)")]
        Slider m_AudioVolumeSlider;
        [SerializeField, Tooltip("Slider to control graphics quality level")]
        Slider m_QualitySlider;

        protected override void Awake()
        {
            base.Awake();
            ValidateComponents();
        }

        void ValidateComponents()
        {
            if (m_Button == null)
                Debug.LogError($"[{nameof(SettingsMenu)}] Back Button reference is missing");
            if (m_EnableMusicToggle == null)
                Debug.LogError($"[{nameof(SettingsMenu)}] Music Toggle reference is missing");
            if (m_EnableSfxToggle == null)
                Debug.LogError($"[{nameof(SettingsMenu)}] SFX Toggle reference is missing");
            if (m_AudioVolumeSlider == null)
                Debug.LogError($"[{nameof(SettingsMenu)}] Volume Slider reference is missing");
            if (m_QualitySlider == null)
                Debug.LogError($"[{nameof(SettingsMenu)}] Quality Slider reference is missing");
        }
        
        void OnEnable()
        {
            m_EnableMusicToggle.isOn = AudioManager.Instance.EnableMusic;
            m_EnableSfxToggle.isOn = AudioManager.Instance.EnableSfx;
            m_AudioVolumeSlider.value = AudioManager.Instance.MasterVolume;
            m_QualitySlider.value = QualityManager.Instance.QualityLevel;
            
            m_Button.AddListener(OnBackButtonClick);
            m_EnableMusicToggle.onValueChanged.AddListener(MusicToggleChanged);
            m_EnableSfxToggle.onValueChanged.AddListener(SfxToggleChanged);
            m_AudioVolumeSlider.onValueChanged.AddListener(VolumeSliderChanged);
            m_QualitySlider.onValueChanged.AddListener(QualitySliderChanged);
        }
        
        void OnDisable()
        {
            m_Button.RemoveListener(OnBackButtonClick);
            m_EnableMusicToggle.onValueChanged.RemoveListener(MusicToggleChanged);
            m_EnableSfxToggle.onValueChanged.RemoveListener(SfxToggleChanged);
            m_AudioVolumeSlider.onValueChanged.RemoveListener(VolumeSliderChanged);
            m_QualitySlider.onValueChanged.RemoveListener(QualitySliderChanged);
        }

        void MusicToggleChanged(bool value)
        {
            AudioManager.Instance.EnableMusic = value;
        }

        void SfxToggleChanged(bool value)
        {
            AudioManager.Instance.EnableSfx = value;
        }

        void VolumeSliderChanged(float value)
        {
            AudioManager.Instance.MasterVolume = value;
        }
        
        void QualitySliderChanged(float value)
        {
            QualityManager.Instance.QualityLevel = (int)value;
        }

        void OnBackButtonClick()
        {
            UIManager.Instance.GoBack();
        }
    }
}
