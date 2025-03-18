using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class Portal : SpawnableEntity
    {
        private const string PlayerTag = "Player";

        [SerializeField]
        private PortalEffect _effectType;

        [SerializeField]
        private float _effectValue;

        [SerializeField]
        private RectTransform _label;

        private bool _isActivated;
        private Vector3 _labelInitialScale;

        private enum PortalEffect
        {
            ModifySpeed,
            ModifySize,
        }

        public override void SetScale(Vector3 scale)
        {
            if (_label != null)
            {
                float xFactor = Mathf.Min(scale.y / scale.x, 1.0f);
                float yFactor = Mathf.Min(scale.x / scale.y, 1.0f);
                _label.localScale = Vector3.Scale(_labelInitialScale, new Vector3(xFactor, yFactor, 1.0f));

                _transform.localScale = scale;
            }
        }

        public override void ResetEntity()
        {
            _isActivated = false;
        }

        protected override void Awake()
        {
            base.Awake();

            if (_label != null)
            {
                _labelInitialScale = _label.localScale;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag) && !_isActivated)
            {
                ActivatePortal();
            }
        }

        private void ActivatePortal()
        {
            switch (_effectType)
            {
                case PortalEffect.ModifySpeed:
                    Player.Instance.AdjustSpeed(_effectValue);
                    break;

                case PortalEffect.ModifySize:
                    Player.Instance.AdjustScale(_effectValue);
                    break;
            }

            _isActivated = true;
        }
    }
}