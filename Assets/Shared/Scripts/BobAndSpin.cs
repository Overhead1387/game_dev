using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Core
{
    public class BobAndSpin : MonoBehaviour
    {
        public bool UsePositionBasedOffset = true;
        public float PositionBasedScale = 2.0f;

        public bool Bob = true;
        public float BobSpeed = 5.0f;
        public float BobHeight = 0.2f;

        public bool Spin = true;
        public float SpinSpeed = 180.0f;

        Transform m_Transform;
        Vector3 m_StartPosition;
        Quaternion m_StartRotation;

        void Awake()
        {
            m_Transform = transform;
            m_StartPosition = m_Transform.position;
            m_StartRotation = m_Transform.rotation;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;
            float offset = (UsePositionBasedOffset) ? m_StartPosition.z * PositionBasedScale + Time.time : Time.time;

            if (Bob)
            {
                Vector3 newPosition = m_StartPosition + Vector3.up * Mathf.Sin(offset * BobSpeed) * BobHeight;
                m_Transform.position = Vector3.Lerp(m_Transform.position, newPosition, deltaTime * BobSpeed);
            }

            if (Spin)
            {
                Quaternion targetRotation = m_StartRotation * Quaternion.AngleAxis(offset * SpinSpeed, Vector3.up);
                m_Transform.rotation = Quaternion.Slerp(m_Transform.rotation, targetRotation, deltaTime * SpinSpeed * 0.1f);
            }
        }
    }
}