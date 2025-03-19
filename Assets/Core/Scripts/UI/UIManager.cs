using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyperCasual.Core
{
    /// <summary>
    /// A singleton that manages display state and access to UI Views.
    /// Provides efficient view management with caching and type-based lookup.
    /// </summary>
    public class UIManager : AbstractSingleton<UIManager>
    {
        [SerializeField]
        Canvas m_Canvas;
        [SerializeField]
        RectTransform m_Root;
        [SerializeField]
        RectTransform m_BackgroundLayer;
        [SerializeField]
        RectTransform m_ViewLayer;

        // Cached list of all available views
        List<View> m_Views;
        // Type-based view lookup for faster access
        Dictionary<Type, View> m_ViewCache;
        
        View m_CurrentView;

        readonly Stack<View> m_History = new();

        void Start()
        {
            if (m_Root == null)
            {
                Debug.LogError("[UIManager] Root transform not assigned!");
                return;
            }

            m_Views = m_Root.GetComponentsInChildren<View>(true).ToList();
            m_ViewCache = new Dictionary<Type, View>();
            
            // Cache views by their concrete types for faster lookup
            foreach (var view in m_Views)
            {
                if (view != null)
                {
                    m_ViewCache[view.GetType()] = view;
                }
            }

            Init();
            
            if (m_ViewLayer != null && m_Canvas != null)
            {
                m_ViewLayer.ResizeToSafeArea(m_Canvas);
            }
        }

        void Init()
        {
            if (m_Views == null) return;

            foreach (var view in m_Views)
            {
                if (view != null)
                {
                    view.Hide();
                }
            }
            m_History?.Clear();
        }

        /// <summary>
        /// Finds the first registered UI View of the specified type
        /// </summary>
        /// <typeparam name="T">The View class to search for</typeparam>
        /// <returns>The instance of the View of the specified type. null if not found </returns>
        public T GetView<T>() where T : View
        {
            // Use cached dictionary for O(1) lookup
            if (m_ViewCache != null && m_ViewCache.TryGetValue(typeof(T), out View view))
            {
                return view as T;
            }
            return null;
        }

        /// <summary>
        /// Finds the View of the specified type and makes it visible
        /// </summary>
        /// <param name="keepInHistory">Pushes the current View to the history stack in case we want to go back to</param>
        /// <typeparam name="T">The View class to search for</typeparam>
        public void Show<T>(bool keepInHistory = true) where T : View
        {
            var view = GetView<T>();
            if (view != null)
            {
                Show(view, keepInHistory);
            }
            else
            {
                Debug.LogWarning($"[UIManager] View of type {typeof(T).Name} not found");
            }
        }

        /// <summary>
        /// Makes a View visible and hides others
        /// </summary>
        /// <param name="view">The view</param>
        /// <param name="keepInHistory">Pushes the current View to the history stack in case we want to go back to</param>
        public void Show(View view, bool keepInHistory = true)
        {
            if (view == null)
            {
                Debug.LogError("[UIManager] Attempted to show null view");
                return;
            }

            if (m_CurrentView != null)
            {
                if (keepInHistory)
                {
                    m_History?.Push(m_CurrentView);
                }

                m_CurrentView.Hide();
            }

            view.Show();
            m_CurrentView = view;
        }

        /// <summary>
        /// Goes to the page visible previously
        /// </summary>
        public void GoBack()
        {
            if (m_History.Count != 0)
            {
                Show(m_History.Pop(), false);
            }
        }
    }
}