using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using TMPro;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A button used by LevelSelectionScreen to dynamically populate the list of levels to select from
    /// </summary>
    public class LevelSelectButton : HyperCasualButton
    {
        [SerializeField]
        TextMeshProUGUI m_Text;

        int m_Index = -1;
        Action<int> m_OnClick;
        bool m_IsUnlocked;

        protected override void Awake()
        {
            base.Awake();
            ValidateReferences();
        }

        void ValidateReferences()
        {
            if (m_Text == null)
                Debug.LogError($"[{nameof(LevelSelectButton)}] Text reference is missing");
        }
        
        /// <param name="index">The index of the associated level</param>
        /// <param name="unlocked">Is the associated level locked?</param>
        /// <param name="onClick">callback method for this button</param>
        public void SetData(int index, bool unlocked, Action<int> onClick)
        {
            m_Index = index;
            if (m_Text != null)
                m_Text.text = (index + 1).ToString();
            else
                Debug.LogWarning($"[{nameof(LevelSelectButton)}] Cannot set text - Text component is missing");
            
            m_OnClick = onClick;
            m_IsUnlocked = unlocked;
            if (m_Button != null)
                m_Button.interactable = m_IsUnlocked;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AddListener(OnClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveListener(OnClick);
        }

        protected override void OnClick()
        {
            if (m_Index < 0)
                throw new Exception("Button is not initialized");

            m_OnClick?.Invoke(m_Index);
            PlayButtonSound();
        }
    }
}