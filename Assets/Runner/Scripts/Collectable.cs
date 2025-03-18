using HyperCasual.Gameplay;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class Collectible : SpawnableEntity
    {
        [SerializeField]
        private SoundID _sound = SoundID.None;

        private const string PlayerTag = "Player";

        public ItemPickedEvent PickedEvent;
        public int Count;

        private bool _isCollected;
        private Renderer[] _renderers;

        public override void ResetEntity()
        {
            _isCollected = false;

            foreach (Renderer renderer in _renderers)
            {
                renderer.enabled = true;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _renderers = gameObject.GetComponentsInChildren<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTag) && !_isCollected)
            {
                Collect();
            }
        }

        private void Collect()
        {
            if (PickedEvent != null)
            {
                PickedEvent.Count = Count;
                PickedEvent.Raise();
            }

            foreach (Renderer renderer in _renderers)
            {
                renderer.enabled = false;
            }

            _isCollected = true;
            AudioManager.Instance.PlayEffect(_sound);
        }
    }
}