using _Bludoku.Scripts.Config;
using _Bludoku.Scripts.Save;

namespace _Bludoku.Scripts.MainMenu
{
    public static class SaveSystem
    {
        private static readonly SaveSettings saveSettings = SaveService.Instance.Settings;

        public static int CurrentLevelIndex
        {
            get
            {
                string value = SaveService.Instance.Get(saveSettings.LevelIndexKey, "0");
                return int.TryParse(value, out int index) ? index : 0;
            }
        }

        public static int CurrentLevelNumber => CurrentLevelIndex + 1;

        public static void SaveLevel(int index)
        {
            SaveService.Instance.Save(saveSettings.LevelIndexKey, index.ToString());
        }

        public static void AdvanceLevel(int totalLevels)
        {
            if (totalLevels <= 0) return;

            int next = (CurrentLevelIndex + 1) % totalLevels;
            SaveLevel(next);
        }
    }
}
