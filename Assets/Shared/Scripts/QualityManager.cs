using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    // Manages game quality settings at runtime using the Singleton pattern
    // Provides centralized control over graphics quality levels and persistence
    public class QualityManager : AbstractSingleton<QualityManager>
    {
        private int m_QualityLevel;    // Current quality level setting

        // Property to get/set quality level and apply changes immediately
        public int QualityLevel
        {
            get => m_QualityLevel;
            set
            {
                m_QualityLevel = value;
                if (QualitySettings.GetQualityLevel() != m_QualityLevel)
                    QualitySettings.SetQualityLevel(m_QualityLevel, true);
            }
        }

        void OnEnable()
        {
            if (SaveManager.Instance.IsQualityLevelSaved)
                QualityLevel = SaveManager.Instance.QualityLevel;
            else
                QualityLevel = 2;
        }

        void OnDisable()
        {
            SaveManager.Instance.QualityLevel = QualityLevel;
        }
    }
}