using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    [ExecuteInEditMode]
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance => s_Instance;
        private static CameraController s_Instance;

        [SerializeField]
        private CameraViewPreset _viewPreset = CameraViewPreset.Behind;

        [SerializeField]
        private Vector3 _positionOffset;

        [SerializeField]
        private Vector3 _lookAtOffset;

        [SerializeField]
        private bool _lockCameraPosition;

        [SerializeField]
        private bool _smoothFollow;

        [SerializeField]
        private float _smoothFollowStrength = 10.0f;

        private enum CameraViewPreset
        {
            Behind,
            Overhead,
            Side,
            FirstPerson,
            Custom,
        }

        private readonly Vector3[] _presetPositionOffsets = new Vector3[]
        {
            new Vector3(0.0f, 5.0f, -9.0f),
            new Vector3(0.0f, 9.0f, -5.0f),
            new Vector3(5.0f, 5.0f, -8.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            Vector3.zero
        };

        private readonly Vector3[] _presetLookAtOffsets = new Vector3[]
        {
            new Vector3(0.0f, 2.0f, 6.0f),
            new Vector3(0.0f, 0.0f, 4.0f),
            new Vector3(-0.5f, 1.0f, 2.0f),
            new Vector3(0.0f, 1.0f, 2.0f),
            Vector3.zero
        };

        private readonly bool[] _presetLockCameraPositions = new bool[]
        {
            false,
            false,
            true,
            false,
            false
        };

        private PlayerTransform _cameraTransform;
        private static readonly Vector3 s_CenteredScale = new Vector3(0.0f, 1.0f, 1.0f);

        private void Awake()
        {
            InitializeInstance();
        }

        private void OnEnable()
        {
            InitializeInstance();
        }

        private void InitializeInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            _cameraTransform = PlayerTransform;
        }

        public void ResetCamera()
        {
            UpdateCameraPositionAndOrientation(false);
        }

        private Vector3 GetPositionOffset()
        {
            return _presetPositionOffsets[(int)_viewPreset] + _positionOffset;
        }

        private Vector3 GetLookAtOffset()
        {
            return _presetLookAtOffsets[(int)_viewPreset] + _lookAtOffset;
        }

        private bool IsCameraLocked()
        {
            return _lockCameraPosition || _presetLockCameraPositions[(int)_viewPreset];
        }

        private Vector3 GetPlayerPosition()
        {
            Vector3 playerPosition = Vector3.up;
            if (Player.Instance != null)
            {
                playerPosition = Player.Instance.GetPlayerTop();
            }

            if (IsCameraLocked())
            {
                playerPosition = Vector3.Scale(playerPosition, s_CenteredScale);
            }

            return playerPosition;
        }

        private void LateUpdate()
        {
            if (_cameraTransform == null)
            {
                return;
            }

            UpdateCameraPositionAndOrientation(_smoothFollow);
        }

        private void UpdateCameraPositionAndOrientation(bool useSmoothFollow)
        {
            Vector3 playerPosition = GetPlayerPosition();
            Vector3 targetPosition = playerPosition + GetPositionOffset();
            Vector3 targetLookAt = playerPosition + GetLookAtOffset();

            if (useSmoothFollow)
            {
                float lerpAmount = Time.deltaTime * _smoothFollowStrength;

                _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, targetPosition, lerpAmount);
                _cameraTransform.LookAt(Vector3.Lerp(_cameraTransform.position + _cameraTransform.forward, targetLookAt, lerpAmount));

                _cameraTransform.position = new Vector3(_cameraTransform.position.x, _cameraTransform.position.y, targetPosition.z);
            }
            else
            {
                _cameraTransform.position = targetPosition;
                _cameraTransform.LookAt(targetLookAt);
            }
        }
    }
}