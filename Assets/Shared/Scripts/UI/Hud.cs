using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// This View contains head-up-display functionalities
    /// </summary>
    public class Hud : View
    {
        [SerializeField]
        TextMeshProUGUI m_GoldText;
        [SerializeField]
        Slider m_XpSlider;
        [SerializeField]
        HyperCasualButton m_PauseButton;
        [SerializeField]
        AbstractGameEvent m_PauseEvent;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_GoldText == null)
                Debug.LogError($"[{nameof(Hud)}] Gold Text reference is missing");
            if (m_XpSlider == null)
                Debug.LogError($"[{nameof(Hud)}] XP Slider reference is missing");
            if (m_PauseButton == null)
                Debug.LogError($"[{nameof(Hud)}] Pause Button reference is missing");
            if (m_PauseEvent == null)
                Debug.LogError($"[{nameof(Hud)}] Pause Event reference is missing");
        }

        /// <summary>
        /// The slider that displays the XP value 
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        int m_GoldValue;
        
        /// <summary>
        /// The amount of gold to display on the hud.
        /// The setter method also sets the hud text.
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
        /// The amount of XP to display on the hud.
        /// The setter method also sets the hud slider value.
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

        void OnEnable()
        {
            if (m_PauseButton != null)
                m_PauseButton.AddListener(OnPauseButtonClick);
        }

        void OnDisable()
        {
            if (m_PauseButton != null)
                m_PauseButton.RemoveListener(OnPauseButtonClick);
        }

        void OnPauseButtonClick()
        {
            if (m_PauseEvent != null)
                m_PauseEvent.Raise();
            else
                Debug.LogWarning($"[{nameof(Hud)}] Pause Event is null");
        }
    }
}
