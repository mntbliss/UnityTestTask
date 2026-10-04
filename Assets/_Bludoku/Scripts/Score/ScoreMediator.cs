using _Bludoku.Scripts.Analytics;
using _Bludoku.Scripts.Boards;
using UnityEngine;

namespace _Bludoku.Scripts.Score
{
    public class ScoreMediator : MonoBehaviour
    {
        [SerializeField] private ScoreView scoreView;
        [SerializeField] private Board board;
        [SerializeField] private ScoreBoosterView boosterView;
        [SerializeField] private ScoreComboView comboView;

        private readonly ScoreComboSystem scoreComboSystem = new();
        private readonly AnalyticsKeys analyticsKeys = AnalyticsService.Instance.Settings.Keys;

        private void Awake()
        {
            board.OnFigurePlaced += FigurePlaced;
            scoreComboSystem.Initialize();
            comboView.Setup(scoreComboSystem.Settings);
        }

        private void Start()
        {
            ScoreSystem.LoadScore();
            scoreComboSystem.SetComboCount(ScoreSystem.Data.ComboCount);
            UpdateComboViews(false);
            scoreView.UpdateScore(false);
        }

        public void ResetScore()
        {
            ScoreSystem.ResetScore();
            scoreComboSystem.Reset();
            UpdateView();
        }

        private void FigurePlaced(ClearResult result)
        {
            int previousCombo = scoreComboSystem.ComboCount;

            scoreComboSystem.FigurePlaced(result.ClearedCount);
            ScoreSystem.SetComboCount(scoreComboSystem.ComboCount);
            ScoreSystem.AddSetScore(result.ClearedCount, scoreComboSystem.Multiplier);
            UpdateComboViews();
            scoreView.UpdateScore();

            TrackComboAnalytics(previousCombo, result);
        }

        private void TrackComboAnalytics(int previousCombo, ClearResult result)
        {
            int comboCount = scoreComboSystem.ComboCount;

            if (comboCount > previousCombo && comboCount >= scoreComboSystem.Settings.MinVisibleCombo)
            {
                AnalyticsService.Instance.Track(
                    analyticsKeys.ComboReached,
                    $"Player reached combo {comboCount}",
                    ("combo", comboCount.ToString()),
                    ("x", result.PlaceX.ToString()),
                    ("y", result.PlaceY.ToString()));
            }

            if (previousCombo > 0 && comboCount == 0)
            {
                AnalyticsService.Instance.Track(
                    analyticsKeys.ComboBroken,
                    $"Player broke combo {previousCombo}",
                    ("previous_combo", previousCombo.ToString()));
            }

            if (result.ClearedCount > 0 && scoreComboSystem.Multiplier > 1)
            {
                AnalyticsService.Instance.Track(
                    analyticsKeys.BonusReceived,
                    $"Player received x{scoreComboSystem.Multiplier} bonus for clearing {result.ClearedCount} cells",
                    ("multiplier", scoreComboSystem.Multiplier.ToString()),
                    ("cleared_count", result.ClearedCount.ToString()),
                    ("combo", comboCount.ToString()));
            }
        }

        private void UpdateView()
        {
            UpdateComboViews(false);
            scoreView.UpdateScore(false);
        }

        private void UpdateComboViews(bool animate = true)
        {
            int comboCount = scoreComboSystem.ComboCount;
            comboView.UpdateCombo(comboCount, animate);
            boosterView.SetBoosterEnabled(comboCount >= scoreComboSystem.Settings.MinVisibleCombo);
        }
    }
}
