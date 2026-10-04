using System;
using System.Collections.Generic;
using UnityEngine;
using _Bludoku.Scripts.Config;
using _Bludoku.Scripts.Save;

namespace _Bludoku.Scripts.Blocks
{
    public class FiguresSaveLoad
    {
        private readonly SaveSettings saveSettings = SaveService.Instance.Settings;

        public void SaveFigures(List<Figure> figures)
        {
            string saveData = "";
            foreach (var figure in figures)
            {
                if (figure != null)
                {
                    saveData += figure.ID + ";";
                }
                else
                {
                    saveData += "-1;";
                }
            }

            SaveService.Instance.Save(saveSettings.FiguresSaveKey, saveData);
        }

        public int[] LoadFigures()
        {
            if (!SaveService.Instance.Has(saveSettings.FiguresSaveKey)) return Array.Empty<int>();

            string saveData = SaveService.Instance.Get(saveSettings.FiguresSaveKey);

            string[] figureIds = saveData.Split(';');
            int[] result = new int[figureIds.Length];
            for (int i = 0; i < figureIds.Length; i++)
            {
                if (int.TryParse(figureIds[i], out int id))
                {
                    result[i] = id;
                }
                else
                {
                    result[i] = -1;
                }
                Debug.Log($"Loaded figure ID: {result[i]} at position {i}");
            }

            return result;
        }
    }
}
