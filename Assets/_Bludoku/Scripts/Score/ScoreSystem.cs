using UnityEngine;
using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Score
{
    public static class ScoreSystem
    {
        public static ScoreData Data { get; private set; } = new ScoreData();

        private static string comboKey = string.Empty;

        private const string SCORE_KEY = "CurrentScore";
        private const string HIGH_SCORE_KEY = "HighScore";
        private const int SCORE_FOR_SET = 1;

        public static void SetComboCount(int comboCount)
        {
            Data.ComboCount = comboCount < 0 ? 0 : comboCount;
        }

        public static void LoadScore()
        {
            comboKey = ConfigService.Instance.Get<ComboSettings>().PlayerPrefsKey;
            Data.Score = PlayerPrefs.GetInt(SCORE_KEY, 0);
            Data.HighScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
            Data.ComboCount = PlayerPrefs.GetInt(comboKey, 0);
        }

        public static void AddSetScore(int setsCount, int multiplier)
        {
            if (setsCount <= 0)
            {
                SaveScore();
                return;
            }

            int safeMultiplier = multiplier < 1 ? 1 : multiplier;
            AddScore(setsCount * SCORE_FOR_SET * safeMultiplier);
        }

        public static void AddScore(int score)
        {
            Data.Score += score;
            if (Data.HighScore < Data.Score) Data.HighScore = Data.Score;

            SaveScore();
        }

        public static void ResetScore()
        {
            Data.Score = 0;
            Data.ComboCount = 0;
            SaveScore();
        }

        private static void SaveScore()
        {
            PlayerPrefs.SetInt(comboKey, Data.ComboCount);
            PlayerPrefs.SetInt(SCORE_KEY, Data.Score);
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, Data.HighScore);
            PlayerPrefs.Save();
        }
    }
}
