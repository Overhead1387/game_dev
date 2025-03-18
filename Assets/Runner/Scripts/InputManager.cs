using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace HyperCasual.Runner
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance => s_Instance;
        private static InputController s_Instance;

        [SerializeField]
        private float _sensitivity = 1.5f;

        private bool _hasInput;
        private Vector3 _currentInputPosition;
        private Vector3 _previousInputPosition;

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
        }

        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        private void Update()
        {
            if (Player.Instance == null)
            {
                return;
            }

#if UNITY_EDITOR
            _currentInputPosition = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.isPressed)
            {
                if (!_hasInput)
                {
                    _previousInputPosition = _currentInputPosition;
                }
                _hasInput = true;
            }
            else
            {
                _hasInput = false;
            }
#else
            if (Touch.activeTouches.Count > 0)
            {
                _currentInputPosition = Touch.activeTouches[0].screenPosition;

                if (!_hasInput)
                {
                    _previousInputPosition = _currentInputPosition;
                }

                _hasInput = true;
            }
            else
            {
                _hasInput = false;
            }
#endif

            if (_hasInput)
            {
                float normalizedDelta = (_currentInputPosition.x - _previousInputPosition.x) / Screen.width * _sensitivity;
                Player.Instance.SetDeltaPosition(normalizedDelta);
            }
            else
            {
                Player.Instance.CancelMovement();
            }

            _previousInputPosition = _currentInputPosition;
        }
    }
}