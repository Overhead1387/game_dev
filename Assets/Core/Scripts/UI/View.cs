using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Core
{
    /// <summary>
    /// The base class for all UI elements that can be registered in UIManager.
    /// Provides core functionality for UI visibility and initialization.
    /// </summary>
    public abstract class View : MonoBehaviour
    {
        protected bool IsInitialized { get; private set; }
        protected RectTransform m_RectTransform;

        protected virtual void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Initializes the View. This method should be called before showing the view.
        /// Override this method to perform custom initialization logic.
        /// </summary>
        public virtual void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;
        }
        
        /// <summary>
        /// Makes the View visible. Ensures the view is initialized before showing.
        /// </summary>
        public virtual void Show()
        {
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
            if (!IsInitialized)
                Initialize();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the view. Safe to call even if the view is not initialized.
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}