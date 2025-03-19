using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HyperCasual.Runner
{
    /// <summary>
    /// Manages touch and mouse input for the runner game, implementing the Singleton pattern.
    /// Handles input detection and movement calculations for the PlayerController with platform-specific optimizations.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        // Singleton instance accessor
        public static InputManager Instance => s_Instance;
        private static InputManager s_Instance;

        [SerializeField, Tooltip("Controls the sensitivity of horizontal movement")]
        private float m_InputSensitivity = 1.5f;

        private bool m_HasInput;
        private Vector3 m_InputPosition;
        private Vector3 m_PreviousInputPosition;
        private PlayerController m_CachedPlayerController;

        void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
        }

        void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        void Start()
        {
            m_CachedPlayerController = PlayerController.Instance;
        }

        /// <summary>
        /// Processes input and updates player movement based on platform-specific input methods.
        /// </summary>
        void Update()
        {
            if (m_CachedPlayerController == null)
            {
                m_CachedPlayerController = PlayerController.Instance;
                if (m_CachedPlayerController == null)
                    return;
            }

#if UNITY_EDITOR
            m_InputPosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.isPressed)
            {
                if (!m_HasInput)
                {
                    m_PreviousInputPosition = m_InputPosition;
                }
                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }
#else
            if (Touch.activeTouches.Count > 0)
            {
                m_InputPosition = Touch.activeTouches[0].screenPosition;

                if (!m_HasInput)
                {
                    m_PreviousInputPosition = m_InputPosition;
                }
                
                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }
#endif

            if (m_HasInput)
            {
                float normalizedDeltaPosition = (m_InputPosition.x - m_PreviousInputPosition.x) / Screen.width * m_InputSensitivity;
                m_CachedPlayerController.SetDeltaPosition(normalizedDeltaPosition);
            }
            else
            {
                m_CachedPlayerController.CancelMovement();
            }

            m_PreviousInputPosition = m_InputPosition;
        }
    }
}

