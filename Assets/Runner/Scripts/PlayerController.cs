using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class Player : MonoBehaviour
    {
        public static Player Instance => s_Instance;
        private static Player s_Instance;

        [SerializeField] private Animator _animator;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private PlayerSpeedPreset _speedPreset = PlayerSpeedPreset.Medium;
        [SerializeField] private float _customSpeed = 10.0f;
        [SerializeField] private float _acceleration = 10.0f;
        [SerializeField] private float _deceleration = 20.0f;
        [SerializeField] private float _horizontalSpeedFactor = 0.5f;
        [SerializeField] private float _scaleVelocity = 2.0f;
        [SerializeField] private bool _autoMoveForward = true;

        private Vector3 _lastPosition;
        private float _startHeight;

        private const float MinimumScale = 0.1f;
        private static readonly string SpeedParameter = "Speed";

        private enum PlayerSpeedPreset
        {
            Slow,
            Medium,
            Fast,
            Custom
        }

        private PlayerTransform _PlayerTransform;
        private Vector3 _startPosition;
        private bool _hasInput;
        private float _maxXPosition;
        private float _xPosition;
        private float _zPosition;
        private float _targetXPosition;
        private float _currentSpeed;
        private float _targetSpeed;
        private Vector3 _currentScale;
        private Vector3 _targetScale;
        private Vector3 _defaultScale;

        private const float HalfWidth = 0.5f;

        public PlayerTransform PlayerTransform => _PlayerTransform;
        public float CurrentSpeed => _currentSpeed;
        public float TargetSpeed => _targetSpeed;
        public float MinimumScaleValue => MinimumScale;
        public Vector3 CurrentScale => _currentScale;
        public Vector3 TargetScale => _targetScale;
        public Vector3 DefaultScale => _defaultScale;
        public float StartHeight => _startHeight;
        public float TargetXPosition => _targetXPosition;
        public float MaxXPosition => _maxXPosition;

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            Initialize();
        }

        public void Initialize()
        {
            _PlayerTransform = PlayerTransform;
            _startPosition = _PlayerTransform.position;
            _defaultScale = _PlayerTransform.localScale;
            _currentScale = _defaultScale;
            _targetScale = _currentScale;

            _startHeight = _skinnedMeshRenderer != null ? _skinnedMeshRenderer.bounds.size.y : 1.0f;

            ResetSpeed();
        }

        public float GetDefaultSpeed()
        {
            switch (_speedPreset)
            {
                case PlayerSpeedPreset.Slow: return 5.0f;
                case PlayerSpeedPreset.Medium: return 10.0f;
                case PlayerSpeedPreset.Fast: return 20.0f;
                default: return _customSpeed;
            }
        }

        public void AdjustSpeed(float speed)
        {
            _targetSpeed += speed;
            _targetSpeed = Mathf.Max(0.0f, _targetSpeed);
        }

        public void ResetSpeed()
        {
            _currentSpeed = 0.0f;
            _targetSpeed = GetDefaultSpeed();
        }

        public void AdjustScale(float scale)
        {
            _targetScale += Vector3.one * scale;
            _targetScale = Vector3.Max(_targetScale, Vector3.one * MinimumScale);
        }

        public void ResetScale()
        {
            _currentScale = _defaultScale;
            _targetScale = _defaultScale;
        }

        public Vector3 GetPlayerTop()
        {
            return _PlayerTransform.position + Vector3.up * (_startHeight * _currentScale.y - _startHeight);
        }

        public void SetDeltaPosition(float normalizedDelta)
        {
            if (_maxXPosition == 0.0f)
                Debug.LogError("Player cannot move because SetMaxXPosition has never been called or Level Width is 0. If you are in the LevelEditor scene, ensure a level has been loaded in the LevelEditor Window!");

            float fullWidth = _maxXPosition * 2.0f;
            _targetXPosition = _targetXPosition + fullWidth * normalizedDelta;
            _targetXPosition = Mathf.Clamp(_targetXPosition, -_maxXPosition, _maxXPosition);
            _hasInput = true;
        }

        public void CancelMovement()
        {
            _hasInput = false;
        }

        public void SetMaxXPosition(float levelWidth)
        {
            _maxXPosition = levelWidth * HalfWidth;
        }

        public void ResetPlayer()
        {
            _PlayerTransform.position = _startPosition;
            _xPosition = 0.0f;
            _zPosition = _startPosition.z;
            _targetXPosition = 0.0f;
            _lastPosition = _PlayerTransform.position;
            _hasInput = false;
            ResetSpeed();
            ResetScale();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            if (!Approximately(_PlayerTransform.localScale, _targetScale))
            {
                _currentScale = Vector3.Lerp(_currentScale, _targetScale, deltaTime * _scaleVelocity);
                _PlayerTransform.localScale = _currentScale;
            }

            if (!_autoMoveForward && !_hasInput)
                Decelerate(deltaTime, 0.0f);
            else if (_targetSpeed < _currentSpeed)
                Decelerate(deltaTime, _targetSpeed);
            else if (_targetSpeed > _currentSpeed)
                Accelerate(deltaTime, _targetSpeed);

            float speed = _currentSpeed * deltaTime;
            _zPosition += speed;

            if (_hasInput)
            {
                float horizontalSpeed = speed * _horizontalSpeedFactor;
                float newPositionTarget = Mathf.Lerp(_xPosition, _targetXPosition, horizontalSpeed);
                float newPositionDifference = Mathf.Clamp(newPositionTarget - _xPosition, -horizontalSpeed, horizontalSpeed);
                _xPosition += newPositionDifference;
            }

            _PlayerTransform.position = new Vector3(_xPosition, _PlayerTransform.position.y, _zPosition);

            if (_animator != null && deltaTime > 0.0f)
                _animator.SetFloat(SpeedParameter, (_PlayerTransform.position - _lastPosition).magnitude / deltaTime);

            if (_PlayerTransform.position != _lastPosition)
                _PlayerTransform.forward = Vector3.Lerp(_PlayerTransform.forward, (_PlayerTransform.position - _lastPosition).normalized, speed);

            _lastPosition = _PlayerTransform.position;
        }

        private void Accelerate(float deltaTime, float targetSpeed)
        {
            _currentSpeed = Mathf.Min(_currentSpeed + deltaTime * _acceleration, targetSpeed);
        }

        private void Decelerate(float deltaTime, float targetSpeed)
        {
            _currentSpeed = Mathf.Max(_currentSpeed - deltaTime * _deceleration, targetSpeed);
        }

        private bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }
    }
}