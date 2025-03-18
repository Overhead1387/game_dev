using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class DataManager : MonoBehaviour
    {
        public class SaveManager : MonoBehaviour
            {
                private void Awake()
                {
                    Debug.LogWarning("SaveManager is deprecated. Please use DataManager instead.");
                }

                public static SaveManager Instance => DataManager.Instance as SaveManager;

                // Forward properties and methods
                public int LevelProgress
                {
                    get => DataManager.Instance.LevelProgress;
                    set => DataManager.Instance.LevelProgress = value;
                }

                public int Currency
                {
                    get => DataManager.Instance.Currency;
                    set => DataManager.Instance.Currency = value;
                }

                public float XP
                {
                    get => DataManager.Instance.XP;
                    set => DataManager.Instance.XP = value;
                }

                public bool IsQualityLevelSaved => DataManager.Instance.IsQualityLevelSaved;

                public int QualityLevel
                {
                    get => DataManager.Instance.QualityLevel;
                    set => DataManager.Instance.QualityLevel = value;
                }

                public AudioSettings LoadAudioSettings() => DataManager.Instance.LoadAudioSettings();
                public void SaveAudioSettings(AudioSettings audioSettings) => DataManager.Instance.SaveAudioSettings(audioSettings);
            }
}