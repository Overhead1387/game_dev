using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HyperCasual.Runner
{
    [ExecuteInEditMode]
    public class SpawnableEntity : MonoBehaviour
    {
        protected PlayerTransform _PlayerTransform;

        private LevelData _levelData;
        private Vector3 _position;
        private Color _baseColor;
        private bool _snappedThisFrame;
        private float _previousGridSize;

        private MeshRenderer[] _meshRenderers;

        [SerializeField]
        private bool _snapToGrid = true;

        public Vector3 SavedPosition => _position;

        public Color BaseColor => _baseColor;

        protected virtual void Awake()
        {
            _PlayerTransform = PlayerTransform;

            if (_meshRenderers == null || _meshRenderers.Length == 0)
            {
                _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            }

            if (_meshRenderers != null && _meshRenderers.Length > 0)
            {
                _baseColor = _meshRenderers[0].sharedMaterial.color;
            }

            if (LevelLoader.Instance != null)
            {
#if UNITY_EDITOR
                if (PrefabUtility.IsPartOfNonAssetPrefabInstance(gameObject))
#endif
                _PlayerTransform.SetParent(LevelLoader.Instance.PlayerTransform);
            }
        }

        public virtual void SetBaseColor(Color baseColor)
        {
            _baseColor = baseColor;

            if (_meshRenderers == null || _meshRenderers.Length == 0)
            {
                _meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            }

            if (_meshRenderers != null)
            {
                foreach (MeshRenderer meshRenderer in _meshRenderers)
                {
                    if (meshRenderer != null)
                    {
                        Material material = new Material(meshRenderer.sharedMaterial);
                        material.color = _baseColor;
                        meshRenderer.sharedMaterial = material;
                    }
                }
            }
        }

        public virtual void SetScale(Vector3 scale)
        {
           _PlayerTransform.localScale = scale;
        }

        public void SetLevelData(LevelData levelData)
        {
            if (levelData == null)
            {
                return;
            }

            _levelData = levelData;
        }

        public virtual void ResetEntity() {}

        protected virtual void OnEnable()
        {
            _PlayerTransform = PlayerTransform;
            _position = _PlayerTransform.position;
            _PlayerTransform.hasChanged = false;

            if (LevelLoader.Instance != null && !Application.isPlaying)
            {
                SetLevelData(LevelLoader.Instance.Data);
                SnapToGrid();
            }
        }

        protected virtual void Update()
        {
            if (!Application.isPlaying && _levelData != null)
            {
                if (_PlayerTransform.hasChanged)
                {
                    _position = _PlayerTransform.position;
                    _PlayerTransform.hasChanged = false;

                    if (_levelData.SnapToGrid)
                    {
                        SnapToGrid();
                    }

                    SetScale(_PlayerTransform.localScale);
                }
                else if (_previousGridSize != _levelData.GridSize)
                {
                    SnapToGrid();
                }
            }
        }

        protected virtual void SnapToGrid()
        {
            if (!_snapToGrid || _levelData == null)
            {
                return;
            }

            Vector3 position = _position;

            position.x = _levelData.GridSize * Mathf.Round(position.x / _levelData.GridSize);
            position.z = _levelData.GridSize * Mathf.Round(position.z / _levelData.GridSize);

            _PlayerTransform.position = position;

            _previousGridSize = _levelData.GridSize;

            _PlayerTransform.hasChanged = false;
        }
    }
}