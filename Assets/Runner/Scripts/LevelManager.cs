using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasual.Runner
{
    [ExecuteInEditMode]
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance => s_Instance;
        private static LevelLoader s_Instance;

        public LevelData Data
        {
            get => _data;
            set
            {
                _data = value;
                if (_data != null && Player.Instance != null)
                {
                    Player.Instance.SetMaxXPosition(_data.Width);
                }
            }
        }
        private LevelData _data;

        private readonly List<SpawnableEntity> _activeEntities = new List<SpawnableEntity>();

        public void AddEntity(SpawnableEntity entity)
        {
            _activeEntities.Add(entity);
        }

        public void ResetEntities()
        {
            foreach (SpawnableEntity entity in _activeEntities)
            {
                entity.ResetEntity();
            }
        }

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
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
                return;
            }

            s_Instance = this;
        }
    }
}