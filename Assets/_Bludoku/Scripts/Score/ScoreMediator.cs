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
            scoreComboSystem.FigurePlaced(result.ClearedCount);
            ScoreSystem.SetComboCount(scoreComboSystem.ComboCount);
            ScoreSystem.AddSetScore(result.ClearedCount, scoreComboSystem.Multiplier);
            UpdateComboViews();
            scoreView.UpdateScore();
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
