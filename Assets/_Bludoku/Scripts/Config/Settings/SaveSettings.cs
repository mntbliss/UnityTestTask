using UnityEngine;

namespace _Bludoku.Scripts.Config
{
    [CreateAssetMenu(fileName = "Save Settings", menuName = "Data/Save Settings")]
    public class SaveSettings : ConfigItem
    {
        [Space]
        public string ScoreDataKey = "ScoreData";
        public string FiguresSaveKey = "FiguresSave";
        public string LevelIndexKey = "Save_LevelIndex";

        [Space]
        public string SoundKey = "Settings_Sound";
        public string MusicKey = "Settings_Music";
        public string NightModeKey = "Settings_NightMode";
        public string VibrationKey = "Settings_Vibration";
    }
}
