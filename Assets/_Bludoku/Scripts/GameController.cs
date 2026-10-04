using _Bludoku.Scripts.Analytics;
using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Core;
using _Bludoku.Scripts.Score;
using _Bludoku.Scripts.UI;
using UnityEngine;

namespace _Bludoku.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        [SerializeField] private ScoreMediator scoreMediator;
        [SerializeField] private UIMediator uiMediator;
        [SerializeField] private Board board;
        [SerializeField] private FiguresController figuresController;

        private readonly AnalyticsKeys analyticsKeys = AnalyticsService.Instance.Settings.Keys;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            figuresController.OnGameOver += HandleGameOver;

            board.LoadGrid();
            figuresController.LoadFigures();

            AnalyticsService.Instance.Track(analyticsKeys.GameStarted);
        }

        public void NewGame()
        {
            BoardSaveLoad.Delete();
            board.ResetBoard();
            figuresController.ResetFigures();
            uiMediator.HideGameOver();
            scoreMediator.ResetScore();

            AnalyticsService.Instance.Track(analyticsKeys.GameStarted, "Player started a new game");
        }

        public void SecondChance()
        {
            uiMediator.HideGameOver();
            figuresController.UpdateToEasyFigures();

            AnalyticsService.Instance.Track(
                analyticsKeys.PowerupUsed,
                "Player used second chance",
                ("powerup", "second_chance"));
        }

        private void HandleGameOver()
        {
            uiMediator.ShowGameOver();

            AnalyticsService.Instance.Track(
                analyticsKeys.GameOver,
                $"Game over with score {ScoreSystem.Data.Score}",
                ("score", ScoreSystem.Data.Score.ToString()),
                ("high_score", ScoreSystem.Data.HighScore.ToString()),
                ("combo", ScoreSystem.Data.ComboCount.ToString()));
        }
    }
}
