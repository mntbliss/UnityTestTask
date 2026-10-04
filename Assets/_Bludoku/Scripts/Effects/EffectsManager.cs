using System.Collections.Generic;
using _Bludoku.Scripts.Boards;
using _Bludoku.Scripts.Config;
using _Bludoku.Scripts.Helpers.Pooling;
using UnityEngine;

namespace _Bludoku.Scripts.Effects
{
    public class EffectsManager : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Camera gameCamera;
        [SerializeField] private Transform crumbCanvas;
        [SerializeField] private ScoreCrumbView scoreCrumbPrefab;

        private ParticleEffect particleEffect;
        private VibrationEffect vibrationEffect;
        private CameraBumpEffect cameraBumpEffect;
        private MonoPool<ScoreCrumbView> crumbPool;
        private ComboSettings comboSettings;

        private void Awake()
        {
            comboSettings = ConfigService.Instance.Get<ComboSettings>();
            particleEffect = new ParticleEffect(particles);
            vibrationEffect = new VibrationEffect();
            cameraBumpEffect = new CameraBumpEffect(gameCamera.transform);
            crumbPool = new MonoPool<ScoreCrumbView>(scoreCrumbPrefab, crumbCanvas, comboSettings.CrumbMaxCount);
        }

        public void PlayClearEffects(ClearResult result, int scoreGained)
        {
            if (result.ClearedCount <= 0) return;

            vibrationEffect.Play(result);
            particleEffect.Play(result);
            cameraBumpEffect.Play(comboSettings);
            PlayCrumbs(result.ClearedPositions, scoreGained);
        }

        private void PlayCrumbs(List<Vector3> clearedPositions, int scoreGained)
        {
            if (scoreGained <= 0 || clearedPositions.Count == 0) return;

            int crumbCount = Mathf.Min(comboSettings.CrumbMaxCount, clearedPositions.Count);
            int baseScore = scoreGained / crumbCount;
            int remainder = scoreGained - baseScore * crumbCount;

            for (int i = 0; i < crumbCount; i++)
            {
                int crumbScore = baseScore + (i == 0 ? remainder : 0);
                if (crumbScore <= 0) continue;

                ScoreCrumbView crumb = crumbPool.GetFreeElement();
                crumb.transform.position = clearedPositions[i];
                crumb.Play(crumbScore, comboSettings, () => crumbPool.SetFreeElement(crumb));
            }
        }
    }
}
