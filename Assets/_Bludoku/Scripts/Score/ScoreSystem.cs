using _Bludoku.Scripts.Config;
using _Bludoku.Scripts.Save;

namespace _Bludoku.Scripts.Score
{
    public static class ScoreSystem
    {
        public static ScoreData Data { get; private set; } = new ScoreData();

        private static readonly SaveSettings saveSettings = SaveService.Instance.Settings;

        private const int SCORE_FOR_SET = 1;

        public static void SetComboCount(int comboCount)
        {
            Data.ComboCount = comboCount < 0 ? 0 : comboCount;
        }

        public static void LoadScore()
        {
            Data = SaveService.Instance.Get(saveSettings.ScoreDataKey, new ScoreData());
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
            SaveService.Instance.Save(saveSettings.ScoreDataKey, Data);
        }
    }
}
