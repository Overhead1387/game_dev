using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HyperCasual.Runner
{
    public class LoadLevelFromData : AbstractState
    {
        private readonly LevelData _levelData;
        private readonly SceneController _sceneController;
        private readonly GameObject[] _managerPrefabs;

        public LoadLevelFromData(SceneController sceneController, AbstractLevelData levelData, GameObject[] managerPrefabs)
        {
            _levelData = levelData as LevelData;
            _managerPrefabs = managerPrefabs;
            _sceneController = sceneController;
        }

        public override IEnumerator Execute()
        {
            if (_levelData == null)
                throw new Exception($"{nameof(_levelData)} is null!");

            yield return _sceneController.LoadNewScene(_levelData.name);

            foreach (var prefab in _managerPrefabs)
            {
                Object.Instantiate(prefab);
            }

            GameController.Instance.LoadLevel(_levelData);
        }
    }
}