using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A base class for the buttons of the hyper-casual game template that
    /// contains basic functionalities like button sound effect
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class HyperCasualButton : MonoBehaviour
    {
        [SerializeField]
        protected Button m_Button;
        [SerializeField]
        SoundID m_ButtonSound = SoundID.ButtonSound;

        Action m_Action;

        protected virtual void OnEnable()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            m_Button.onClick.RemoveListener(OnClick);
        }
        
        /// <summary>
        /// Adds a listener the button event.
        /// </summary>
        /// <param name="handler">callback function</param>
        public void AddListener(Action handler)
        {
            m_Action += handler;
        }
        
        /// <summary>
        /// Removes a listener from the button event.
        /// </summary>
        /// <param name="handler">callback function</param>
        public void RemoveListener(Action handler)
        {
            m_Action -= handler;
        }
        
        protected virtual void OnClick()
        {
            if (!m_Button.interactable)
                return;
                
            m_Button.interactable = false;
            m_Action?.Invoke();
            PlayButtonSound();
            StartCoroutine(ReenableButton());
        }

        protected void PlayButtonSound()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayEffect(m_ButtonSound);
            }
        }

        IEnumerator ReenableButton()
        {
            yield return new WaitForSeconds(0.1f);
            if (m_Button != null)
            {
                m_Button.interactable = true;
            }
        }
    }
}
