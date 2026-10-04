using UnityEngine;
using _Bludoku.Scripts.Config;
using _Bludoku.Scripts.Save;

namespace _Bludoku.Scripts.MainMenu
{
    public static class SettingsManager
    {
        private static readonly SaveSettings saveSettings = SaveService.Instance.Settings;

        public static bool IsSoundEnabled { get; private set; }
        public static bool IsMusicEnabled { get; private set; }
        public static bool IsNightModeEnabled { get; private set; }
        public static bool IsVibrationEnabled { get; private set; }

        static SettingsManager()
        {
            IsSoundEnabled = !SaveService.Instance.Has(saveSettings.SoundKey, "0");
            IsMusicEnabled = !SaveService.Instance.Has(saveSettings.MusicKey, "0");
            IsNightModeEnabled = !SaveService.Instance.Has(saveSettings.NightModeKey, "0");
            IsVibrationEnabled = !SaveService.Instance.Has(saveSettings.VibrationKey, "0");
            ApplySettings();
        }

        public static void SetSound(bool enabled)
        {
            IsSoundEnabled = enabled;
            SaveService.Instance.Save(saveSettings.SoundKey, enabled ? "1" : "0");
            ApplySettings();
        }

        public static void SetMusic(bool enabled)
        {
            IsMusicEnabled = enabled;
            SaveService.Instance.Save(saveSettings.MusicKey, enabled ? "1" : "0");
            ApplySettings();
        }

        public static void SetNightMode(bool enabled)
        {
            IsNightModeEnabled = enabled;
            SaveService.Instance.Save(saveSettings.NightModeKey, enabled ? "1" : "0");
            ApplySettings();
        }

        public static void SetVibration(bool enabled)
        {
            IsVibrationEnabled = enabled;
            SaveService.Instance.Save(saveSettings.VibrationKey, enabled ? "1" : "0");
            ApplySettings();
        }

        static void ApplySettings()
        {
            AudioListener.volume = IsSoundEnabled ? 1f : 0f;
        }
    }
}
