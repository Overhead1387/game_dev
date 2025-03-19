using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to manage camera movement
    /// in a Runner game.
    /// </summary>
    [ExecuteInEditMode]
    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the CameraManager.
        /// </summary>
        public static CameraManager Instance => s_Instance;
        static CameraManager s_Instance;

        [SerializeField]
        CameraAnglePreset m_CameraAnglePreset = CameraAnglePreset.Behind;

        [SerializeField]
        Vector3 m_Offset;

        [SerializeField]
        Vector3 m_LookAtOffset;

        [SerializeField]
        bool m_LockCameraPosition;

        [SerializeField]
        bool m_SmoothCameraFollow;

        [SerializeField]
        float m_SmoothCameraFollowStrength = 10.0f;

        enum CameraAnglePreset
        {
            Behind,
            Overhead,
            Side,
            FirstPerson,
            Custom,
        }

        Vector3[] m_PresetOffsets = new Vector3[]
        {
            new Vector3(0.0f, 5.0f, -9.0f), // Behind
            new Vector3(0.0f, 9.0f, -5.0f), // Overhead
            new Vector3(5.0f, 5.0f, -8.0f), // Side
            new Vector3(0.0f, 1.0f, 0.0f),  // FirstPerson
            Vector3.zero                    // Custom
        };

        Vector3[] m_PresetLookAtOffsets = new Vector3[]
        {
            new Vector3(0.0f, 2.0f, 6.0f),  // Behind
            new Vector3(0.0f, 0.0f, 4.0f),  // Overhead
            new Vector3(-0.5f, 1.0f, 2.0f), // Side
            new Vector4(0.0f, 1.0f, 2.0f),  // FirstPerson
            Vector3.zero                    // Custom
        };

        bool[] m_PresetLockCameraPosition = new bool[]
        {
            false, // Behind
            false, // Overhead
            true,  // Side
            false, // FirstPerson
            false  // Custom
        };

        Transform m_Transform;
        Vector3 m_PrevLookAtOffset;

        static readonly Vector3 k_CenteredScale = new Vector3(0.0f, 1.0f, 1.0f);

        void Awake()
        {
            SetupInstance();
        }

        void OnEnable()
        {
            SetupInstance();
        }

        void SetupInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            m_Transform = transform;
        }

        /// <summary>
        /// Reset the camera to its starting position relative
        /// to the player.
        /// </summary>
        public void ResetCamera()
        {
            SetCameraPositionAndOrientation(false);
        }

        Vector3 GetCameraOffset()
        {
            return m_PresetOffsets[(int)m_CameraAnglePreset] + m_Offset;
        }

        Vector3 GetCameraLookAtOffset()
        {
            return m_PresetLookAtOffsets[(int)m_CameraAnglePreset] + m_LookAtOffset;
        }

        bool GetCameraLockStatus()
        {
            if (m_LockCameraPosition)
            {
                return true;
            }

            return m_PresetLockCameraPosition[(int)m_CameraAnglePreset];
        }

        Vector3 GetPlayerPosition()
        {
            Vector3 playerPosition = Vector3.up;
            if (PlayerController.Instance != null) 
            {
                playerPosition = PlayerController.Instance.GetPlayerTop();
            }

            if (GetCameraLockStatus())
            {
                playerPosition = Vector3.Scale(playerPosition, k_CenteredScale);
            }

            return playerPosition;
        }

        private Vector3 m_CachedOffset;
        private Vector3 m_CachedLookAtOffset;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_CurrentLookAtVelocity;
        private const float k_PositionSmoothTime = 0.1f;
        private const float k_RotationSmoothTime = 0.15f;

        void LateUpdate()
        {
            if (m_Transform == null || !Application.isPlaying)
                return;

            SetCameraPositionAndOrientation(m_SmoothCameraFollow);
        }

        void SetCameraPositionAndOrientation(bool smoothCameraFollow)
        {
            Vector3 playerPosition = GetPlayerPosition();
            m_CachedOffset = playerPosition + GetCameraOffset();
            m_CachedLookAtOffset = playerPosition + GetCameraLookAtOffset();

            if (smoothCameraFollow)
            {
                float lerpAmount = Time.deltaTime * m_SmoothCameraFollowStrength;
                
                // Use SmoothDamp for smoother camera movement and rotation
                var targetPos = m_CachedOffset;
                var currentPos = m_Transform.position;
                
                currentPos = Vector3.SmoothDamp(
                    currentPos,
                    targetPos,
                    ref m_CurrentVelocity,
                    k_PositionSmoothTime
                );

                // Smoothly interpolate look-at position for more natural camera rotation
                var currentLookAt = m_Transform.position + m_Transform.forward;
                var targetLookAt = m_CachedLookAtOffset;
                
                var smoothLookAt = Vector3.SmoothDamp(
                    currentLookAt,
                    targetLookAt,
                    ref m_CurrentLookAtVelocity,
                    k_RotationSmoothTime
                );

                // Update position and rotation
                m_Transform.position = currentPos;
                m_Transform.LookAt(smoothLookAt);

                // Lock Z position to target
                m_Transform.position = new Vector3(
                    m_Transform.position.x,
                    m_Transform.position.y,
                    m_CachedOffset.z
                );
            }
            else
            {
                m_Transform.position = m_CachedOffset;
                m_Transform.LookAt(m_CachedLookAtOffset);
            }
        }
    }
}

