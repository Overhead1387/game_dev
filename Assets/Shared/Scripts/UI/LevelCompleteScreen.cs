using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains celebration screen functionalities
    /// </summary>
    public class LevelCompleteScreen : View
    {
        [SerializeField]
        HyperCasualButton m_NextButton;
        [SerializeField]
        Image[] m_Stars;
        [SerializeField]
        AbstractGameEvent m_NextLevelEvent;
        [SerializeField]
        TextMeshProUGUI m_GoldText;
        [SerializeField]
        Slider m_XpSlider;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_NextButton == null)
                Debug.LogError($"[{nameof(LevelCompleteScreen)}] Next Button reference is missing");
            if (m_Stars == null || m_Stars.Length == 0)
                Debug.LogError($"[{nameof(LevelCompleteScreen)}] Stars array is empty or null");
            if (m_NextLevelEvent == null)
                Debug.LogError($"[{nameof(LevelCompleteScreen)}] Next Level Event reference is missing");
            if (m_GoldText == null)
                Debug.LogError($"[{nameof(LevelCompleteScreen)}] Gold Text reference is missing");
            if (m_XpSlider == null)
                Debug.LogError($"[{nameof(LevelCompleteScreen)}] XP Slider reference is missing");
        }
        
        /// <summary>
        /// The slider that displays the XP value 
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        int m_GoldValue;
        
        /// <summary>
        /// The amount of gold to display on the celebration screen.
        /// The setter method also sets the celebration screen text.
        /// </summary>
        public int GoldValue
        {
            get => m_GoldValue;
            set
            {
                if (m_GoldValue != value)
                {
                    m_GoldValue = value;
                    m_GoldText.text = GoldValue.ToString();
                }
            }
        }

        float m_XpValue;
        
        /// <summary>
        /// The amount of XP to display on the celebration screen.
        /// The setter method also sets the celebration screen slider value.
        /// </summary>
        public float XpValue
        {
            get => m_XpValue;
            set
            {
                if (!Mathf.Approximately(m_XpValue, value))
                {
                    m_XpValue = value;
                    m_XpSlider.value = m_XpValue;
                }
            }
        }

        int m_StarCount = -1;
        
        /// <summary>
        /// The number of stars to display on the celebration screen.
        /// </summary>
        public int StarCount
        {
            get => m_StarCount;
            set
            {
                if (m_StarCount != value)
                {
                    m_StarCount = value;
                    DisplayStars(m_StarCount);
                }
            }
        }

        void OnEnable()
        {
            if (m_NextButton != null)
                m_NextButton.AddListener(OnNextButtonClicked);
        }

        void OnDisable()
        {
            if (m_NextButton != null)
                m_NextButton.RemoveListener(OnNextButtonClicked);
        }

        void OnNextButtonClicked()
        {
            if (m_NextLevelEvent != null)
                m_NextLevelEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(LevelCompleteScreen)}] Next Level Event is null");
        }

        void DisplayStars(int count)
        {
            if (m_Stars == null || m_Stars.Length == 0)
            {
                Debug.LogWarning($"[{nameof(LevelCompleteScreen)}] Cannot display stars - star array is empty or null");
                return;
            }

            count = Mathf.Clamp(count, 0, m_Stars.Length);
            for (int i = 0; i < m_Stars.Length; i++)
            {
                if (m_Stars[i] != null)
                    m_Stars[i].gameObject.SetActive(i < count);
                else
                    Debug.LogWarning($"[{nameof(LevelCompleteScreen)}] Star at index {i} is null");
            }
        }
    }
}
