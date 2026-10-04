using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Score
{
    public class ScoreComboSystem
    {
        private ComboSettings comboSettings;
        private int comboCount;

        public int ComboCount => comboCount;
        public int Multiplier => comboCount > 0 ? comboCount : comboSettings.MinMultiplier;
        public ComboSettings Settings => comboSettings;

        public void Initialize()
        {
            comboSettings = ConfigService.Instance.Get<ComboSettings>();
        }

        public void FigurePlaced(int clearedCount)
        {
            if (clearedCount <= 0)
            {
                comboCount = 0;
                return;
            }

            comboCount++;
        }

        public void Reset()
        {
            comboCount = 0;
        }

        public void SetComboCount(int comboCount)
        {
            this.comboCount = comboCount < 0 ? 0 : comboCount;
        }
    }
}
