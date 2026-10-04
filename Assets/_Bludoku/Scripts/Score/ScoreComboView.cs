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

            if (isVisible)
                comboText.text = string.Format(comboSettings.ComboFormat, comboCount);

            if (isVisible && !wasVisible)
                Show(animate);
            else if (!isVisible && wasVisible)
                Hide(animate);
            else if (isVisible && animate && comboCount != currentCombo)
                PlayComboPunch();

            currentCombo = comboCount;
        }

        private bool IsVisible(int comboCount) => comboCount >= comboSettings.MinVisibleCombo;

        private void Show(bool animate)
        {
            KillTweens();

            if (!animate)
            {
                comboTransform.localScale = Vector3.one;
                StartPulse();
                return;
            }

            comboTransform.DOScale(Vector3.one, comboSettings.ShowDuration)
                .SetEase(Ease.OutElastic)
                .OnComplete(OnShowComplete);
        }

        private void Hide(bool animate)
        {
            KillTweens();

            if (!animate)
            {
                comboTransform.localScale = Vector3.zero;
                return;
            }

            comboTransform.DOScale(Vector3.zero, comboSettings.HideDuration).SetEase(Ease.InBack);
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
