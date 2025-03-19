using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using UnityEngine;

namespace HyperCasual.Runner
{
    // Manages player's in-game resources (gold, XP, keys) and handles resource-related events
    // Implements Singleton pattern for global access to inventory state
    public class Inventory : AbstractSingleton<Inventory>
    {
        [SerializeField] private GenericGameEventListener m_GoldEventListener;  // Listens for gold collection events
        [SerializeField] private GenericGameEventListener m_KeyEventListener;    // Listens for key collection events
        [SerializeField] private GenericGameEventListener m_WinEventListener;    // Handles level completion
        [SerializeField] private GenericGameEventListener m_LoseEventListener;   // Handles level failure

        private int m_TempGold;    // Current level gold count
        private int m_TotalGold;   // Total accumulated gold
        private float m_TempXp;    // Current level XP
        private float m_TotalXp;   // Total accumulated XP
        private int m_TempKeys;    // Current level key count

        // Multiplier for XP milestone calculations
        // TODO: Implement proper milestone calculation formula based on game progression
        private const float k_MilestoneFactor = 1.2f;

        Hud m_Hud;
        LevelCompleteScreen m_LevelCompleteScreen;

        void Start()
        {
            m_GoldEventListener.EventHandler = OnGoldPicked;
            m_KeyEventListener.EventHandler = OnKeyPicked;
            m_WinEventListener.EventHandler = OnWin;
            m_LoseEventListener.EventHandler = OnLose;

            m_TempGold = 0;
            m_TotalGold = SaveManager.Instance.Currency;
            m_TempXp = 0;
            m_TotalXp = SaveManager.Instance.XP;
            m_TempKeys = 0;

            m_LevelCompleteScreen = UIManager.Instance.GetView<LevelCompleteScreen>();
            m_Hud = UIManager.Instance.GetView<Hud>();
        } 

        void OnEnable()
        {
            m_GoldEventListener.Subscribe();
            m_KeyEventListener.Subscribe();
            m_WinEventListener.Subscribe();
            m_LoseEventListener.Subscribe();
        }

        void OnDisable()
        {
            m_GoldEventListener.Unsubscribe();
            m_KeyEventListener.Unsubscribe();
            m_WinEventListener.Unsubscribe();
            m_LoseEventListener.Unsubscribe();
        }

        void OnGoldPicked()
        {
            if (m_GoldEventListener.m_Event is ItemPickedEvent goldPickedEvent)
            {
                m_TempGold += goldPickedEvent.Count;
                m_Hud.GoldValue = m_TempGold;
            }
            else
            {
                throw new Exception($"Invalid event type!");
            }
        }

        void OnKeyPicked()
        {
            if (m_KeyEventListener.m_Event is ItemPickedEvent keyPickedEvent)
            {
                m_TempKeys += keyPickedEvent.Count;
            }
            else
            {
                throw new Exception($"Invalid event type!");
            }
        }

        void OnWin()
        {
            m_TotalGold += m_TempGold;
            m_TempGold = 0;
            SaveManager.Instance.Currency = m_TotalGold;

            m_LevelCompleteScreen.GoldValue = m_TotalGold;
            m_LevelCompleteScreen.XpSlider.minValue = m_TotalXp;
            m_LevelCompleteScreen.XpSlider.maxValue = k_MilestoneFactor * (m_TotalXp + m_TempXp);
            m_LevelCompleteScreen.XpValue = m_TotalXp + m_TempXp;

            m_LevelCompleteScreen.StarCount = m_TempKeys;

            m_TotalXp += m_TempXp;
            m_TempXp = 0f;
            SaveManager.Instance.XP = m_TotalXp;
        }

        void OnLose()
        {
            m_TempGold = 0;
            m_TotalXp += m_TempXp;
            m_TempXp = 0f;
            SaveManager.Instance.XP = m_TotalXp;
        }

        void Update()
        {
            // Only update XP when HUD is visible to optimize performance
            if (m_Hud != null && m_Hud.gameObject.activeSelf)
            {
                m_TempXp += PlayerController.Instance.Speed * Time.deltaTime;
                m_Hud.XpValue = m_TempXp;
                
                if (SequenceManager.Instance.m_CurrentLevel is LoadLevelFromDef loadLevelFromDef)
                {
                    m_Hud.XpSlider.minValue = 0;
                    m_Hud.XpSlider.maxValue = loadLevelFromDef.m_LevelDefinition.LevelLength;
                }
            }
        }
    }
}
