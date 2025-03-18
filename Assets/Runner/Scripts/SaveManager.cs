using HyperCasual.Core;
using UnityEngine;

namespace HyperCasual.Runner
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance => s_Instance;
        private static DataManager s_Instance;

        private const string LevelProgressKey = "LevelProgress";
        private const string CurrencyKey = "Currency";
        private const string XpKey = "Xp";
        private const string AudioSettingsKey = "AudioSettings";
        private const string QualityLevelKey = "QualityLevel";

        private void Awake()
        {
            s_Instance = this;
        }

        public int LevelProgress
        {
            get => PlayerPrefs.GetInt(LevelProgressKey);
            set => PlayerPrefs.SetInt(LevelProgressKey, value);
        }

        public int Currency
        {
            get => PlayerPrefs.GetInt(CurrencyKey);
            set => PlayerPrefs.SetInt(CurrencyKey, value);
        }

        public float XP
        {
            get => PlayerPrefs.GetFloat(XpKey);
            set => PlayerPrefs.SetFloat(XpKey, value);
        }

        public bool IsQualityLevelSaved => PlayerPrefs.HasKey(QualityLevelKey);

        public int QualityLevel
        {
            get => PlayerPrefs.GetInt(QualityLevelKey);
            set => PlayerPrefs.SetInt(QualityLevelKey, value);
        }

        public AudioSettings LoadAudioSettings()
        {
            return PlayerPrefsUtils.Read<AudioSettings>(AudioSettingsKey);
        }

        public void SaveAudioSettings(AudioSettings audioSettings)
        {
            PlayerPrefsUtils.Write(AudioSettingsKey, audioSettings);
        }
    }
}