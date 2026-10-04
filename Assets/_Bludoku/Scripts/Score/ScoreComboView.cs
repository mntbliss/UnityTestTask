using DG.Tweening;
using TMPro;
using UnityEngine;
using _Bludoku.Scripts.Config;

namespace _Bludoku.Scripts.Score
{
    public class ScoreComboView : MonoBehaviour
    {
        [SerializeField] private TMP_Text comboText;
        [SerializeField] private Transform comboTransform;

        private ComboSettings comboSettings;
        private int currentCombo;
        private Tween pulseTween;

        public void Setup(ComboSettings settings)
        {
            comboSettings = settings;
        }

        private void Awake()
        {
            comboTransform.localScale = Vector3.zero;
        }

        public void UpdateCombo(int comboCount, bool animate = true)
        {
            bool wasVisible = IsVisible(currentCombo);
            bool isVisible = IsVisible(comboCount);
            bool comboChanged = comboCount != currentCombo;

            if (isVisible) comboText.text = string.Format(comboSettings.ComboFormat, comboCount);

            currentCombo = comboCount;

            if (isVisible != wasVisible) Show(isVisible, animate);
            else if (isVisible && animate && comboChanged) PlayComboPunch();
        }

        private bool IsVisible(int comboCount) => comboCount >= comboSettings.MinVisibleCombo;

        private void Show(bool visible, bool animate)
        {
            KillTweens();

            Vector3 targetScale = visible ? Vector3.one : Vector3.zero;

            if (!animate)
            {
                comboTransform.localScale = targetScale;
                if (visible) StartPulse();
                return;
            }

            float duration = visible ? comboSettings.ShowDuration : comboSettings.HideDuration;
            Ease ease = visible ? Ease.OutElastic : Ease.InBack;
            Tween tween = comboTransform.DOScale(targetScale, duration).SetEase(ease);

            if (visible) tween.OnComplete(OnShowComplete);
        }

        private void PlayComboPunch()
        {
            KillPulse();
            comboTransform.localScale = Vector3.one;
            comboTransform.DOPunchScale(Vector3.one * comboSettings.PunchScale, comboSettings.PunchDuration, comboSettings.PunchVibrato, comboSettings.PunchElasticity)
                .OnComplete(OnShowComplete);
        }

        private void OnShowComplete()
        {
            comboTransform.localScale = Vector3.one;
            StartPulse();
        }

        private void StartPulse()
        {
            if (!IsVisible(currentCombo))
                return;

            pulseTween = comboTransform.DOScale(comboSettings.PulseScale, comboSettings.PulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void KillTweens()
        {
            comboTransform.DOKill();
            KillPulse();
        }

        private void KillPulse()
        {
            pulseTween?.Kill();
            pulseTween = null;
        }

        private void OnDisable()
        {
            KillTweens();
        }
    }
}
