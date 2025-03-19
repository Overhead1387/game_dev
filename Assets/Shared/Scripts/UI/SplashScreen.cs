using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// Splash screen that displays on game startup.
    /// Handles initialization sequence and transitions to the main menu.
    /// Can be customized to show company logo, loading progress, or other startup information.
    /// </summary>
    public class SplashScreen : View
    {
        [SerializeField, Tooltip("Duration to display the splash screen in seconds")]
        float m_DisplayDuration = 2f;

        void Start()
        {
            StartCoroutine(SplashSequence());
        }

        IEnumerator SplashSequence()
        {
            // Wait for the specified duration
            yield return new WaitForSeconds(m_DisplayDuration);
            
            // Transition to the main menu
            UIManager.Instance.Show<MainMenu>();
        }
    }
}