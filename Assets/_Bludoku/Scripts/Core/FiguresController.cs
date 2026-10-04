using System;
using System.Collections.Generic;
using _Bludoku.Scripts.Analytics;
using _Bludoku.Scripts.Blocks;
using _Bludoku.Scripts.Boards;
using UnityEngine;

namespace _Bludoku.Scripts.Core
{
    public class FiguresController : MonoBehaviour
    {
        public event Action OnGameOver;

        [SerializeField] private Board board;
        [SerializeField] private List<Transform> figurePositions;
        [SerializeField] private int minTotalCells = 20;

        private readonly FiguresSaveLoad saveLoad = new();
        private readonly List<Figure> currentFigures = new();
        private readonly AnalyticsKeys analyticsKeys = AnalyticsService.Instance.Settings.Keys;

        public void LoadFigures()
        {
            int[] savedFigureIds = saveLoad.LoadFigures();
            if (savedFigureIds == null || savedFigureIds.Length == 0)
            {
                UpdateFigures();
            }
            else
            {
                for (int i = 0; i < savedFigureIds.Length; i++)
                {
                    if (savedFigureIds[i] < 0)
                        continue;

                    Figure newFigure = FigureFactory.GetByIndex(savedFigureIds[i]);

                    Debug.Log($"Loaded figure ID: {savedFigureIds[i]} at position {i}");
                    RegisterFigure(newFigure, i);
                }
            }

            CheckPlaceability();
        }

        public void UpdateFigures(int totalCells = -1)
        {
            List<int> indices;
            if (totalCells < 0)
            {
                indices = FigureSuggestion.SelectShapeIndices(board.GetGrid(), FigureFactory.Shapes,
                    figurePositions.Count, minTotalCells);
            }
            else
            {
                indices = FigureSuggestion.SelectShapeIndices(board.GetGrid(), FigureFactory.Shapes,
                    figurePositions.Count, totalCells);
            }

            for (int i = 0; i < figurePositions.Count; i++)
            {
                Figure newFigure = i < indices.Count
                    ? FigureFactory.GetByIndex(indices[i])
                    : FigureFactory.GetRandom();

                RegisterFigure(newFigure, i);
            }

            saveLoad.SaveFigures(currentFigures);
        }

        public void ResetFigures()
        {
            foreach (var figure in currentFigures)
            {
                Destroy(figure.gameObject);
            }
            currentFigures.Clear();

            UpdateFigures();
        }

        public void UpdateToEasyFigures()
        {
            foreach (var figure in currentFigures)
            {
                Destroy(figure.gameObject);
            }
            currentFigures.Clear();

            UpdateFigures(1);
        }

        private void FigurePicked(Figure figure)
        {
            AnalyticsService.Instance.Track(
                analyticsKeys.PieceMoveStarted,
                $"Player started moving figure {figure.ID}",
                ("figure_id", figure.ID.ToString()));
        }

        private void FigureDragged(Figure figure)
        {
            board.UpdateHighlight(figure);
        }

        private void FigureReleased(Figure figure)
        {
            board.ClearHighlight();

            if (board.CanPlaceFigure(figure))
                PlaceFigure(figure);
            else
                figure.SnapBack();
        }

        private void RegisterFigure(Figure figure, int index)
        {
            figure.SetInitialPosition(figurePositions[index]);
            figure.transform.position = figurePositions[index].position;

            figure.OnPicked += FigurePicked;
            figure.OnDragged += FigureDragged;
            figure.OnReleased += FigureReleased;

            currentFigures.Add(figure);
        }

        private void PlaceFigure(Figure figure)
        {
            figure.OnPicked -= FigurePicked;
            figure.OnDragged -= FigureDragged;
            figure.OnReleased -= FigureReleased;

            ClearResult clearResult = board.SetFigure(figure);

            AnalyticsService.Instance.Track(
                analyticsKeys.PiecePlaced,
                $"Player placed figure {clearResult.FigureId} at ({clearResult.PlaceX}, {clearResult.PlaceY})",
                ("x", clearResult.PlaceX.ToString()),
                ("y", clearResult.PlaceY.ToString()),
                ("figure_id", clearResult.FigureId.ToString()));

            if (clearResult.ClearedCount > 0)
            {
                AnalyticsService.Instance.Track(
                    analyticsKeys.PieceDeleted,
                    $"Cleared {clearResult.ClearedCount} cells after placing figure {clearResult.FigureId}",
                    ("x", clearResult.PlaceX.ToString()),
                    ("y", clearResult.PlaceY.ToString()),
                    ("cleared_count", clearResult.ClearedCount.ToString()),
                    ("figure_id", clearResult.FigureId.ToString()));
            }

            currentFigures.Remove(figure);
            Destroy(figure.gameObject);

            if (currentFigures.Count == 0)
                UpdateFigures();

            CheckPlaceability();

            saveLoad.SaveFigures(currentFigures);
        }

        private void CheckPlaceability()
        {
            bool anyCanBePlaced = false;

            foreach (Figure figure in currentFigures)
            {
                bool canPlace = board.CanPlaceAnywhere(figure.Grid);
                figure.SetPlaceable(canPlace);
                if (canPlace) anyCanBePlaced = true;
            }

            if (!anyCanBePlaced && currentFigures.Count > 0) OnGameOver?.Invoke();
        }
    }
}
