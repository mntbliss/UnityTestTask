using UnityEngine;
using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Score
{
    public static class ScoreSystem
    {
        private static int score;
        private static int highScore;
        private static int comboCount;
        private static string comboKey = string.Empty;

        private const string SCORE_KEY = "CurrentScore";
        private const string HIGH_SCORE_KEY = "HighScore";
        private const int SCORE_FOR_SET = 1;

        public static int Score => score;
        public static int HighScore => highScore;
        public static int ComboCount => comboCount;

        public static void SetComboCount(int comboCount)
        {
            ScoreSystem.comboCount = comboCount < 0 ? 0 : comboCount;
        }

        public static void LoadScore()
        {
            comboKey = ConfigService.Instance.Get<ComboSettings>().PlayerPrefsKey;
            score = PlayerPrefs.GetInt(SCORE_KEY, 0);
            highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
            comboCount = PlayerPrefs.GetInt(comboKey, 0);
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
            ScoreSystem.score = Score + score;
            if (HighScore < Score)
            {
                highScore = Score;
            }

            SaveScore();
        }

        public static void ResetScore()
        {
            score = 0;
            comboCount = 0;
            SaveScore();
        }

        private static void SaveScore()
        {
            PlayerPrefs.SetInt(comboKey, ComboCount);
            PlayerPrefs.SetInt(SCORE_KEY, Score);
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, HighScore);
            PlayerPrefs.Save();
        }
    }
}
