using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using _Bludoku.Scripts.Config;
using _Bludoku.Scripts.Helpers.Pooling;

namespace _Bludoku.Scripts.Effects
{
    public class ScoreCrumbView : MonoPoolableItem
    {
        [SerializeField] private Transform crumbTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text scoreText;

        private Tween activeTween;

        public override void SetPoolable(bool value)
        {
            if (IsPoolable == value) return;
            base.SetPoolable(value);

            if (value) KillTween();
            gameObject.SetActive(!value);
        }

        public void Play(int score, ComboSettings settings, Action onComplete)
        {
            KillTween();

            scoreText.text = string.Format(settings.CrumbFormat, score);
            crumbTransform.localEulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(settings.CrumbRotationRange.x, settings.CrumbRotationRange.y));
            canvasGroup.alpha = 1f;

            float targetY = crumbTransform.position.y + settings.CrumbFlyHeight;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(crumbTransform.DOMoveY(targetY, settings.CrumbMoveDuration).SetEase(Ease.OutCubic));
            sequence.Join(canvasGroup.DOFade(0f, settings.CrumbFadeDuration));
            sequence.OnComplete(() => onComplete?.Invoke());
            activeTween = sequence;
        }

        private void KillTween()
        {
            if (activeTween != null && activeTween.IsActive()) activeTween.Kill();

            activeTween = null;
            crumbTransform.DOKill();
            canvasGroup.DOKill();
            canvasGroup.alpha = 1f;
        }
    }
}
